namespace NetEngine.Core
{
    using System;
    using System.Diagnostics;
    using NetEngine.Gameplay;
    using OpenTK.Mathematics;
    using Renderer;
    using RenderManager;
    using Utilities;

    /// <summary>
    /// The core of NetEngine, which handles init, game loop, and shutdown.
    /// </summary>
    public class Engine
    {
        /// <summary>
        /// The game module to run.
        /// </summary>
        private readonly IGameModule Game;

        /// <summary>
        /// The game window.
        /// </summary>
        private NetEngineWindow Window { get; set; }

        /// <summary>
        /// The window's renderer instance.
        /// </summary>
        private RenderManager RenderManager { get; set; }

        /// <summary>
        /// The game instance's asset manager
        /// </summary>
        private AssetManager AssetManager { get; set; }

        /// <summary>
        /// A Stopwatch for timing the update loop.
        /// </summary>
        private Stopwatch UpdateStopwatch { get; set; }

        /// <summary>
        /// A Stopwatch for timing the fixed update loop.
        /// </summary>
        private Stopwatch FixedUpdateStopwatch { get; set; }

        /// <summary>
        /// Temporary camera set here for testing.
        /// </summary>
        private Camera Cam { get; set; }

        /// <summary>
        /// Constructs the engine instance and stores the game module
        /// </summary>
        /// <param name="game">The game module to run.</param>
        private Engine(IGameModule game)
        {
            Game = game;
            UpdateStopwatch = new Stopwatch();
            FixedUpdateStopwatch = new Stopwatch();
        }

        /// <summary>
        /// Initializes the engine and its resources, runs the loop, and shuts down after.
        /// </summary>
        private void Initialize()
        {
            if (!Stopwatch.IsHighResolution)
            {
                throw new Exception("High-resolution stopwatch is required to run NetEngine.");
            }

            // Load and initialize global config options
            GlobalConfig.Initialize();
            GlobalConfig.Instance.InstanceName = Game.Name;
            Time.FixedDeltaTime = 1.0F / 60.0F;

            // Create the game window
            Window = new NetEngineWindow();
            Window.Initialize();
            RenderManager = new RenderManager(Window.WindowRenderer);

            // Start up the asset manager
            AssetManager = new AssetManager();
            AssetManager.Initialize();
            Game.AssetManager = AssetManager;

            // Create the game world
            Game.World = World.InitializeGameWorld(
                RenderManager.GetRootNode(),
                AssetManager);

            Cam = new Camera
            {
                Position = new Vector3(4.0F, 4.0F, 4.0F),
                Direction = Vector3.UnitZ,
                Up = Vector3.UnitY,
                FieldOfView = 70.0F
            };

            // Call game start
            Game.OnGameStart();

            // Begin running the main loop
            RunGameLoop();
        }

        /// <summary>
        /// Handles running the game loop and dispatching update and render frames.
        /// </summary>
        private void RunGameLoop()
        {
            UpdateStopwatch.Start();
            FixedUpdateStopwatch.Start();
            while (true)
            {
                Window.ProcessEvents();
                HandleLogicUpdates();

                if (Window.ShouldExit)
                {
                    return;
                }

                HandleRenderUpdate();
            }
        }
        
        /// <summary>
        /// Handles dispatching fixed and fast logic updates.
        /// </summary>
        private void HandleLogicUpdates()
        {
            // Dispatach the constant logic updates
            Time.DeltaTime = (float)UpdateStopwatch.Elapsed.TotalSeconds;
            Time.Runtime += Time.DeltaTime;
            Time.DeltaTimeTicks = UpdateStopwatch.Elapsed.Ticks;
            UpdateStopwatch.Restart();
            Update();

            // Determine how many fixed logic updates to run
            var fixedElapsed = FixedUpdateStopwatch.Elapsed.TotalSeconds;
            var fixedTime = 1.0F / GlobalConfig.Instance.FixedLoopSpeed;
            if (fixedElapsed >= fixedTime)
            {
                FixedUpdateStopwatch.Restart();
                var fixedUpdatesThisRun = 0;
                while (fixedElapsed >= fixedTime && fixedUpdatesThisRun++ < 3)
                {
                    fixedElapsed -= fixedTime;
                    FixedUpdate();
                }
            }
        }

        /// <summary>
        /// Called once before every frame is rendered.
        /// </summary>
        /// <param name="Delta"></param>
        private void Update()
        {
            Game.OnUpdate();
        }

        /// <summary>
        /// Called at a fixed frequency defined in the global config.
        /// </summary>
        private void FixedUpdate()
        {
            Game.OnFixedUpdate();
        }

        /// <summary>
        /// Dispatches render commands.
        /// </summary>
        private void HandleRenderUpdate()
        {
            // Determine current active camera
            // For now this is just hard coded
            var activeCam = Cam;
            RenderManager.DrawFrame(activeCam);
        }

        /// <summary>
        /// Initializes the engine with the given game module type.
        /// </summary>
        /// <typeparam name="TGameModule">The game module to run.</typeparam>
        public static void InitializeEngine<TGameModule>()
            where TGameModule : IGameModule, new()
        {
            var instance = new Engine(new TGameModule());
            instance.Initialize();
        }
    }
}
