namespace NetEngine.Renderer
{
    using Utilities;

    public static class AssetManagerExtensions
    {
        public static StaticMesh ResolveStaticMesh(this AssetManager assetManager, string name)
        {
            return assetManager.LoadAsset<StaticMesh>(name);
        }

        public static Material ResolveMaterial(this AssetManager assetManager, string name)
        {
            return assetManager.LoadAsset<Material>(name);
        }
    }
}
