namespace NetEngine.Gameplay
{
    /// <summary>
    /// GameStates handle the high-level logic of a game, such as arena rules, spawning stuff, etc.
    /// </summary>
    public abstract class GameState
    {
        /// <summary>
        /// The static world instance for use across game states.
        /// </summary>
        private static World World;

        /// <summary>
        /// Called when a player connects, and handles spawning an appropriate player controller for them.
        /// </summary>
        /// <returns></returns>
        public abstract PlayerController GeneratePlayerController();

        /// <summary>
        /// Called once when the game state begins ticking.
        /// </summary>
        public abstract void OnStartGameState();

        /// <summary>
        /// Called once just before the game state is unloaded.
        /// </summary>
        public abstract void OnStopGameState();
        
        /// <summary>
        /// Called each frame to update game state logic.
        /// </summary>
        public abstract void Update();

        /// <summary>
        /// Called each fixed update tick for physics logic and whatnot.
        /// </summary>
        public abstract void FixedUpdate();

        /// <summary>
        /// Gets the world instance.
        /// </summary>
        /// <returns>The world instance.</returns>
        protected static World GetWorld() => World;

        /// <summary>
        /// Set on world initialization to populate the static world.
        /// </summary>
        /// <param name="world">The world instance.</param>
        internal static void SetWorld(World world)
        {
            World = world;
        }
    }
}
