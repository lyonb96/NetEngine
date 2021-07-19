namespace NetEngine.Utilities
{
    /// <summary>
    /// An interface that describes a basic, loadable Asset.
    /// </summary>
    public interface IAsset
    {
        /// <summary>
        /// Populates data from a binary buffer.
        /// </summary>
        /// <param name="rawData">The raw binary data to load from.</param>
        void LoadFromBinary(byte[] rawData);
    }
}
