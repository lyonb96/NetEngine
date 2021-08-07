namespace NetEngine.Core
{
    using Gameplay;
    using InputManager;
    using RenderManager;
    using Utilities;

    /// <summary>
    /// Implements the game module interface.
    /// </summary>
    public abstract class GameModule : IGameModule
    {
        /// <inheritdoc/>
        public abstract string Name { get; }

        /// <summary>
        /// The game instance's asset manager.
        /// </summary>
        protected AssetManager AssetManager { get; private set; }

        /// <summary>
        /// The game instance's input manager.
        /// </summary>
        protected InputManager InputManager { get; private set; }

        /// <inheritdoc/>
        protected World World { get; private set; }

        /// <inheritdoc cref="IGameModule.InitializeModule(ISceneGraphNode, AssetManager, InputManager)"/>
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

        /// <inheritdoc/>
        void IGameModule.InitializeModule(
            ISceneGraphNode root,
            AssetManager assetManager,
            InputManager inputManager)
        {
            InitializeModule(root, assetManager, inputManager);
        }

        /// <summary>
        /// Gets the world instance.
        /// </summary>
        /// <returns>The world instance.</returns>
        protected World GetWorld() => World;

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
        /// <inheritdoc/>
        public abstract void OnGameStart();

        /// <inheritdoc/>
        public abstract void FixedUpdate();

        /// <inheritdoc/>
        public abstract void Update();

        /// <inheritdoc/>
        public abstract void OnGameShutdown();

        /// <inheritdoc/>
        void IGameModule.OnUpdate()
        {
            Update();
            World.OnUpdate();
        }

        /// <inheritdoc/>
        void IGameModule.OnFixedUpdate()
        {
            FixedUpdate();
            World.OnFixedUpdate();
        }
        #endregion
    }
}
