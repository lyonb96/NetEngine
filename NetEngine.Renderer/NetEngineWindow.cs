namespace NetEngine.Renderer
{
    using Core;
    using NetEngine.Input;
    using OpenTK.Mathematics;
    using OpenTK.Windowing.Common;
    using OpenTK.Windowing.Desktop;

    /// <summary>
    /// Wraps all window functionality
    /// </summary>
    public class NetEngineWindow : NativeWindow
    {
        /// <summary>
        /// A flag indicating if the window is trying to exit.
        /// </summary>
        public bool ShouldExit => !Exists || IsExiting;

        /// <summary>
        /// The Window's renderer.
        /// </summary>
        public Renderer WindowRenderer { get; private set; }

        /// <summary>
        /// The input handler for the Window.
        /// </summary>
        public InputHandler WindowInputHandler { get; private set; }

        /// <summary>
        /// Builds the game window with native window settings required by OpenTK.
        /// </summary>
        public NetEngineWindow()
            : base(GlobalConfig.Instance.BuildNativeWindowSettings())
        {}

        /// <summary>
        /// Initializes the window and builds up its Renderer.
        /// </summary>
        public void Initialize()
        {
            // Make context current for the resize call and render init
            Context.MakeCurrent();

            // Dispatch the resize event so listeners have the correct size
            OnResize(new ResizeEventArgs(Size));

            // Build the renderer
            WindowRenderer = new Renderer(Context);
            WindowRenderer.Initialize();

            WindowInputHandler = new InputHandler();
            unsafe
            {
                WindowInputHandler.ConfigureCallbacks(WindowPtr);
            }
        }
    }

    /// <summary>
    /// GlobalConfig extensions specific to rendering functionality.
    /// </summary>
    internal static class RendererConfigExtensions
    {
        /// <summary>
        /// Generates OpenTK's NativeWindowSettings based on stored config values.
        /// </summary>
        /// <param name="config">The GlobalConfig instance to use.</param>
        /// <returns>NativeWindowSettings for building up the game window.</returns>
        internal static NativeWindowSettings BuildNativeWindowSettings(this GlobalConfig config)
        {
            return new NativeWindowSettings
            {
                Size = new Vector2i(config.WindowWidth, config.WindowHeight),
                API = ContextAPI.OpenGL,
                Profile = ContextProfile.Core,
                WindowBorder = WindowBorder.Fixed,
                IsFullscreen = config.IsFullscreen,
                Flags = ContextFlags.Debug,
                AutoLoadBindings = true,
                Title = config.InstanceName,
            };
        }

        /// <summary>
        /// Gets the aspect ratio of the window based on the width and height set in config.
        /// </summary>
        /// <param name="config">The config to load from.</param>
        /// <returns>The aspect ratio of the window.</returns>
        internal static float GetWindowAspectRatio(this GlobalConfig config)
        {
            return (float)config.WindowWidth / (float)config.WindowHeight;
        }
    }
}
