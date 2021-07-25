namespace NetEngine.Utilities
{
    using System.IO;

    /// <summary>
    /// An interface that describes a basic, loadable Asset.
    /// </summary>
    public interface IAsset
    {
        /// <summary>
        /// Populates data from a binary buffer.
        /// </summary>
        /// <param name="stream">A binary stream from which to load the data.</param>
        /// <param name="assetManager">An instance of the asset manager to help with loading dependencies.</param>
        void LoadFromBinary(BinaryReader stream, AssetManager assetManager);
    }
}
