namespace NetEngine.Gameplay
{
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
        }
    }
}
