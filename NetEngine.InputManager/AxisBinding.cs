namespace NetEngine.InputManager
{
    using System.Collections.Generic;
    using Input;

    /// <summary>
    /// Axis triggers include a multiplier for scaling the input value.
    /// </summary>
    internal class AxisTrigger
    {
        /// <summary>
        /// The trigger for this... trigger.
        /// </summary>
        public Input Trigger { get; set; }

        /// <summary>
        /// The multiplier for this trigger.
        /// </summary>
        public float Multiplier { get; set; }
    }

    /// <summary>
    /// Axis bindings represent bindings whose state is non-binary, such as
    /// joysticks, mouse movement, etc.
    /// </summary>
    internal class AxisBinding
    {
        /// <summary>
        /// The name of the binding.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The binding's triggers.
        /// </summary>
        public List<AxisTrigger> Triggers { get; set; }

        /// <summary>
        /// Constructs an axis binding instance.
        /// </summary>
        /// <param name="name">The name of the binding.</param>
        public AxisBinding(string name)
        {
            Name = name;
            Triggers = new List<AxisTrigger>();
        }
    }
}
