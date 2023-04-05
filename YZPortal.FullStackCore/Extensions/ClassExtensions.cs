using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace YZPortal.Core.Extensions
{
    public static class ClassExtensions
    {
        // Not tested yet
        public static Tuple<bool, string, string> GetDataAnotationDetail<T>(T item) where T : class
        {
            Tuple<bool, string, string> result = null;

            var tableName = string.Empty;
            var tableAttr = item.GetType().GetCustomAttributes().FirstOrDefault() as TableAttribute;

            if (tableAttr != null)
            {
                tableName = tableAttr.Name;
            }

            var properties = item.GetType().GetProperties();
            bool breakcondition = false;
            foreach (var property in properties)
            {
                Attribute[] attrs = Attribute.GetCustomAttributes(property);
                if (attrs != null)
                {
                    foreach (Attribute attr in attrs)
                    {

                        if (attr is KeyAttribute)
                        {
                            var a = (KeyAttribute)attr;
                            var obj = property.GetValue(item, null);
                            result = Tuple.Create(true, tableName, Convert.ToString(obj));
                            breakcondition = true;
                            break;
                        }

                    }
                }
                if (breakcondition)
                {
                    break;
                }
            }

            return result;
        }

        public static string GetAttributeTableName<T>(T item) where T : class
        {
            var tableAttr = item.GetType().GetCustomAttributes().FirstOrDefault() as TableAttribute;

            if (tableAttr != null)
            {
                var tableName = tableAttr.Name;
                return tableName;
            }

            return null;
        }
    }
}
