namespace NetEngine.RenderManager
{
    using NetEngine.Models;
    using OpenTK.Mathematics;
    using Renderer;

    /// <summary>
    /// Handles converting higher-level scene graph structure and objects into lower-level graphics calls.
    /// </summary>
    public class RenderManager
    {
        /// <summary>
        /// The renderer implementation that handles lower level calls.
        /// </summary>
        private readonly Renderer Renderer;

        /// <summary>
        /// The root of the scene graph.
        /// </summary>
        private readonly ISceneGraphNode RootNode;

        /// <summary>
        /// Inititalizes the render manager.
        /// </summary>
        /// <param name="renderer">The rendering implementation.</param>
        public RenderManager(Renderer renderer)
        {
            Renderer = renderer;
            RootNode = new RootNode();
        }

        /// <summary>
        /// Gets the scene graph tree's root node.
        /// </summary>
        /// <returns>The scene graph tree's root node.</returns>
        public ISceneGraphNode GetRootNode()
        {
            return RootNode;
        }

        /// <summary>
        /// Draws a frame.
        /// </summary>
        /// <param name="cam"></param>
        public void DrawFrame(Camera cam)
        {
            Renderer.BeginFrame();
            Renderer.SetActiveCamera(cam);
            foreach (var node in RootNode.Children)
            {
                if (node is ISceneGraphDrawable drawable)
                {
                    // Drawable is the model
                    var obj = drawable.GetDrawable();
                    // World matrix is where to draw it
                    var worldMatrix = node.GetWorldMatrix();
                    // Draw the object
                    DrawObject(obj, worldMatrix);
                }
            }
            Renderer.EndFrame();
        }

        /// <summary>
        /// Draws a drawable object.
        /// </summary>
        /// <param name="obj">The object to draw.</param>
        /// <param name="translation">The translation at which to draw it.</param>
        private void DrawObject(IDrawableObject obj, Matrix4 translation)
        {
            if (obj == null) return;
            if (obj is StaticMeshModel staticMesh)
            {
                Renderer.DrawMeshAtMatrixWithMaterial(
                    staticMesh.Mesh,
                    staticMesh.Material,
                    translation);
            }
        }
    }
}
