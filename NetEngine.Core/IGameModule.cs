namespace NetEngine.Core
{
    using Utilities;

    /// <summary>
    /// The interface responsible for structuring a NetEngine game implementation.
    /// </summary>
    public interface IGameModule
    {
        /// <summary>
        /// The name of the Game Module - this is what displays in the title bar.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The game instance's asset manager.
        /// </summary>
        AssetManager AssetManager { get; set; }

        /// <summary>
        /// This method gets called when the engine is done initializing, but before the loop starts.
        /// </summary>
        void OnGameStart();
    }
}
