using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace YZPortal.Core.Attributes
{
    public class Attribute
    {
        [AttributeUsage(AttributeTargets.Property)]
        public sealed class ListRequired : RequiredAttribute
        {
            public override bool IsValid(object? value)
            {
                var list = value != null ? value as IEnumerable : default;
                return list != null && list.GetEnumerator().MoveNext();
            }
        }
    }
}
