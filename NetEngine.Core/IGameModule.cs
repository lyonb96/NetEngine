namespace NetEngine.Core
{
    /// <summary>
    /// The interface responsible for structuring a NetEngine game implementation.
    /// </summary>
    public interface IGameModule
    {
        /// <summary>
        /// The name of the Game Module - this is what displays in the title bar.
        /// </summary>
        string Name { get; }
    }
}
