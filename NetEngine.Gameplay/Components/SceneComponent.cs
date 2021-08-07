namespace NetEngine.Gameplay
{
    using System.Collections.Generic;
    using NetEngine.RenderManager;
    using OpenTK.Mathematics;

    /// <summary>
    /// A Scene Component is any component that participates in the visual structure of the scene.
    /// The base scene component acts as an empty node, for structuring the hierarchy.
    /// Sub-classes include light components, mesh components, and more.
    /// </summary>
    public class SceneComponent : Component, ISceneGraphNode
    {
        /// <summary>
        /// This SceneComponent's transform.
        /// </summary>
        public Transform Transform { get; set; }

        /// <inheritdoc/>
        public List<ISceneGraphNode> Children { get; set; }

        /// <inheritdoc/>
        public ISceneGraphNode Parent { get; set; }

        /// <inheritdoc/>
        public SceneComponent()
            : base()
        {
            Children = new List<ISceneGraphNode>();
            Transform = new Transform();
        }

        /// <summary>
        /// Used to attach a Component to this one.
        /// </summary>
        /// <param name="child">The child to attach to this object.</param>
        public void AttachChild(ISceneGraphNode child)
        {
            child.Parent = this;
            Children.Add(child);
        }

        /// <summary>
        /// Used to detach a Component from this one.
        /// </summary>
        /// <param name="child">The child Component to detach.</param>
        public void DetachChild(ISceneGraphNode child)
        {
            if (child.Parent != this)
            {
                return;
            }
            child.Parent = null;
            Children.Remove(child);
        }

        /// <summary>
        /// Gets the local transform matrix for this scene component.
        /// </summary>
        /// <returns>The local transform matrix for this scene component.</returns>
        public Matrix4 GetLocalMatrix()
        {
            return Transform.ToMatrix4();
        }

        /// <summary>
        /// Gets the world transform matrix for this scene component.
        /// </summary>
        /// <returns>The world transform matrix for this scene component.</returns>
        public Matrix4 GetWorldMatrix()
        {
            return (Parent?.GetWorldMatrix() ?? Matrix4.Identity) * GetLocalMatrix();
        }

        /// <summary>
        /// Gets the world location of this scene component based on its parent and root component.
        /// </summary>
        /// <returns>A Vector3 representing the world position of this object.</returns>
        public Vector3 GetWorldLocation()
        {
            return GetWorldMatrix().ExtractTranslation();
        }

        /// <summary>
        /// Gets the world rotation of this scene component based on its parent and root component.
        /// </summary>
        /// <returns>A Quaternion representing the world rotation of this object.</returns>
        public Quaternion GetWorldRotation()
        {
            return GetWorldMatrix().ExtractRotation();
        }

        /// <summary>
        /// Gets the world scale of this scene component based on its parent and root component.
        /// </summary>
        /// <returns>A Vector3 representing the world scale of this object.</returns>
        public Vector3 GetWorldScale()
        {
            return GetWorldMatrix().ExtractScale();
        }

        /// <summary>
        /// Gets the forward axis of this scene component in world space.
        /// </summary>
        /// <returns>A Vector3 representing the forward axis of this scene component in world space.</returns>
        public Vector3 GetForwardAxis()
        {
            return GetWorldRotation() * Vector3.UnitZ;
        }

        /// <summary>
        /// Gets the right axis of this scene component in world space.
        /// </summary>
        /// <returns>A Vector3 representing the right axis of this scene component in world space.</returns>
        public Vector3 GetRightAxis()
        {
            return GetWorldRotation() * Vector3.UnitX;
        }

        /// <summary>
        /// Gets the up axis of this scene component in world space.
        /// </summary>
        /// <returns>A Vector3 representing the up axis of this scene component in world space.</returns>
        public Vector3 GetUpAxis()
        {
            return GetWorldRotation() * Vector3.UnitY;
        }
    }
}
