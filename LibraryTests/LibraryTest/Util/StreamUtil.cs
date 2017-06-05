using System.IO;

namespace LibraryTests.LibraryTest.Util
{
    public class StreamUtil
    {
        public static Stream StreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
