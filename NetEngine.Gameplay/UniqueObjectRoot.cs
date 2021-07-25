namespace NetEngine.Gameplay
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class UniqueObjectRoot : IUniqueObject
    {
        /// <summary>
        /// This object's unique identifier.
        /// </summary>
        public Guid UniqueID { get; internal set; }

        /// <summary>
        /// An instance of the game world, used for spawning and despawning things.
        /// </summary>
        private static World World { get; set; }

        /// <summary>
        /// Gets an instance of the game world.
        /// </summary>
        /// <returns>An instance of the game world.</returns>
        protected World GetWorld()
        {
            return World;
        }

        /// <summary>
        /// Called by the world instance itself to set the world reference.
        /// </summary>
        /// <param name="world">The world reference to set.</param>
        internal static void SetWorld(World world)
        {
            World = world;
        }
    }
}
