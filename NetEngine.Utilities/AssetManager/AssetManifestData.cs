namespace NetEngine.Utilities
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines asset manifest data, which describes and locates an asset and its dependencies
    /// </summary>
    public class AssetManifestData
    {
        /// <summary>
        /// The name of the Asset.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The path to the Asset
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// The number of bytes to load from the file for this Asset.
        /// </summary>
        public int SizeBytes { get; set; }

        /// <summary>
        /// The number of bytes to skip in the file to read this asset.
        /// </summary>
        public int OffsetBytes { get; set; }

        /// <summary>
        /// A list of Asset names that this Asset depends on.
        /// </summary>
        public List<string> Dependencies { get; set; }
    }
}
