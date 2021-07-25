namespace NetEngine.Core
{
    using NetEngine.Gameplay;
    using NetEngine.Utilities;

    /// <summary>
    /// Implements the game module interface.
    /// </summary>
    public abstract class GameModule : IGameModule
    {
        /// <inheritdoc/>
        public abstract string Name { get; }

        /// <inheritdoc/>
        public AssetManager AssetManager { get; set; }

        /// <inheritdoc/>
        public World World { get; set; }

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
    }
}
