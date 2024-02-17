using System.Text;

namespace Application.Extensions
{
    public static class ByteExtensions
    {
        public static string GetString(this byte[] bytes) => Encoding.Default.GetString(bytes);
    }
}
