using Yahoo.Yui.Compressor;

namespace SimpleBundler.Compressor
{
    public class StyleSheetCompressor : ICompressor
    {
        public string Compress(string contents)
        {
            var cssCompressor = new CssCompressor();
            return cssCompressor.Compress(contents);
        }
    }
}
