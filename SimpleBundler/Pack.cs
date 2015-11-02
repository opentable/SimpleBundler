namespace SimpleBundler
{
    public static class Pack
    {
        public static CssPack Css()
        {
            return new CssPack();
        }

        public static JavaScriptPack JavaScript()
        {
            return new JavaScriptPack();
        }
    }
}
