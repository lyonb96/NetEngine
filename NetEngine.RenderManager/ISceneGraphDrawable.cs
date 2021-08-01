namespace NetEngine.RenderManager
{
    using Models;

    /// <summary>
    /// Represents a node of the scene graph that can be drawn.
    /// </summary>
    public interface ISceneGraphDrawable
    {
        /// <summary>
        /// Gets the drawable object for this scene graph node.
        /// </summary>
        /// <returns>The drawable object for this scene graph node.</returns>
        IDrawableObject GetDrawable();
    }
}
