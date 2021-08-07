namespace NetEngine.Gameplay
{
    using System;
    using System.Collections.Generic;
    using Input;

    /// <summary>
    /// PlayerControllers represent the player's actions and inputs into the game world.
    /// They are used to forward actions from the player to their avatar in the game.
    /// </summary>
    public class PlayerController : Controller
    {
        /// <summary>
        /// Wraps the press and release action into one binding.
        /// </summary>
        private class ActionPressReleaseBinding
        {
            public Action PressAction { get; set; }
            public Action ReleaseAction { get; set; }
        }

        /// <summary>
        /// Action bindings set for the current Pawn.
        /// </summary>
        private readonly Dictionary<string, ActionPressReleaseBinding> ActionBindings;

        /// <summary>
        /// Axis bindings set for the current Pawn.
        /// </summary>
        private readonly Dictionary<string, Action<float>> AxisBindings;

        /// <summary>
        /// Constructs an instance of a player controller.
        /// </summary>
        public PlayerController()
        {
            ActionBindings = new Dictionary<string, ActionPressReleaseBinding>();
            AxisBindings = new Dictionary<string, Action<float>>();
        }

        /// <inheritdoc/>
        public override void Update()
        {
            base.Update();
            foreach (var binding in ActionBindings)
            {
                if (binding.Value.PressAction != null
                    && GetWorld().GetInputManager().IsJustPressed(binding.Key))
                {
                    binding.Value.PressAction();
                }
                else if (binding.Value.ReleaseAction != null
                    && GetWorld().GetInputManager().IsJustReleased(binding.Key))
                {
                    binding.Value.ReleaseAction();
                }
            }
            foreach (var binding in AxisBindings)
            {
                binding.Value(GetWorld().GetInputManager().GetAxisState(binding.Key));
            }
        }

        #region Pawn possession overrides
        /// <inheritdoc/>
        public override void PossessPawn(Pawn newPawn)
        {
            base.PossessPawn(newPawn);
            newPawn.SetupPlayerInput(this);
        }

        /// <inheritdoc/>
        public override void UnpossessPawn()
        {
            base.UnpossessPawn();
            ClearInputBindings();
        }
        #endregion

        #region Input binding
        /// <summary>
        /// Binds a method to the given action binding.
        /// </summary>
        /// <param name="name">The name of the binding to use.</param>
        /// <param name="action">The method to call for the action.</param>
        public void BindAction(string name, ActionMode mode, Action action)
        {
            if (ActionBindings.TryGetValue(name, out var binding))
            {
                switch (mode)
                {
                    case ActionMode.Pressed:
                        binding.PressAction = action;
                        break;
                    case ActionMode.Released:
                        binding.ReleaseAction = action;
                        break;
                }
            }
            else
            {
                ActionBindings[name] = new ActionPressReleaseBinding
                {
                    PressAction = mode == ActionMode.Pressed ? action : null,
                    ReleaseAction = mode == ActionMode.Released ? action : null,
                };
            }
        }

        /// <summary>
        /// Binds a method to the given axis binding.
        /// </summary>
        /// <param name="name">The name of the binding to use.</param>
        /// <param name="action">The method to call for the axis.</param>
        public void BindAxis(string name, Action<float> action)
        {
            AxisBindings[name] = action;
        }

        /// <summary>
        /// Clears all registered action and axis bindings
        /// </summary>
        public void ClearInputBindings()
        {
            ActionBindings.Clear();
            AxisBindings.Clear();
        }
        #endregion
    }
}
