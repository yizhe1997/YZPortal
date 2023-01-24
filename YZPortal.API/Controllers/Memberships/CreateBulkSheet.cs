using Azure.Storage.Blobs;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;
using System.Net;
using System.Data;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using YZPortal.Core.Domain.Contexts;

namespace YZPortal.Api.Controllers.Memberships
{
    public class CreateBulkSheet
    {
        public class Request : IRequest<Model>
        {
        }

        public class Model
        {
            public MemoryStream Stream { get; set; }
            public string MimeType { get; set; }
            public string FileName { get; set; }
        }

        public class RequestHandler : BaseRequestHandler<Request, Model>
        {
            BlobServiceClient BlobClient { get; set; }
            AzureStorageOptions StorageOptions { get; set; }

            public RequestHandler(PortalContext dbContext, IMapper mapper, IHttpContextAccessor httpContext, CurrentContext userAccessor, BlobServiceClient blobClient, IOptions<AzureStorageOptions> options) : base(dbContext, mapper, httpContext, userAccessor)
            {
                BlobClient = blobClient;
                StorageOptions = options.Value;
            }

            public override async Task<Model> Handle(Request request, CancellationToken cancellationToken)
            {
                // Check X in "%VerX.xlsm" for version control. Selecting latest for download
                // Alternative is just to use latet modified to reduce code, but why risk it

                BlobContainerClient containerClient = BlobClient.GetBlobContainerClient(StorageOptions.MembershipsCreateBulkSheetContainerName);
                
                var fileVerDict = new Dictionary<int, string>();

                await foreach (var blobItem in containerClient.GetBlobsAsync(cancellationToken: cancellationToken))
                {
                    var blobFileName = Regex.Replace(blobItem.Name, "[^0-9]", "");

                    var parseCheck = int.TryParse(blobFileName, out int fileVer);

                    if (parseCheck)
                        fileVerDict.Add(fileVer, blobItem.Name);
                    else
                        throw new RestException(HttpStatusCode.BadRequest, "Error processing blob uri");
                }

                var latestBlobName = fileVerDict.GetValueOrDefault(fileVerDict.Max(d => d.Key), null);

                if (latestBlobName == null)
                    throw new RestException(HttpStatusCode.BadRequest, "Blob missing in middle of process");

                var latestBlob = containerClient.GetBlobClient(latestBlobName);

                var outputStream = new MemoryStream();

                await latestBlob.DownloadToAsync(outputStream, cancellationToken);

                outputStream.Position = 0;


                // TO DO: use the get constants...
                //Get list of dealers, dealer role and access levels fromd db and store into dataset
                DataSet ds = new DataSet();
                ds.Tables.Add(ToDataTable(Database.Dealers.Select(x => x.Name).ToList()));
                ds.Tables.Add(ToDataTable(typeof(DealerRoleTypes).GetConstants().ToList()));
                ds.Tables.Add(ToDataTable(typeof(ContentAccessLevelTypes).GetConstants().ToList()));

                // TODO: having diificulty with resizing the table size in xlsm when inserting data, its hardcoded to around 600-800 atm
                try
                {
                    using (SpreadsheetDocument spreadsheet = SpreadsheetDocument.Open(outputStream, true))
                    {
                        SharedStringTablePart shareStringPart;
                        if (spreadsheet.WorkbookPart.GetPartsOfType<SharedStringTablePart>().Count() > 0)
                        {
                            shareStringPart = spreadsheet.WorkbookPart.GetPartsOfType<SharedStringTablePart>().First();
                        }
                        else
                        {
                            throw new RestException(HttpStatusCode.UnprocessableEntity, "Missing shared string table.");
                        }

                        WorksheetPart worksheetPart = RetrieveSheetPartByName(spreadsheet, "SourceData");
                        int index;
                        // equivalent A
                        int ascii = 65;

                        foreach (DataTable table in ds.Tables)
                        {
                            uint rowIndex = 3;
                            foreach (System.Data.DataRow dsrow in table.Rows)
                            {
                                foreach (System.Data.DataColumn column in table.Columns)
                                {
                                    index = InsertSharedStringItem(dsrow[column].ToString(), shareStringPart);

                                    Cell cell = InsertCellInWorksheet(Convert.ToChar(ascii).ToString(), rowIndex, worksheetPart);
                                    cell.CellValue = new CellValue(index.ToString());
                                    cell.DataType = new EnumValue<CellValues>(CellValues.SharedString);
                                }
                                rowIndex++;
                            }
                            ascii++;
                        }
                        spreadsheet.WorkbookPart.Workbook.CalculationProperties.ForceFullCalculation = true;
                        spreadsheet.WorkbookPart.Workbook.CalculationProperties.FullCalculationOnLoad = true;
                        worksheetPart.Worksheet.Save();
                    }

                    outputStream.Position = 0;

                    return new Model
                    {
                        Stream = outputStream,
                        MimeType = "application/xlsm",
                        FileName = $"{latestBlobName}"
                    };
                }
                catch (Exception err)
                {
                    throw new RestException(HttpStatusCode.UnprocessableEntity, err.Message);
                }
            }

