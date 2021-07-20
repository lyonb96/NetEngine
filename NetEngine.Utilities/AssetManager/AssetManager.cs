namespace NetEngine.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Handles loading assets and resolving their dependencies
    /// </summary>
    public class AssetManager
    {
        /// <summary>
        /// A registry of loaded assets
        /// </summary>
        private readonly Dictionary<string, IAsset> LoadedAssets;

        /// <summary>
        /// The loaded asset manifest
        /// </summary>
        private AssetManifest Manifest { get; set; }

        /// <summary>
        /// Constructs an instance of the Asset Manager.
        /// </summary>
        public AssetManager()
        {
            LoadedAssets = new Dictionary<string, IAsset>();
        }

        /// <summary>
        /// Loads the asset manifest and prepares data structures for storing assets.
        /// </summary>
        public void Initialize()
        {
            // Load manifest from json file
        }

        /// <summary>
        /// Attempts to load an asset of the given name, if it is found in the manifest.
        /// </summary>
        /// <typeparam name="TAssetType"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public TAssetType LoadAsset<TAssetType>(string name)
            where TAssetType : IAsset, new()
        {
            // Check if the asset is already loaded
            if (LoadedAssets.TryGetValue(name, out var loaded))
            {
#if DEBUG
                // In debug builds, verify that the type of the loaded asset matches the requested asset type
                if (loaded is not TAssetType)
                {
                    throw new Exception($"Requested asset type {typeof(TAssetType).Name} does not match the loaded type of asset {name}: {loaded.GetType().Name}");
                }
#endif
                return (TAssetType)loaded;
            }

            if (Manifest == null) return default(TAssetType);

            // Find asset manifest data
            var assetManifestData = Manifest.Assets.FirstOrDefault(x => x.Name == name);
            if (assetManifestData == null)
            {
                throw new ArgumentException($"No asset found in the registry with name {name}");
            }

            // Open the file the asset is in
            using var stream = File.OpenRead(assetManifestData.Path);

            // Seek to the asset position in the stream
            stream.Seek(assetManifestData.OffsetBytes, SeekOrigin.Begin);
            using var reader = new BinaryReader(stream);

            // Create the asset instance
            var asset = new TAssetType();
            asset.LoadFromBinary(reader);

            // Store it in the registry
            LoadedAssets[name] = asset;

            return asset;
        }
    }
}
