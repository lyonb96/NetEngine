namespace NetEngine.Gameplay
{
    using System;

    /// <summary>
    /// Components are modular bits of logic that can attach to GameObjects in a hierarchical pattern.
    /// </summary>
    public class Component : IUniqueObject
    {
        /// <summary>
        /// This Component's unique ID.
        /// </summary>
        public Guid UniqueID { get; set; }

        /// <summary>
        /// The GameObject that owns this Component.
        /// </summary>
        public GameObject Owner { get; private set; }

        /// <summary>
        /// Constructs an instance of a Component with the given owning GameObject.
        /// </summary>
        /// <param name="owner"></param>
        public Component(GameObject owner)
        {
            Owner = owner;
        }
    }
}
