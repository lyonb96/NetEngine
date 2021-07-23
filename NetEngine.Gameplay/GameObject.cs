namespace NetEngine.Gameplay
{
    using System;
    using System.Collections.Generic;
    using OpenTK.Mathematics;

    /// <summary>
    /// GameObjects are the core of gameplay. They represent a spawnable object in the scene.
    /// </summary>
    public class GameObject : IUniqueObject
    {
        /// <summary>
        /// This GameObject's unique GUID.
        /// </summary>
        public Guid UniqueID { get; set; }

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
        public SceneComponent RootComponent { get; set; }

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
    }
}
