namespace NetEngine.InputManager
{
    using System.Collections.Generic;
    using Input;

    /// <summary>
    /// Action bindings represent bindings whose inputs are binary, AKA true/false.
    /// Like keyboard keys, mouse buttons, controller buttons, etc.
    /// </summary>
    internal class ActionBinding
    {
        /// <summary>
        /// The name of the binding.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The binding's triggers.
        /// </summary>
        public List<Input> Triggers { get; set; }

        /// <summary>
        /// Constructs an action binding instance.
        /// </summary>
        /// <param name="name">The name of the binding.</param>
        public ActionBinding(string name)
        {
            Name = name;
            Triggers = new List<Input>();
        }
    }
}
