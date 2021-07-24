namespace NetEngine.Gameplay
{
    using System.Collections.Generic;
    using OpenTK.Mathematics;

    /// <summary>
    /// A Scene Component is any component that participates in the visual structure of the scene.
    /// The base scene component acts as an empty node, for structuring the hierarchy.
    /// Sub-classes include light components, mesh components, and more.
    /// </summary>
    public class SceneComponent : Component
    {
        /// <summary>
        /// This SceneComponent's transform.
        /// </summary>
        public Transform Transform { get; set; }

        /// <summary>
        /// The parent of this Component - should only be null if this is the root component of the object.
        /// </summary>
        public SceneComponent Parent { get; protected set; }

        /// <summary>
        /// A list of the Components attached to this one.
        /// </summary>
        public List<SceneComponent> Children { get; set; }

        /// <inheritdoc/>
        public SceneComponent(GameObject owner) : base(owner)
        {
            Children = new List<SceneComponent>();
            Transform = new Transform();
        }

        /// <summary>
        /// Used to attach a Component to this one.
        /// </summary>
        /// <param name="child">The child to attach to this object.</param>
        public void AttachChild(SceneComponent child)
        {
            child.Parent = this;
            Children.Add(child);
        }

        /// <summary>
        /// Used to detach a Component from this one.
        /// </summary>
        /// <param name="child">The child Component to detach.</param>
        public void DetachChild(SceneComponent child)
        {
            if (child.Parent != this)
            {
                return;
            }
            child.Parent = null;
            Children.Remove(child);
        }

        /// <summary>
        /// Calculates this scene component's local transform.
        /// </summary>
        /// <returns>A 4x4 matrix describing this component's local translation, rotation, and scale.</returns>
        public Matrix4 GetLocalMatrix()
        {
            return Transform.ToMatrix4();
        }

        /// <summary>
        /// Calculates this scene component's world transform.
        /// </summary>
        /// <returns>A 4x4 matrix describing this component's world translation, rotation, and scale.</returns>
        public Matrix4 GetWorldMatrix()
        {
            return (Parent?.GetWorldMatrix() ?? Owner.Parent?.GetWorldMatrix() ?? Matrix4.Identity)
                * GetLocalMatrix();
        }
    }
}
