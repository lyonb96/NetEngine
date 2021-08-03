namespace NetEngine.InputManager
{
    using System.Collections.Generic;

    /// <summary>
    /// Convenience wrapper for saving and loading bindings from config.
    /// </summary>
    internal class SaveLoadBindingWrapper
    {
        /// <summary>
        /// Stored action bindings.
        /// </summary>
        public List<ActionBinding> ActionBindings { get; set; }

        /// <summary>
        /// Stored axis bindings.
        /// </summary>
        public List<AxisBinding> AxisBindings { get; set; }
    }
}
