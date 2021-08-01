namespace NetEngine.RenderManager
{
    using System.Collections.Generic;
    using OpenTK.Mathematics;

    /// <summary>
    /// A node is an entry in the scenegraph, which can have children and supports arbitrary translation.
    /// </summary>
    public interface ISceneGraphNode
    {
        /// <summary>
        /// The transform of this Node.
        /// </summary>
        Transform Transform { get; set; }

        /// <summary>
        /// This node's children, whose transform is relative to this one.
        /// </summary>
        List<ISceneGraphNode> Children { get; set; }

        /// <summary>
        /// This Node's Parent. If null and not the root node, this node and its children will not be drawn.
        /// </summary>
        ISceneGraphNode Parent { get; set; }

        /// <summary>
        /// Attaches the given Node to this Node.
        /// Child node transforms are relative to their parents.
        /// </summary>
        /// <param name="child">The child to attach.</param>
        public void AttachChild(ISceneGraphNode child);

        /// <summary>
        /// Detaches the given item from this Node, if it is a child of the Node.
        /// </summary>
        /// <param name="child">The child to detach.</param>
        public void DetachChild(ISceneGraphNode child);

        /// <summary>
        /// Gets the local transform matrix of this Node.
        /// </summary>
        /// <returns>The local transform matrix of this Node.</returns>
        public Matrix4 GetLocalMatrix();

        /// <summary>
        /// Gets the world transform matrix of this Node.
        /// </summary>
        /// <returns>The world transform matrix of this Node.</returns>
        public Matrix4 GetWorldMatrix();
    }
}
