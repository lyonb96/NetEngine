namespace NetEngine.Renderer
{
    using NetEngine.Core;
    using OpenTK.Mathematics;

    /// <summary>
    /// Describes the position and orientation of a camera in the scene.
    /// </summary>
    public class Camera
    {
        /// <summary>
        /// The position of the camera in world space.
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// The direction of the camera in world space.
        /// </summary>
        public Vector3 Direction { get; set; }

        /// <summary>
        /// The camera's up vector in world space.
        /// </summary>
        public Vector3 Up { get; set; }

        /// <summary>
        /// The field of view of the camera in degrees.
        /// </summary>
        public float FieldOfView { get; set; }

        /// <summary>
        /// Constructs an instance of a camera.
        /// </summary>
        public Camera()
        { }

        /// <summary>
        /// Gets the view matrix for this camera.
        /// </summary>
        /// <returns>The view matrix for this camera.</returns>
        internal Matrix4 GetViewMatrix()
        {
            var forward = Vector3.Normalize(Position);
            var right = Vector3.Normalize(Vector3.Cross(Up, forward));
            var up = Vector3.Cross(forward, right);
            return Matrix4.LookAt(Position, Position + Direction, up);
        }

        /// <summary>
        /// Gets the projection matrix for this camera.
        /// </summary>
        /// <returns>The projection matrix for this camera.</returns>
        internal Matrix4 GetProjectionMatrix()
        {
            return Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(FieldOfView), GlobalConfig.Instance.GetWindowAspectRatio(), 0.1F, 1000.0F);
        }

        /// <summary>
        /// Gets the view projection matrix for this camera.
        /// </summary>
        /// <returns>The view projection matrix for this camera.</returns>
        internal Matrix4 GetViewProjectionMatrix()
        {
            return GetViewMatrix() * GetProjectionMatrix();
        }

        /// <summary>
        /// Gets the inverse view matrix for this camera.
        /// </summary>
        /// <returns>The inverse view matrix for this camera.</returns>
        internal Matrix4 GetInverseViewMatrix()
        {
            return GetViewMatrix().Inverted();
        }

        /// <summary>
        /// Gets the inverse projection matrix for this camera.
        /// </summary>
        /// <returns>The inverse projection matrix for this camera.</returns>
        internal Matrix4 GetInverseProjectionMatrix()
        {
            return GetProjectionMatrix().Inverted();
        }

        /// <summary>
        /// Gets the inverse view projection matrix for this camera.
        /// </summary>
        /// <returns>The inverse view projection matrix for this camera.</returns>
        internal Matrix4 GetInverseViewProjectionMatrix()
        {
            return GetViewProjectionMatrix().Inverted();
        }
    }
}
