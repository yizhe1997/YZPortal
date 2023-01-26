using AutoMapper;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using MediatR;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;
using System.Data;
using System.Net;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;

namespace YZPortal.API.Controllers.Memberships.Invites
{
    public class InviteXLSM
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

            public RequestHandler(DealerPortalContext dbContext, FunctionApiContext apiContext, IMapper mapper, IHttpContextAccessor httpContext, CurrentUserContext userAccessor, BlobServiceClient blobClient, IOptions<AzureStorageOptions> options) : base(dbContext, apiContext, mapper, httpContext, userAccessor)
            {
                BlobClient = blobClient;
                StorageOptions = options.Value;
            }

            public override async Task<Model> Handle(Request request, CancellationToken cancellationToken)
            {
                #region Dwnload Sheet from Azure

                // Check X in "%VerX.xlsm" for version control. Selecting latest for download
                // Alternative is just to use latest modified to reduce code, but why risk it
                var containerName = StorageOptions.InviteSheetXLSMContainerName.CheckIfContainerNameIsNullOrEmpty(nameof(StorageOptions.InviteSheetXLSMContainerName));
                BlobContainerClient containerClient = BlobClient.GetBlobContainerClient(containerName);
                var blobs = containerClient.GetBlobsAsync(prefix: "BulkInviteVer");
                var listOfFileNames = new List<string>();
                var listOfver = new List<string>();

                await foreach (var blob in blobs)
                {
                    var blobFileName = blob.Name;
                    listOfFileNames.Add(blobFileName);
                    blobFileName = Regex.Replace(blobFileName, "[^0-9]", "");
                    listOfver.Add(blobFileName);
                }

                if (!listOfver.Any())
                    throw new RestException(HttpStatusCode.NoContent, $"No file with prefix \"BulkInviteVer\" found in container for {StorageOptions.InviteSheetXLSMContainerName}, please contact admin!");

                var latestVer = listOfver.Select(s => { int i; return int.TryParse(s, out i) ? i : (int?)null; })
                                    .Where(i => i.HasValue)
                                    .Select(i => i.Value)
                                    .ToList().Max();

                var latestBlobName = listOfFileNames.FirstOrDefault(x => x.Contains(latestVer.ToString()));

                MemoryStream outputStream = new MemoryStream();
                await outputStream.DwnloadAttachmentFromBlob(BlobClient, containerName, latestBlobName);

                #endregion

                //Get list of dealers, dealer role and access levels fromd db and store into dataset
                DataSet ds = new DataSet();

                var dealers = await Database.Dealers.Select(x => x.CustomerAccount).ToListAsync();
                dealers.Add(new string("All"));
                var dealerRoleTypes = await Database.DealerRoles.Select(x => x.Name).ToListAsync();
                var accessLevelTypes = await Database.AccessLevels.Select(x => x.Name).ToListAsync();
                ds.Tables.Add(ToDataTable(dealers));
                ds.Tables.Add(ToDataTable(dealerRoleTypes));
                ds.Tables.Add(ToDataTable(accessLevelTypes));

                try
                {
                    using (SpreadsheetDocument spreadsheet = SpreadsheetDocument.Open(outputStream, true))
                    {
                        spreadsheet.ChangeDocumentType(SpreadsheetDocumentType.MacroEnabledWorkbook);
                        spreadsheet.WorkbookPart.Workbook.CalculationProperties.ForceFullCalculation = true;
                        spreadsheet.WorkbookPart.Workbook.CalculationProperties.FullCalculationOnLoad = true;

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
                            foreach (DataRow dsrow in table.Rows)
                            {
                                foreach (DataColumn column in table.Columns)
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

            #region Helpers

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
                shareStringPart.SharedStringTable.AppendChild(new SharedStringItem(new Text(text)));
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
                DataColumn workCol = dataTable.Columns.Add("foo", typeof(string));
                foreach (var row in list)
                {
                    dataTable.Rows.Add(row);
                }
                return dataTable;
            }

            #endregion
        }
    }
}
