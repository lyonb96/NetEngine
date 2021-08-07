namespace NetEngine.Gameplay
{
    using System.Linq;
    using OpenTK.Mathematics;
    using Renderer;

    /// <summary>
    /// Controllers are the go-between for player input, AI logic, etc. and the Pawns
    /// they control.
    /// </summary>
    public abstract class Controller : UniqueObject
    {
        /// <summary>
        /// The pawn this controller is currently possessing.
        /// </summary>
        protected Pawn CurrentPawn { get; set; }

        /// <summary>
        /// The currently active camera on the possessed pawn.
        /// </summary>
        protected CameraComponent CurrentCamera { get; set; }

        /// <summary>
        /// Called each frame to handle logical updates for the controller.
        /// </summary>
        public virtual void Update()
        {
        }

        /// <summary>
        /// Possesses a new pawn.
        /// </summary>
        /// <param name="newPawn">The new pawn to possess.</param>
        public virtual void PossessPawn(Pawn newPawn)
        {
            if (newPawn == null || newPawn == CurrentPawn) return;
            CurrentPawn = newPawn;
            newPawn.Possess(this);
        }

        /// <summary>
        /// Unpossesses the current pawn, if there is one.
        /// </summary>
        public virtual void UnpossessPawn()
        {
            if (CurrentPawn == null) return;

            CurrentPawn = null;
            CurrentCamera = null;
        }

        /// <summary>
        /// Gets the currently active camera for this controller.
        /// </summary>
        /// <returns>The active camera for this controller.</returns>
        public Camera GetActiveCamera()
        {
            if (CurrentPawn == null)
            {
                return new Camera
                {
                    Position = Vector3.Zero,
                    Direction = Vector3.UnitZ,
                    Up = Vector3.UnitY,
                };
            }
            if (CurrentCamera == null)
            {
                // Find a camera component on the pawn
                var camera = CurrentPawn
                    .GetComponents<CameraComponent>()
                    .FirstOrDefault();
                if (camera == null)
                {
                    // If the current pawn has no camera, return a camera centered at its origin
                    return new Camera
                    {
                        Position = CurrentPawn.GetWorldLocation(),
                        Direction = CurrentPawn.GetForwardAxis(),
                        Up = CurrentPawn.GetUpAxis(),
                    };
                }
                CurrentCamera = camera;
            }
            return new Camera
            {
                Position = CurrentCamera.GetWorldLocation(),
                Direction = CurrentCamera.GetForwardAxis(),
                Up = CurrentCamera.GetUpAxis(),
            };
        }
    }
}
