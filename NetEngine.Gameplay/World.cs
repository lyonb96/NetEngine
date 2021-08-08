namespace NetEngine.Gameplay
{
    using System;
    using System.Collections.Generic;
    using InputManager;
    using RenderManager;
    using Utilities;

    /// <summary>
    /// The container for all currently spawned objects. Handles updates and creation/destruction.
    /// </summary>
    public sealed class World
    {
        /// <summary>
        /// A list of currently spawned objects.
        /// </summary>
        public List<GameObject> Objects { get; private set; }

        /// <summary>
        /// A list of active controllers.
        /// </summary>
        public List<Controller> Controllers { get; private set; }

        /// <summary>
        /// The scene graph's root node, for attaching objects.
        /// </summary>
        private readonly ISceneGraphNode RootNode;

        /// <summary>
        /// The current game state.
        /// </summary>
        private GameState CurrentGameState { get; set; }

        /// <summary>
        /// The engine's asset manager instance, for loading or resolving assets in components.
        /// </summary>
        private AssetManager AssetManager { get; set; }

        /// <summary>
        /// The engine's input manager, for listening to input events in controllers.
        /// </summary>
        private InputManager InputManager { get; set; }

        /// <summary>
        /// The local player controller reference.
        /// </summary>
        private PlayerController localPlayerController;

        /// <summary>
        /// Initializes the world instance.
        /// </summary>
        /// <param name="root">The scene graph's root node.</param>
        /// <param name="assetManager">The asset manager instance.</param>
        internal World(
            ISceneGraphNode root,
            AssetManager assetManager,
            InputManager inputManager)
        {
            Objects = new List<GameObject>();
            Controllers = new List<Controller>();
            RootNode = root;
            AssetManager = assetManager;
            InputManager = inputManager;
            UniqueObject.SetWorld(this);
            GameState.SetWorld(this);
        }

        /// <summary>
        /// Returns the asset manager instance.
        /// </summary>
        /// <returns>The asset manager instance.</returns>
        public AssetManager GetAssetManager() => AssetManager;

        /// <summary>
        /// Returns the input manager instance.
        /// </summary>
        /// <returns>The input manager instance.</returns>
        public InputManager GetInputManager() => InputManager;

        public PlayerController GetLocalPlayerController() => localPlayerController;

        /// <summary>
        /// Called by an object when its root component is changed.
        /// </summary>
        /// <param name="oldRoot">The old root component of the object.</param>
        /// <param name="newRoot">The new root component of the object.</param>
        internal void OnRootComponentChanged(SceneComponent oldRoot, SceneComponent newRoot)
        {
            if (oldRoot != null)
            {
                var parent = oldRoot.Parent;
                parent.DetachChild(oldRoot);
                parent.AttachChild(newRoot);
            }
            else
            {
                RootNode.DetachChild(oldRoot);
                RootNode.AttachChild(newRoot);
            }
        }

        #region Object Creation
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
        /// Creates a controller of the given type.
        /// </summary>
        /// <typeparam name="TController">The type of controller to create.</typeparam>
        /// <returns>A new instance of the given controller type.</returns>
        public TController CreateController<TController>()
            where TController : Controller, new()
        {
            // Generate the controller
            var controller = new TController();
            controller.UniqueID = Guid.NewGuid();
            Controllers.Add(controller);
            // Store a reference to the local player controller if none has been stored yet
            if (controller is PlayerController pc && localPlayerController == null)
            {
                localPlayerController = pc;
            }
            return controller;
        }

        public void OnObjectDestroyed(GameObject obj)
        {
            if (obj == null)
            {
                return;
            }
            if (obj.Parent != null)
            {
                // Detach from parent - TODO
            }
            if (obj.RootComponent != null)
            {
                var rootParent = obj.RootComponent.Parent;
                if (rootParent != null)
                {
                    rootParent.DetachChild(obj.RootComponent);
                }
            }
            Objects.Remove(obj);
        }
        #endregion

        #region State Management
        /// <summary>
        /// Sets a new active game state type. This method will clear all objects in the world 
        /// and call Stop on the current game state, if there is one.
        /// </summary>
        /// <typeparam name="TGameState">The type of game state to create and start.</typeparam>
        public void SetGameState<TGameState>()
            where TGameState : GameState, new()
        {
            SetGameState(new TGameState());
        }

        /// <summary>
        /// Constructs a new game state via the given factory, and sets it active. This method
        /// will clear all objects in the world and call Stop on the current game state, if
        /// there is one.
        /// </summary>
        /// <typeparam name="TGameState">The type of game state to create and start.</typeparam>
        /// <param name="factory">The factory function to create the game state.</param>
        public void SetGameState<TGameState>(Func<TGameState> factory)
            where TGameState : GameState
        {
            SetGameState(factory());
        }

        /// <summary>
        /// Sets the given game state instance to active, clears the world, and stops the current
        /// game state if there is one.
        /// </summary>
        /// <param name="state">The new state to activate.</param>
        private void SetGameState(GameState state)
        {
            if (CurrentGameState != null)
            {
                CurrentGameState.OnStopGameState();
            }
            ClearWorld();
            CurrentGameState = state;
            CurrentGameState.OnStartGameState();
            // Notify new game state of connected players
            // For now, since no networking has been built out, just hard-code to tell it there's one player
            CurrentGameState.OnPlayerConnected();
        }

        /// <summary>
        /// Removes all objects and controllers from the world.
        /// </summary>
        private void ClearWorld()
        {
            foreach (var controller in Controllers)
            {
                controller.UnpossessPawn();
            }
            Controllers.Clear();
            localPlayerController = null;
            foreach (var obj in Objects)
            {
                obj.Destroy();
            }
        }
        #endregion

        #region Updates
        /// <summary>
        /// Handles updating all of the game objects and their components each frame.
        /// </summary>
        public void OnUpdate()
        {
            CurrentGameState?.Update();
            foreach (var controller in Controllers)
            {
                controller.Update();
            }
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
            CurrentGameState?.FixedUpdate();
            foreach (var obj in Objects)
            {
                obj.FixedUpdate();
                foreach (var comp in obj.Components)
                {
                    comp.FixedUpdate();
                }
            }
        }
        #endregion

        /// <summary>
        /// Creates a World instance and performs any necessary initialization.
        /// </summary>
        /// <returns>An initialized game world.</returns>
        public static World InitializeGameWorld(
            ISceneGraphNode root,
            AssetManager assetManager,
            InputManager inputManager)
        {
            var inst = new World(root, assetManager, inputManager);
            return inst;
        }
    }
}
