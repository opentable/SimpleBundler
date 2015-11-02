using System.Collections.Generic;

namespace SimpleBundler
{
    public class JavaScriptPack
    {
        private readonly List<string> _files = new List<string>();

        public string Name
        {
            get;
            private set;
        }

        public string BasePath
        {
            get;
            set;
        }

        public bool SkipBundling
        {
            get;
            set;
        }

        public IEnumerable<string> Files
        {
            get
            {
                return _files;
            }
        }

        public void Add(string file)
        {
            _files.Add(file);
        }

        public void AsNamed(string name)
        {
            Name = name;
            MasterJavaScriptPack.AddJavaScriptPack(this);
        }

        public string RenderNormalTags(string name)
        {
            return MasterJavaScriptPack.JavaScriptTags[name];
        }

        public string RenderBundleTags(string name)
        {
            return MasterJavaScriptPack.CompressedJavaScriptTags[name];
        }

        public string RenderContents(string name)
        {
            return MasterJavaScriptPack.CompressedJavaScriptContents[name];
        }
    }
}
