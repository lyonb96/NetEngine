namespace NetEngine.Gameplay
{
    using System;

    public interface IUniqueObject
    {
        /// <summary>
        /// This object's unique identifier.
        /// </summary>
        Guid UniqueID { get; }
    }
}
