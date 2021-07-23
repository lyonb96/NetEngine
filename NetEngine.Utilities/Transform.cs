namespace NetEngine
{
    using OpenTK.Mathematics;

    /// <summary>
    /// Represents a translation, rotation, and scale in 3D space.
    /// </summary>
    public class Transform
    {
        /// <summary>
        /// The translation of this Transform.
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// The rotation of this transform.
        /// </summary>
        public Quaternion Rotation { get; set; }

        /// <summary>
        /// The scale of this transform.
        /// </summary>
        public Vector3 Scale { get; set; }

        /// <summary>
        /// Initializes a transform with default values.
        /// </summary>
        public Transform()
        {
            Position = Vector3.Zero;
            Rotation = Quaternion.Identity;
            Scale = Vector3.One;
        }

        /// <summary>
        /// Converts this transform into its equivalent 4x4 matrix.
        /// </summary>
        /// <returns>A 4x4 matrix equivalent to this transform.</returns>
        public Matrix4 ToMatrix4()
        {
            // TODO: This can almost certainly be optimized...
            return Matrix4.CreateTranslation(Position) * Matrix4.CreateFromQuaternion(Rotation) * Matrix4.CreateScale(Scale);
        }
    }
}
