namespace NetEngine.Core
{
    using Gameplay;
    using InputManager;
    using RenderManager;
    using Utilities;

    /// <summary>
    /// Serves as the base for a game implementation in NetEngine.
    /// </summary>
    public abstract class GameModule
    {
        /// <summary>
        /// The name of the game. Displayed in the title bar.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// The game instance's asset manager.
        /// </summary>
        protected AssetManager AssetManager { get; private set; }

        /// <summary>
        /// The game instance's input manager.
        /// </summary>
        protected InputManager InputManager { get; private set; }

        /// <summary>
        /// The game's world instance.
        /// </summary>
        protected World World { get; private set; }

        /// <summary>
        /// Initializes the game module with the given managers.
        /// </summary>
        /// <param name="root">The root of the scene.</param>
        /// <param name="assetManager">The asset manager instance.</param>
        /// <param name="inputManager">The input manager instance.</param>
        internal void InitializeModule(
            ISceneGraphNode root,
            AssetManager assetManager,
            InputManager inputManager)
        {
            World = World.InitializeGameWorld(
                root,
                assetManager,
                inputManager);
            AssetManager = assetManager;
            InputManager = inputManager;
        }

        /// <summary>
        /// Gets the world instance.
        /// </summary>
        /// <returns>The world instance.</returns>
        public World GetWorld() => World;

        /// <summary>
        /// Gets the asset manager.
        /// </summary>
        /// <returns>The asset manager.</returns>
        protected AssetManager GetAssetManager() => AssetManager;

        /// <summary>
        /// Gets the input manager.
        /// </summary>
        /// <returns>The input manager.</returns>
        protected InputManager GetInputManager() => InputManager;

        #region Logic Hooks
        /// <summary>
        /// Called just before the main loop has started, after engine startup is complete.
        /// </summary>
        public abstract void OnGameStart();

        /// <summary>
        /// Called once each fixed logic step.
        /// </summary>
        public abstract void FixedUpdate();

        /// <summary>
        /// Called once each rendered frame.
        /// </summary>
        public abstract void Update();

        /// <summary>
        /// Called after the main loop has terminated, before engine shutdown has started.
        /// </summary>
        public abstract void OnGameShutdown();

        /// <summary>
        /// Updates the game module and the world instance.
        /// </summary>
        internal void OnUpdate()
        {
            Update();
            World.OnUpdate();
        }

        /// <summary>
        /// Calls fixed update on the game module and world instance.
        /// </summary>
        internal void OnFixedUpdate()
        {
            FixedUpdate();
            World.OnFixedUpdate();
        }
        #endregion
    }
}
