namespace NetEngine.Renderer
{
    using OpenTK.Graphics.OpenGL4;
    using OpenTK.Mathematics;
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
        /// The currently active camera.
        /// </summary>
        private Camera ActiveCam { get; set; }

        /// <summary>
        /// Constructs a renderer instance.
        /// </summary>
        /// <param name="context">The GLFW context.</param>
        public Renderer(IGLFWGraphicsContext context)
        {
            Context = context;
            Context.MakeCurrent();
        }

        /// <summary>
        /// Initializes the renderer.
        /// </summary>
        public void Initialize()
        {
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
        }

        /// <summary>
        /// Begins a frame.
        /// </summary>
        public void BeginFrame()
        {
            Context.MakeCurrent();
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        /// <summary>
        /// Ends a frame.
        /// </summary>
        public void EndFrame()
        {
            Context.SwapBuffers();
        }

        /// <summary>
        /// Sets the active camera instance to draw from.
        /// </summary>
        /// <param name="cam">The camera to draw from.</param>
        public void SetActiveCamera(Camera cam)
        {
            ActiveCam = cam;
        }

        /// <summary>
        /// Draws the given mesh with the given material at the given world matrix.
        /// </summary>
        /// <param name="mesh">The mesh to draw.</param>
        /// <param name="mat">The material to draw with.</param>
        /// <param name="matrix">The matrix to draw the mesh at.</param>
        public void DrawMeshAtMatrixWithMaterial(
            StaticMesh mesh,
            MaterialInstance mat,
            Matrix4 matrix)
        {
            GL.BindVertexArray(mesh.VertexArrayObject);
            mat.SetUniformMatrix4("WorldMatrix", matrix);
            mat.SetUniformMatrix4("ViewMatrix", ActiveCam.GetViewMatrix());
            mat.SetUniformMatrix4("ProjectionMatrix", ActiveCam.GetProjectionMatrix());
            mat.SetActive();
            GL.DrawElements(PrimitiveType.Triangles, mesh.IndexCount, DrawElementsType.UnsignedInt, 0);
            GL.BindVertexArray(0);
        }
    }
}
