﻿namespace NetEngine.Gameplay
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using OpenTK.Mathematics;

    /// <summary>
    /// GameObjects are the core of gameplay. They represent a spawnable object in the scene.
    /// </summary>
    public class GameObject : UniqueObject
    {
        /// <summary>
        /// The object's name (not guaranteed to be unique, or even set).
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The parent object - if this is not null, this object's transform is relative to the parent.
        /// </summary>
        public GameObject Parent { get; protected set; }

        /// <summary>
        /// A list of the Components attached to this GameObject.
        /// </summary>
        public List<Component> Components { get; protected set; }

        /// <summary>
        /// This GameObject's root component.
        /// </summary>
        public SceneComponent RootComponent { get; private set; }

        /// <summary>
        /// Constructs a GameObject instance.
        /// </summary>
        public GameObject()
        {
            Components = new List<Component>();
        }

        /// <summary>
        /// Marks this game object for destruction.
        /// </summary>
        public void Destroy()
        {
            GetWorld().OnObjectDestroyed(this);
        }

        #region Component Management
        /// <summary>
        /// Sets the root component of this game object.
        /// </summary>
        /// <param name="newRoot">The new component to set as the root.</param>
        protected void SetRootComponent(SceneComponent newRoot)
        {
            GetWorld().OnRootComponentChanged(RootComponent, newRoot);
            RootComponent = newRoot;
        }

        /// <summary>
        /// Adds a component to this GameObject and returns the new component instance.
        /// </summary>
        /// <typeparam name="TComponent">The type of component to create.</typeparam>
        /// <returns>The newly-created Component.</returns>
        public TComponent AddComponent<TComponent>()
            where TComponent : Component, new()
        {
            var comp = GetWorld().CreateComponent<TComponent>(this);
            Components.Add(comp);
            return comp;
        }

        /// <summary>
        /// Constructs a component with the given factory method and adds it to this GameObject.
        /// </summary>
        /// <typeparam name="TComponent">The type of component to create.</typeparam>
        /// <param name="factory">The factory method that creates the component.</param>
        /// <returns>The newly-created Component.</returns>
        public TComponent AddComponent<TComponent>(Func<TComponent> factory)
            where TComponent : Component
        {
            var comp = GetWorld().CreateComponent(this, factory);
            Components.Add(comp);
            return comp;
        }

        /// <summary>
        /// Gets this game object's components.
        /// </summary>
        /// <returns>A list of the components attached to this game object.</returns>
        public List<Component> GetComponents() => Components;

        /// <summary>
        /// Gets all of the components attached to this object that are of a given type.
        /// </summary>
        /// <typeparam name="TComponent">The type of component to look for.</typeparam>
        /// <returns>A list of all components attached to the object that match the
        /// given type.</returns>
        public List<TComponent> GetComponents<TComponent>()
            where TComponent : Component
        {
            return Components
                .Where(x => x is TComponent)
                .Select(x => x as TComponent)
                .ToList();
        }
        #endregion

        #region Transform Accessors
        /// <summary>
        /// Gets the world matrix of this Game Object based on its parent and root component.
        /// </summary>
        /// <returns>A Matrix4 that describes this object's position, rotation, and scale.</returns>
        public Matrix4 GetWorldMatrix()
        {
            return //(Parent?.GetWorldMatrix() ?? Matrix4.Identity) * (RootComponent?.Transform.ToMatrix4() ?? Matrix4.Identity);
                RootComponent?.GetWorldMatrix() ?? Matrix4.Identity;
        }

        /// <summary>
        /// Gets the world location of this Game Object based on its parent and root component.
        /// </summary>
        /// <returns>A Vector3 representing the world position of this object.</returns>
        public Vector3 GetWorldLocation()
        {
            return GetWorldMatrix().ExtractTranslation();
        }

        /// <summary>
        /// Gets the world rotation of this Game Object based on its parent and root component.
        /// </summary>
        /// <returns>A Quaternion representing the world rotation of this object.</returns>
        public Quaternion GetWorldRotation()
        {
            return GetWorldMatrix().ExtractRotation();
        }

        /// <summary>
        /// Gets the world scale of this Game Object based on its parent and root component.
        /// </summary>
        /// <returns>A Vector3 representing the world scale of this object.</returns>
        public Vector3 GetWorldScale()
        {
            return GetWorldMatrix().ExtractScale();
        }

        /// <summary>
        /// Gets the forward axis of this game object in world space.
        /// </summary>
        /// <returns>A Vector3 representing the forward axis of this game object in world space.</returns>
        public Vector3 GetForwardAxis()
        {
            if (RootComponent == null)
            {
                return Vector3.UnitZ;
            }
            return GetWorldRotation() * Vector3.UnitZ;
        }

        /// <summary>
        /// Gets the right axis of this game object in world space.
        /// </summary>
        /// <returns>A Vector3 representing the right axis of this game object in world space.</returns>
        public Vector3 GetRightAxis()
        {
            if (RootComponent == null)
            {
                return Vector3.UnitX;
            }
            return GetWorldRotation() * Vector3.UnitX;
        }

        /// <summary>
        /// Gets the up axis of this game object in world space.
        /// </summary>
        /// <returns>A Vector3 representing the up axis of this game object in world space.</returns>
        public Vector3 GetUpAxis()
        {
            if (RootComponent == null)
            {
                return Vector3.UnitY;
            }
            return GetWorldRotation() * Vector3.UnitY;
        }

        /// <summary>
        /// Adds an offset to the position of the game object in "local" space, AKA relative
        /// to its parent.
        /// </summary>
        /// <param name="offset">The offset to apply.</param>
        public void AddLocalPosition(Vector3 offset)
        {
            if (RootComponent == null)
            {
                return;
            }
            RootComponent.Transform.Position += offset;
        }

        /// <summary>
        /// Rotates the object by the given rotation in "local" space, AKA relative to its
        /// parent.
        /// </summary>
        /// <param name="offset">The offset to apply.</param>
        public void AddLocalRotation(Quaternion offset)
        {
            if (RootComponent == null)
            {
                return;
            }
            RootComponent.Transform.Rotation *= offset;
        }

        /// <summary>
        /// Rotates the object by the given rotation in "world" space, AKA relative to the
        /// world origin.
        /// </summary>
        /// <param name="offset">The offset to apply.</param>
        public void AddWorldRotation(Quaternion offset)
        {
            if (RootComponent == null)
            {
                return;
            }
            RootComponent.Transform.Rotation = offset * RootComponent.Transform.Rotation;
        }
        #endregion

        #region Gameplay Triggers
        /// <summary>
        /// Called once when this object is spawned, or when gameplay begins for pre-spawned objects.
        /// </summary>
        public virtual void OnBeginPlay()
        { }

        /// <summary>
        /// Called once for each rendered frame.
        /// </summary>
        public virtual void Update()
        { }

        /// <summary>
        /// Called once at each fixed update (by default, 60 times per second).
        /// </summary>
        public virtual void FixedUpdate()
        { }

        /// <summary>
        /// Called once when the object is despawned, or when the game is shutting down.
        /// </summary>
        public virtual void OnDestroy()
        { }
        #endregion
    }
}
