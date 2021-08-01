namespace NetEngine.Utilities
{
    /// <summary>
    /// Stores time values for public access.
    /// </summary>
    public static class Time
    {
        /// <summary>
        /// The amount of time in seconds since the last frame.
        /// </summary>
        public static float DeltaTime { get; set; }

        /// <summary>
        /// The amount of time in "ticks" since the last frame.
        /// </summary>
        public static long DeltaTimeTicks { get; set; }

        /// <summary>
        /// The amount of time in seconds that pass between each fixed frame.
        /// </summary>
        public static float FixedDeltaTime { get; set; }

        /// <summary>
        /// The amount of time in seconds that the game has been running.
        /// </summary>
        public static float Runtime { get; set; }
    }
}
