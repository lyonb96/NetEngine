namespace NetEngine.Core
{
    using NetEngine.Gameplay;
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
        /// The game instance's world.
        /// </summary>
        World World { get; set; }

        /// <summary>
        /// Called once per frame to update all of the game logic.
        /// </summary>
        internal void OnUpdate();

        /// <summary>
        /// Called once per fixed update to update game logic.
        /// </summary>
        internal void OnFixedUpdate();

        /// <summary>
        /// This method gets called when the engine is done initializing, but before the loop starts.
        /// </summary>
        void OnGameStart();

        /// <summary>
        /// Called once per frame to update all of the game logic.
        /// </summary>
        void Update();

        /// <summary>
        /// Called once per fixed update to update game logic.
        /// </summary>
        void FixedUpdate();

        /// <summary>
        /// Called when the main loop has terminated and the engine is shutting down.
        /// </summary>
        void OnGameShutdown();
    }
}
