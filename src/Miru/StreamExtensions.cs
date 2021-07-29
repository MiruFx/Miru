using System.IO;

namespace Miru
{
    public static class StreamExtensions
    {
        public static byte[] ToBytes(this Stream input)
        {
            using var ms = new MemoryStream();

            if (input.CanSeek)
                input.Position = 0;
            
            input.CopyTo(ms);
            
            return ms.ToArray();
        }
    }
}