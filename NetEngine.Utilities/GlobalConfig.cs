namespace NetEngine.Core
{
    using System.Collections.Generic;

    /// <summary>
    /// Holds global configuration data, some of which is loaded from config files.
    /// </summary>
    public class GlobalConfig
    {
        /// <summary>
        /// The global config instance.
        /// </summary>
        public static GlobalConfig Instance { get; private set; }

        #region Basic Options
        /// <summary>
        /// The name of the instance - also displayed in the title bar.
        /// </summary>
        public string InstanceName { get; set; }

        /// <summary>
        /// The number of updates per second the fixed loop should use.
        /// </summary>
        public float FixedLoopSpeed { get; set; } = 60.0F;

        /// <summary>
        /// A dictionary of custom configuration options.
        /// </summary>
        public Dictionary<string, object> CustomOptions { get; set; }
        #endregion

        #region Render Settings
        /// <summary>
        /// The width of the game window.
        /// </summary>
        public int WindowWidth { get; set; } = 1280;

        /// <summary>
        /// The height of the game window.
        /// </summary>
        public int WindowHeight { get; set; } = 720;

        /// <summary>
        /// A flag for if the game window should be fullscreen.
        /// </summary>
        public bool IsFullscreen { get; set; } = false;
        #endregion

        /// <summary>
        /// Loads config from the config files.
        /// </summary>
        public static void Initialize()
        {
            Instance = new GlobalConfig();
        }
    }
}
