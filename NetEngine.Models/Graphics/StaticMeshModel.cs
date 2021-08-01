namespace NetEngine.Models
{
    using Renderer;

    /// <summary>
    /// Contains all data for drawing a static mesh.
    /// </summary>
    public class StaticMeshModel : IDrawableObject
    {
        /// <summary>
        /// The mesh itself.
        /// </summary>
        public StaticMesh Mesh { get; set; }

        /// <summary>
        /// The material to draw the mesh with.
        /// </summary>
        public MaterialInstance Material { get; set; }

        // public CollisionShape Collision { get; set; }

        /// <summary>
        /// Constructs a static mesh model instance.
        /// </summary>
        public StaticMeshModel()
        { }
    }
}
