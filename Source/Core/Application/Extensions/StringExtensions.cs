using System.Text;

namespace Application.Extensions
{
    public static class StringExtensions
    {
        public static byte[] GetBytes(this string str) => Encoding.Default.GetBytes(str);
    }
}
