namespace NetEngine.Renderer
{
    using Utilities;

    /// <summary>
    /// Some asset manager extensions to simplify the call site for loading stuff.
    /// </summary>
    public static class AssetManagerExtensions
    {
        /// <summary>
        /// Given a name, loads a static mesh.
        /// </summary>
        /// <param name="assetManager">The asset manager instance to load from.</param>
        /// <param name="name">The name of the mesh to load.</param>
        /// <returns>A static mesh instance, or null if it can't be loaded.</returns>
        public static StaticMesh ResolveStaticMesh(this AssetManager assetManager, string name)
        {
            return assetManager.LoadAsset<StaticMesh>(name);
        }

        /// <summary>
        /// Given a name, loads a material.
        /// </summary>
        /// <param name="assetManager">The asset manager instance to load from.</param>
        /// <param name="name">The name of the material to load.</param>
        /// <returns>A material, or null if it can't be loaded.</returns>
        public static Material ResolveMaterial(this AssetManager assetManager, string name)
        {
            return assetManager.LoadAsset<Material>(name);
        }
    }
}
