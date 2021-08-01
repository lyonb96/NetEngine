namespace NetEngine.RenderManager
{
    using System.Collections.Generic;
    using OpenTK.Mathematics;

    /// <summary>
    /// Acts as the root of the scene graph
    /// </summary>
    internal sealed class RootNode : ISceneGraphNode
    {
        /// <inheritdoc/>
        public Transform Transform { get; set; }

        /// <inheritdoc/>
        public List<ISceneGraphNode> Children { get; set; }

        /// <inheritdoc/>
        public ISceneGraphNode Parent { get; set; }

        public RootNode()
        {
            Children = new List<ISceneGraphNode>();
        }

        /// <inheritdoc/>
        public void AttachChild(ISceneGraphNode child)
        {
            Children.Add(child);
            child.Parent = this;
        }

        /// <inheritdoc/>
        public void DetachChild(ISceneGraphNode child)
        {
            if (Children.Remove(child))
            {
                child.Parent = null;
            }
        }

        /// <inheritdoc/>
        public Matrix4 GetLocalMatrix()
        {
            return Matrix4.Identity;
        }

        /// <inheritdoc/>
        public Matrix4 GetWorldMatrix()
        {
            return Matrix4.Identity;
        }
    }
}
