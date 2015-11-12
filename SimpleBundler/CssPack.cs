using System.Collections.Generic;

namespace SimpleBundler
{
    public class CssPack
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

        public CacheBustingMethod CacheBustingMethod
        {
            get;
            set;
        }

        public string CacheBustingString
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
            MasterCssPack.AddCssPack(this);
        }


        public string RenderNormalTags(string name)
        {
            return MasterCssPack.CssTags[name];
        }

        public string RenderBundleTags(string name)
        {
            return MasterCssPack.CompressedCssTags[name];
        }

        public string RenderContents(string name)
        {
            return MasterCssPack.CompressedCssContents[name];
        }
    }
}
