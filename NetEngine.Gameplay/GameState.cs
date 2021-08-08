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
        /// Called when a new player joins the game. This will be called once for all connected players
        /// when this game state is started.
        /// </summary>
        public void OnPlayerConnected()
        {
            // By default, generate a player controller for each one. Defer to the implementation on what
            // else to do for a player connection.
            var controller = GeneratePlayerController();
            OnPlayerConnected(controller);
        }

        /// <summary>
        /// Called when a new player joins the game and their controller has been generated. This will
        /// be called once for all connected players when this game state is started.
        /// </summary>
        /// <param name="controller">The new player's controller.</param>
        public virtual void OnPlayerConnected(PlayerController controller)
        {
        }

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
