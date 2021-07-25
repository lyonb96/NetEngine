namespace NetEngine.Gameplay
{
    using System;
    using System.Collections.Generic;

    public sealed class World
    {
        /// <summary>
        /// A list of currently spawned objects.
        /// </summary>
        private readonly List<GameObject> Objects;

        /// <summary>
        /// Initializes the world instance.
        /// </summary>
        internal World()
        {
            Objects = new List<GameObject>();
            UniqueObjectRoot.SetWorld(this);
        }

        /// <summary>
        /// Spawns a game object of the given type, 
        /// </summary>
        /// <typeparam name="TGameObject">The type of GameObject to create.</typeparam>
        /// <returns>A new instance of the given GameObject type.</returns>
        public TGameObject SpawnGameObject<TGameObject>()
            where TGameObject : GameObject, new()
        {
            var newObj = new TGameObject();
            newObj.UniqueID = Guid.NewGuid();
            Objects.Add(newObj);
            return newObj;
        }

        /// <summary>
        /// Creates a component with the supplied factory method and assigns it to the given game object.
        /// </summary>
        /// <typeparam name="TComponent">The type of component to create.</typeparam>
        /// <param name="owner">The object that will own the component.</param>
        /// <param name="factory">The factory that creates an instance of the component.</param>
        /// <returns>A new instance of the given component type, assigned to the given object.</returns>
        public TComponent CreateComponent<TComponent>(GameObject owner, Func<TComponent> factory)
            where TComponent : Component
        {
            var comp = factory();
            comp.UniqueID = Guid.NewGuid();
            comp.Owner = owner;
            return comp;
        }

        /// <summary>
        /// Creates a component of the given type and assigns it to the given game object.
        /// </summary>
        /// <typeparam name="TComponent">The type of component to create.</typeparam>
        /// <param name="owner">The object that will own the component.</param>
        /// <returns>A new instance of the given component type, assigned to the given object.</returns>
        public TComponent CreateComponent<TComponent>(GameObject owner)
            where TComponent : Component, new()
        {
            var comp = new TComponent();
            comp.UniqueID = Guid.NewGuid();
            comp.Owner = owner;
            return comp;
        }

        /// <summary>
        /// Handles updating all of the game objects and their components each frame.
        /// </summary>
        public void OnUpdate()
        {
            foreach (var obj in Objects)
            {
                obj.Update();
                foreach (var comp in obj.Components)
                {
                    comp.Update();
                }
            }
        }

        /// <summary>
        /// Handles updating all of the game objects and their components on fixed ticks.
        /// </summary>
        public void OnFixedUpdate()
        {
            foreach (var obj in Objects)
            {
                obj.FixedUpdate();
                foreach (var comp in obj.Components)
                {
                    comp.FixedUpdate();
                }
            }
        }

        /// <summary>
        /// Creates a World instance and performs any necessary initialization.
        /// </summary>
        /// <returns>An initialized game world.</returns>
        public static World InitializeGameWorld()
        {
            var inst = new World();
            return inst;
        }
    }
}
