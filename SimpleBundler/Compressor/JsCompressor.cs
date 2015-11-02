using Yahoo.Yui.Compressor;

namespace SimpleBundler.Compressor
{
    public class JsCompressor : ICompressor
    {
        public string Compress(string contents)
        {
            var jsCompressor = new JavaScriptCompressor();
            return jsCompressor.Compress(contents);
        }
    }
}
