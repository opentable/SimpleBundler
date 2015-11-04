using System.Globalization;
using System.Text;
using Yahoo.Yui.Compressor;

namespace SimpleBundler.Compressor
{
    public class JsCompressor : ICompressor
    {
        public string Compress(string contents)
        {
            var jsCompressor = new JavaScriptCompressor { Encoding = Encoding.UTF8, ThreadCulture = CultureInfo.InvariantCulture };
            return jsCompressor.Compress(contents);
        }
    }
}
