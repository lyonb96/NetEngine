namespace NetEngine.Gameplay
{
    /// <summary>
    /// Pawns represent a controllable object in the game scene. They are
    /// controlled by Controllers.
    /// </summary>
    public class Pawn : GameObject
    {
        /// <summary>
        /// The current controller of this pawn.
        /// </summary>
        protected Controller Controller { get; set; }

        /// <summary>
        /// Possesses the pawn with the given controller. Controller can be null
        /// to unpossess and leave it with no controller.
        /// </summary>
        /// <param name="newController">The new controller to possess this pawn.</param>
        internal void Possess(Controller newController)
        {
            if (newController == Controller) return;
            if (Controller != null)
            {
                OnUnpossess();
                Controller.UnpossessPawn();
            }
            Controller = newController;
            if (Controller != null)
            {
                OnPossess();
            }
        }

        /// <summary>
        /// Called when a new controller takes control of this pawn.
        /// The value of <see cref="Controller"/> will be set to the new controller.
        /// </summary>
        protected virtual void OnPossess()
        {
        }

        /// <summary>
        /// Called when the controller is unpossessing the current object.
        /// The value of <see cref="Controller"/> will still be the controller that is releasing control.
        /// </summary>
        protected virtual void OnUnpossess()
        {
        }

        /// <summary>
        /// Called by PlayerControllers when they possess this pawn. Used to configure
        /// input bindings for controlling this pawn.
        /// </summary>
        public virtual void SetupPlayerInput()
        {
        }
    }
}
