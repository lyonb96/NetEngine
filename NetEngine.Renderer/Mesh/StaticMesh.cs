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

        /// <summary>
        /// The number of indices in the index buffer.
        /// </summary>
        internal int IndexCount { get; set; }

        /// <summary>
        /// The static mesh's VAO.
        /// </summary>
        internal int VertexArrayObject { get; private set; }

        public StaticMesh()
        { }

        /// <inheritdoc/>
        public void LoadFromBinary(BinaryReader stream, AssetManager assetManager)
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
            var rawVertexData = stream.ReadBytes(vertexSize * vertexCount);

            // Load indices
            IndexCount = stream.ReadInt32();
            var rawIndexData = stream.ReadBytes(IndexCount * sizeof(uint));

            GL.GenVertexArrays(1, out int vao);
            GL.GenBuffers(1, out int vbo);
            GL.GenBuffers(1, out int ebo);

            GL.BindVertexArray(vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(
                BufferTarget.ArrayBuffer,
                rawVertexData.Length,
                rawVertexData,
                BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            GL.BufferData(
                BufferTarget.ElementArrayBuffer,
                rawIndexData.Length,
                rawIndexData,
                BufferUsageHint.StaticDraw);

            // Vertex position
            var offset = 0;
            var stepSize = 3 * sizeof(float);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, vertexSize, offset);
            offset += stepSize;
            // Vertex normal
            stepSize = 4 * sizeof(float);
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, vertexSize, offset);
            offset += stepSize;
            // Vertex tex coords
            stepSize = 2 * sizeof(float);
            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, vertexSize, offset);

            GL.BindVertexArray(0);

            VertexBufferObject = vbo;
            IndexBufferObject = ebo;
            VertexArrayObject = vao;
        }
    }
}
