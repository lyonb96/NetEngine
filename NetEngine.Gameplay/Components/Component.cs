namespace NetEngine.Gameplay
{
    /// <summary>
    /// Components are modular bits of logic that can attach to GameObjects in a hierarchical pattern.
    /// </summary>
    public class Component : UniqueObjectRoot
    {
        /// <summary>
        /// The GameObject that owns this Component.
        /// </summary>
        public GameObject Owner { get; internal set; }

        /// <summary>
        /// Constructs an instance of a Component.
        /// </summary>
        public Component()
        {
        }

        /// <summary>
        /// Called each frame for logic updates.
        /// </summary>
        public virtual void Update()
        {
        }

        /// <summary>
        /// Called each fixed step for logic updates.
        /// </summary>
        public virtual void FixedUpdate()
        {
        }
    }
}
