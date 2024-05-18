using OfficeOpenXml;

namespace Infrastructure.Extensions.OfficeOpenXmlExtensions
{
    public static class ExcelPackageExtensions
    {
        /// <summary>
        /// Creates a new worksheet for the excelpackage with headers based on the generic class T
        /// </summary>
        /// <param name="printHeaders">Print the property names on the first row. If the property is decorated with
        /// a System.ComponentModel.DisplayNameAttribute or a System.ComponentModel.DescriptionAttribute
        /// that attribute will be used instead of the reflected member name.</param>
        public static void CreateWorksheetWithHeaders<T>(this ExcelPackage package, string sheetName, bool printHeaders) where T : class
        {
            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(sheetName);

            worksheet.Cells.LoadFromCollection(new List<T> { }, printHeaders);
        }

        /// <summary>
        /// Creates a new worksheet for the excelpackage with headers based on the generic class T and
        /// load data of type T into the newly created worksheet
        /// </summary>
        /// <param name="printHeaders">Print the property names on the first row. If the property is decorated with
        /// a System.ComponentModel.DisplayNameAttribute or a System.ComponentModel.DescriptionAttribute
        /// that attribute will be used instead of the reflected member name.</param>
        /// <param name="isFilterPropertyEnabled">If this is enabled, properties with EpplusIgnore attribute 
        /// will not be loaded into collection</param>
        public static void CreateWorksheetWithHeadersAndData<T>(this ExcelPackage package, List<T> data, bool printHeaders, string sheetName, bool isFilterPropertyEnabled) where T : class
        {
            var worksheet = package.Workbook.Worksheets.Add(sheetName);

            if (isFilterPropertyEnabled)
            {
                worksheet.Cells.LoadFromCollectionFiltered(data, printHeaders);
            }
            else
            {
                worksheet.Cells.LoadFromCollection(data, printHeaders);
            }

            package.Save();
        }
    }
}
