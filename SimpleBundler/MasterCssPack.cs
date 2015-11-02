using System.Collections.Generic;
using System.Linq;
using SimpleBundler.Compressor;

namespace SimpleBundler
{
    internal class MasterCssPack
    {
        private static readonly Dictionary<string, string> CssTagsInternal = new Dictionary<string, string>();

        private static readonly Dictionary<string, string> CompressedCssTagsInternal = new Dictionary<string, string>();

        private static readonly Dictionary<string, string> CompressedCssContentsInternal = new Dictionary<string, string>();

        public static IDictionary<string, string> CssTags
        {
            get
            {
                return CssTagsInternal;
            }
        }

        public static IDictionary<string, string> CompressedCssTags
        {
            get
            {
                return CompressedCssTagsInternal;
            }
        }

        public static IDictionary<string, string> CompressedCssContents
        {
            get
            {
                return CompressedCssContentsInternal;
            }
        }

        public static void AddCssPack(CssPack pack)
        {
            BuildCss(pack);
            if (!pack.SkipBundling)
            {
                BuildCompressedCss(pack);
            }
        }

        private static void BuildCss(CssPack pack)
        {
            const string cssTemplate = "<link rel=\"stylesheet\" type=\"text/css\" href=\"/{0}\" />";
            var fileList = pack.Files.ToList();
            var text = string.Join(string.Empty, fileList.Select(file => string.Format(cssTemplate, file)).ToArray());
            CssTagsInternal.Add(pack.Name, text);
        }

        private static void BuildCompressedCss(CssPack pack)
        {
            const string compressedTemplate = "<link rel=\"stylesheet\" type=\"text/css\" href=\"/bundles/css/{0}?r={1}\" />";
            var contents = PackerUtils.GetCompressedContents(pack.Files, pack.BasePath, new StyleSheetCompressor());
            CompressedCssContentsInternal.Add(pack.Name, contents);
            var contentHash = PackerUtils.GetSignature(contents);
            CompressedCssTagsInternal.Add(pack.Name, string.Format(compressedTemplate, pack.Name, contentHash));
        }
    }
}
