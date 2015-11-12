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
            var contents = Utils.GetCompressedContents(pack.Files, pack.BasePath, new StyleSheetCompressor());
            CompressedCssContentsInternal.Add(pack.Name, contents);

            if (string.IsNullOrWhiteSpace(pack.CacheBustingString))
            {
                pack.CacheBustingString = Utils.GetSignature(contents);
            }

            switch (pack.CacheBustingMethod)
            {
                case CacheBustingMethod.Path:
                    var compressedTemplate = "<link rel=\"stylesheet\" type=\"text/css\" href=\"/bundles/css/{0}/{1}\" />";
                    CompressedCssTagsInternal.Add(pack.Name, string.Format(compressedTemplate, pack.CacheBustingString, pack.Name));
                    break;

                default:
                    compressedTemplate = "<link rel=\"stylesheet\" type=\"text/css\" href=\"/bundles/css/{0}?r={1}\" />";
                    CompressedCssTagsInternal.Add(pack.Name, string.Format(compressedTemplate, pack.Name, pack.CacheBustingString));
                    break;
            }
        }
    }
}
