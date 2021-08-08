namespace NetEngine.Utilities
{
    using OpenTK.Mathematics;

    /// <summary>
    /// Specifies some helper methods for math operations.
    /// </summary>
    public static class MathHelpers
    {
        /// <summary>
        /// Caps the length of a vector to the given value, or returns the vector if it
        /// does not exceed the cap already.
        /// </summary>
        /// <param name="input">The vector to cap.</param>
        /// <param name="cap">The max length of the vector to return, default is 1.0.</param>
        /// <returns>A clamped copy of the vector.</returns>
        public static Vector3 CapLength(this Vector3 input, float cap = 1.0F)
        {
            return input.LengthSquared > (cap * cap)
                ? input.Normalized() * cap
                : input;
        }
    }
}
