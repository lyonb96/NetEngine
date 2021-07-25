﻿namespace NetEngine.Renderer
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
        /// The VAO for static meshes
        /// </summary>
        internal static int StaticMeshVAO { get; private set; }

        /// <summary>
        /// Method used internally in the renderer to generate the VAO for static meshes
        /// </summary>
        internal static void GenerateStaticMeshVAO()
        {
            StaticMeshVAO = GL.GenVertexArray();
            GL.BindVertexArray(StaticMeshVAO);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), 3 * sizeof(float));
            GL.VertexAttribPointer(2, 4, VertexAttribPointerType.Float, false, 4 * sizeof(float), 5 * sizeof(float));
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);
            GL.EnableVertexAttribArray(2);
        }

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