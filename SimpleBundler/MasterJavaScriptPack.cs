using System.Collections.Generic;
using System.Linq;
using SimpleBundler.Compressor;

namespace SimpleBundler
{
    internal class MasterJavaScriptPack
    {
        private static readonly Dictionary<string, string> JavaScriptTagsInternal = new Dictionary<string, string>();

        private static readonly Dictionary<string, string> CompressedJavaScriptTagsInternal = new Dictionary<string, string>();

        private static readonly Dictionary<string, string> CompressedJavaScriptContentsInternal = new Dictionary<string, string>();

        public static IDictionary<string, string> JavaScriptTags
        {
            get
            {
                return JavaScriptTagsInternal;
            }
        }

        public static IDictionary<string, string> CompressedJavaScriptTags
        {
            get
            {
                return CompressedJavaScriptTagsInternal;
            }
        }

        public static IDictionary<string, string> CompressedJavaScriptContents
        {
            get
            {
                return CompressedJavaScriptContentsInternal;
            }
        }

        public static void AddJavaScriptPack(JavaScriptPack pack)
        {
            BuildJavaScript(pack);
            if (!pack.SkipBundling)
            {
                BuildCompressedJavaScript(pack);
            }

        }

        private static void BuildJavaScript(JavaScriptPack pack)
        {
            const string javascriptTemplate = "<script src=\"/{0}\"></script>";
            var fileList = pack.Files.ToList();
            var text = string.Join(string.Empty, fileList.Select(file => string.Format(javascriptTemplate, file)).ToArray());
            JavaScriptTagsInternal.Add(pack.Name, text);
        }

        private static void BuildCompressedJavaScript(JavaScriptPack pack)
        {
            var contents = Utils.GetCompressedContents(pack.Files, pack.BasePath, new JsCompressor());
            CompressedJavaScriptContentsInternal.Add(pack.Name, contents);

            if (string.IsNullOrWhiteSpace(pack.CacheBustingString))
            {
                pack.CacheBustingString = Utils.GetSignature(contents);
            }

            switch (pack.CacheBustingMethod)
            {
                case CacheBustingMethod.VaryByUrlPath:
                    var compressedTemplate = "<script src=\"/bundles/js/{0}/{1}\"></script>";
                    CompressedJavaScriptTagsInternal.Add(pack.Name, string.Format(compressedTemplate, pack.CacheBustingString, pack.Name));
                    break;

                default:
                    compressedTemplate = "<script src=\"/bundles/js/{0}?r={1}\"></script>";
                    CompressedJavaScriptTagsInternal.Add(pack.Name, string.Format(compressedTemplate, pack.Name, pack.CacheBustingString));
                    break;
            }
        }
    }
}
