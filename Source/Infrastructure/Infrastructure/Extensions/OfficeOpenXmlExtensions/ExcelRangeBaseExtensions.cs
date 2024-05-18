using Infrastructure.Attributes.EPP;
using OfficeOpenXml;
using System.Reflection;

namespace Infrastructure.Extensions.OfficeOpenXmlExtensions
{
    public static class ExcelRangeBaseExtensions
    {
        /// <summary>
        /// Used in conjuction with the mocked EpplusIgnore attribute to ignore mapping for certain properties.
        /// Use LoadFromCollection<T> if filtering is not necessary.
        /// </summary>
        public static ExcelRangeBase LoadFromCollectionFiltered<T>(this ExcelRangeBase excelRangeBase, IEnumerable<T> collection, bool printHeaders = true) where T : class
        {
            MemberInfo[] membersToInclude = typeof(T)
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => !Attribute.IsDefined(p, typeof(EpplusIgnore)))
                .ToArray();

            return excelRangeBase.LoadFromCollection(collection, printHeaders,
                OfficeOpenXml.Table.TableStyles.None,
                BindingFlags.Instance | BindingFlags.Public,
                membersToInclude);
        }
    }
}
