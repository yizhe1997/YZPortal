using System.ComponentModel;
using OfficeOpenXml;

namespace Infrastructure.Extensions.OfficeOpenXmlExtensions
{
    public static class ExcelWorksheetExtensions
    {
        /// <summary>
        /// Return a generic list from the excelworksheet based on the properties defined in the generic class.
        /// </summary>
        /// <param name="IgnoreRowIfFirstColumnIsEmpty">
        /// Will not add row to list if first column is empty. Set this to true to avoid having to deal with unexpected issues
        /// caused by dropdowns or default value of custom datetime/datetime (1/1/0001 12:00:00AM if datetime, 0 if custome datetime).
        /// BY NO MEANS IS THIS THE PERMANENT FIX, developer can explore other nugets as an alternative.
        /// </param>
        public static List<T> GetList<T>(this ExcelWorksheet sheet, bool IgnoreRowIfFirstColumnIsEmpty = false)
        {
            var list = new List<T>();

            // First row is for knowing the properties of object
            var columnInfo = Enumerable.Range(1, sheet.Dimension.Columns)
                .Select(n => new { Index = n, ColumnName = sheet.Cells[1, n].Value?.ToString()?.Trim() })
                .Where(c => !string.IsNullOrWhiteSpace(c.ColumnName) && !string.IsNullOrEmpty(c.ColumnName))
                .ToList();

            // Get properties of T
            var properties = typeof(T).GetProperties();

            for (int row = 2; row <= sheet.Dimension.Rows; row++)
            {
                // Flag to skip insert
                var skipRowFlag = false;

                T obj = (T)Activator.CreateInstance(typeof(T));

                foreach (var colInfo in columnInfo)
                {
                    var displayName = colInfo.ColumnName;
                    // Fall back to using the field name if DisplayName attributes do not match
                    var prop = properties.FirstOrDefault(p =>
                        p.GetCustomAttributes(typeof(DisplayNameAttribute), true)
                            .OfType<DisplayNameAttribute>()
                            .Any(attr => attr.DisplayName.Equals(displayName, StringComparison.OrdinalIgnoreCase))
                    ) ?? properties.FirstOrDefault(p => p.Name.Equals(displayName, StringComparison.OrdinalIgnoreCase));
                    if (prop != null)
                    {
                        int col = colInfo.Index;
                        var val = sheet.Cells[row, col].Value?.ToString()?.Trim();
                        var propType = prop.PropertyType;

                        skipRowFlag = IgnoreRowIfFirstColumnIsEmpty && col == 1 && string.IsNullOrEmpty(val);

                        if (skipRowFlag)
                            break;

                        prop.SetValue(obj, val != null ? Convert.ChangeType(val, propType) : null);
                    }
                }

                if (!skipRowFlag)
                    list.Add(obj);
            }

            return list;
        }
    }
}
