namespace NetEngine.Renderer
{
    using OpenTK.Graphics.OpenGL4;
    using OpenTK.Windowing.Desktop;

    /// <summary>
    /// Wraps lower-level rendering functionality
    /// </summary>
    public class Renderer
    {
        /// <summary>
        /// The GLFW context that actually handles the OpenGL calls
        /// </summary>
        private readonly IGLFWGraphicsContext Context;

        /// <summary>
        /// Constructs a renderer instance.
        /// </summary>
        /// <param name="context">The GLFW context.</param>
        public Renderer(IGLFWGraphicsContext context)
        {
            Context = context;
        }

        public void Initialize()
        {
            StaticMesh.GenerateStaticMeshVAO();
        }

        /// <summary>
        /// Draws a full frame.
        /// </summary>
        public void Draw()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            Context.SwapBuffers();
        }
    }
}
