namespace NetEngine.Gameplay
{
    using Renderer;

    /// <summary>
    /// Camera Components allow us to attach a camera to pawns, and use them
    /// as the primary viewpoint of the game as desired.
    /// </summary>
    public class CameraComponent : SceneComponent
    {
        /// <summary>
        /// True if the camera is currently active, false if not.
        /// </summary>
        public bool IsActive { get; private set; }

        /// <summary>
        /// Gets or sets the field of view for this camera in degrees.
        /// </summary>
        public float FieldOfView { get; set; }

        /// <summary>
        /// The camera instance this component represents.
        /// </summary>
        protected Camera CameraInst;

        /// <summary>
        /// Constructs an instance of a Camera Component.
        /// </summary>
        public CameraComponent()
        {
            IsActive = false;
            FieldOfView = 70.0F;
        }

        /// <summary>
        /// Called to set this camera as the current active camera for this Pawn.
        /// </summary>
        public void SetActive()
        {
            if (Owner == null || Owner is not Pawn pawn)
            {
                return;
            }
            var cameras = Owner.GetComponents<CameraComponent>();
            foreach (var cam in cameras)
            {
                cam.IsActive = false;
            }
            IsActive = true;
            pawn.GetController()?.SetActiveCamera(this);
        }
        
        /// <summary>
        /// Updates the stored camera instance and returns it.
        /// </summary>
        /// <returns>The camera instance, updated to this component's current position and rotation.</returns>
        public Camera GetCamera()
        {
            CameraInst ??= new Camera();
            CameraInst.Position = GetWorldLocation();
            CameraInst.Direction = GetForwardAxis().Normalized();
            CameraInst.Up = GetUpAxis().Normalized();
            CameraInst.FieldOfView = FieldOfView;
            return CameraInst;
        }
    }
}
