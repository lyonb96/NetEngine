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
        /// Called to set this camera as the current active camera for this Pawn.
        /// </summary>
        /// <param name="active">True to set this camera as active, false to disable it. True by default.</param>
        /// <remarks>Passing in true will find and disable all other cameras on the pawn.</remarks>
        public void SetActive(bool active = true)
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
    }
}
