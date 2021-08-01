namespace NetEngine.Gameplay
{
    using System;

    /// <summary>
    /// Represents a basic uniquely identifiable object.
    /// </summary>
    public interface IUniqueObject
    {
        /// <summary>
        /// This object's unique identifier.
        /// </summary>
        Guid UniqueID { get; }
    }
}
