namespace NetEngine.Renderer
{
    using System.IO;
    using OpenTK.Graphics.OpenGL4;
    using Utilities;

    /// <summary>
    /// Represents data about a single, non-animated mesh.
    /// </summary>
    public class StaticMesh : IAsset
    {
        /// <summary>
        /// The vertex buffer object for OpenGL rendering.
        /// </summary>
        internal int VertexBufferObject { get; set; }

        /// <summary>
        /// The index buffer object for OpenGL rendering.
        /// </summary>
        internal int IndexBufferObject { get; set; }

        public StaticMesh()
        { }

        /// <inheritdoc/>
        public void LoadFromBinary(BinaryReader stream)
        {
            var versionFlag = stream.ReadByte();
            switch (versionFlag)
            {
                case 1:
                    LoadStaticMeshV1(stream);
                    break;
                default:
                    return;
            }
        }
        
        /// <summary>
        /// Loads "version 1" static meshes.
        /// </summary>
        /// <param name="stream">The stream to load the data from.</param>
        private void LoadStaticMeshV1(BinaryReader stream)
        {
            const int vertexSize
                = (4 * 3) // 3 floats for position
                + (4 * 4) // 4 floats for tangent
                + (4 * 2); // 2 floats for tex coords

            // Load vertices
            var vertexCount = stream.ReadInt32();
            var vertexData = stream.ReadBytes(vertexSize * vertexCount);
            // Load index
            var indexCount = stream.ReadInt32();
            var indexData = stream.ReadBytes(indexCount * sizeof(uint));

            // Generate buffer objects
            VertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(
                BufferTarget.ArrayBuffer,
                vertexData.Length,
                vertexData,
                BufferUsageHint.StaticDraw);

            IndexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, IndexBufferObject);
            GL.BufferData(
                BufferTarget.ArrayBuffer,
                indexData.Length,
                indexData,
                BufferUsageHint.StaticDraw);
        }
    }
}