            // Given a column name, a row index, and a WorksheetPart, inserts a cell into the worksheet. 
            // If the cell already exists, returns it. 
            private static Cell InsertCellInWorksheet(string columnName, uint rowIndex, WorksheetPart worksheetPart)
            {
                Worksheet worksheet = worksheetPart.Worksheet;
                SheetData sheetData = worksheet.GetFirstChild<SheetData>();
                string cellReference = columnName + rowIndex;

                // If the worksheet does not contain a row with the specified row index, insert one.
                Row row;
                if (sheetData.Elements<Row>().Where(r => r.RowIndex == rowIndex).Count() != 0)
                {
                    row = sheetData.Elements<Row>().Where(r => r.RowIndex == rowIndex).First();
                }
                else
                {
                    row = new Row() { RowIndex = rowIndex };
                    sheetData.Append(row);
                }

                // If there is not a cell with the specified column name, insert one.  
                if (row.Elements<Cell>().Where(c => c.CellReference.Value == columnName + rowIndex).Count() > 0)
                {
                    return row.Elements<Cell>().Where(c => c.CellReference.Value == cellReference).First();
                }
                else
                {
                    // Cells must be in sequential order according to CellReference. Determine where to insert the new cell.
                    Cell refCell = null;
                    foreach (Cell cell in row.Elements<Cell>())
                    {
                        if (string.Compare(cell.CellReference.Value, cellReference, true) > 0)
                        {
                            refCell = cell;
                            break;
                        }
                    }

                    Cell newCell = new Cell() { CellReference = cellReference };
                    row.InsertBefore(newCell, refCell);

                    worksheet.Save();
                    return newCell;
                }
            }

            //Given text and a SharedStringTablePart, creates a SharedStringItem with the specified text
            //and inserts it into the SharedStringTablePart.If the item already exists, returns its index.
            private static int InsertSharedStringItem(string text, SharedStringTablePart shareStringPart)
            {
                // If the part does not contain a SharedStringTable, create one.
                if (shareStringPart.SharedStringTable == null)
                {
                    shareStringPart.SharedStringTable = new SharedStringTable();
                }

                int i = 0;

                // Iterate through all the items in the SharedStringTable. If the text already exists, return its index.
                foreach (SharedStringItem item in shareStringPart.SharedStringTable.Elements<SharedStringItem>())
                {
                    if (item.InnerText == text)
                    {
                        return i;
                    }

                    i++;
                }

                // The text does not exist in the part. Create the SharedStringItem and return its index.
                shareStringPart.SharedStringTable.AppendChild(new SharedStringItem(new DocumentFormat.OpenXml.Spreadsheet.Text(text)));
                shareStringPart.SharedStringTable.Save();

                return i;
            }

            //retrieve sheetpart            
            public WorksheetPart RetrieveSheetPartByName(SpreadsheetDocument document,
             string sheetName)
            {
                IEnumerable<Sheet> sheets = document.WorkbookPart.Workbook.GetFirstChild<Sheets>().
                Elements<Sheet>().Where(s => s.Name == sheetName);
                if (sheets.Count() == 0)
                    return null;

                string relationshipId = sheets.First().Id.Value;
                WorksheetPart worksheetPart = (WorksheetPart)
                document.WorkbookPart.GetPartById(relationshipId);
                return worksheetPart;
            }

            public static DataTable ToDataTable(List<string> list)
            {
                DataTable dataTable = new DataTable();
                DataColumn workCol = dataTable.Columns.Add("foo", typeof(String));
                foreach (var row in list)
                {
                    dataTable.Rows.Add(row);
                }
                return dataTable;
            }
        }
    }
}
