﻿namespace NetEngine.Renderer
{
    using System.IO;
    using OpenTK.Graphics.OpenGL4;
    using OpenTK.Mathematics;
    using Utilities;

    /// <summary>
    /// A Material describes how a surface should be drawn.
    /// </summary>
    public class Material : IAsset
    {
        /// <summary>
        /// This material's Shader instance.
        /// </summary>
        internal Shader Shader { get; set; }

        /// <summary>
        /// Constructs an instance of a material.
        /// </summary>
        public Material()
        {}

        /// <summary>
        /// Constructs an instance of a material from the given source code.
        /// </summary>
        /// <param name="vs">Vertex shader code.</param>
        /// <param name="fs">Fragment shader code.</param>
        public Material(string vs, string fs)
        {
            InitializeFromSource(vs, fs);
        }

        /// <summary>
        /// Sets this material's shader as the active shader for OpenGL.
        /// </summary>
        public void SetActive()
        {
            GL.UseProgram(Shader.Handle);
        }

        #region Uniforms
        /// <summary>
        /// Sets a new float value for the given uniform.
        /// </summary>
        /// <param name="name">The name of the uniform to set.</param>
        /// <param name="value">The new value to assign to the uniform.</param>
        internal void SetUniformFloat_Implementation(string name, float value)
        {
            GL.UseProgram(Shader.Handle);
            GL.Uniform1(Shader.UniformLocations[name], value);
        }

        /// <summary>
        /// Sets a new Vector2 value for the given uniform.
        /// </summary>
        /// <param name="name">The name of the uniform to set.</param>
        /// <param name="value">The new value to assign to the uniform.</param>
        internal void SetUniformFloat2_Implementation(string name, Vector2 value)
        {
            GL.UseProgram(Shader.Handle);
            GL.Uniform2(Shader.UniformLocations[name], value);
        }

        /// <summary>
        /// Sets a new Vector3 value for the given uniform.
        /// </summary>
        /// <param name="name">The name of the uniform to set.</param>
        /// <param name="value">The new value to assign to the uniform.</param>
        internal void SetUniformFloat3_Implementation(string name, Vector3 value)
        {
            GL.UseProgram(Shader.Handle);
            GL.Uniform3(Shader.UniformLocations[name], value);
        }

        /// <summary>
        /// Sets a new Vector4 value for the given uniform.
        /// </summary>
        /// <param name="name">The name of the uniform to set.</param>
        /// <param name="value">The new value to assign to the uniform.</param>
        internal void SetUniformFloat4_Implementation(string name, Vector4 value)
        {
            GL.UseProgram(Shader.Handle);
            GL.Uniform4(Shader.UniformLocations[name], value);
        }

        /// <summary>
        /// Sets a new Matrix4 value for the given uniform.
        /// </summary>
        /// <remarks>Transposes the matrix before setting it.</remarks>
        /// <param name="name">The name of the uniform to set.</param>
        /// <param name="value">The new value to assign to the uniform.</param>
        internal void SetUniformMatrix4_Implementation(string name, Matrix4 value)
        {
            GL.UseProgram(Shader.Handle);
            GL.UniformMatrix4(Shader.UniformLocations[name], true, ref value);
        }
        #endregion

        /// <inheritdoc/>
        public void LoadFromBinary(BinaryReader stream, AssetManager assetManager)
        {
            // Get vertex and fragment shader source from the stream
            var vs = stream.ReadString();
            var fs = stream.ReadString();

            InitializeFromSource(vs, fs);
        }

        /// <summary>
        /// Initializes the Shader instance from the given source code.
        /// </summary>
        /// <param name="vs">The vertex shader source code.</param>
        /// <param name="fs">The fragment shader source code.</param>
        private void InitializeFromSource(string vs, string fs)
        {
            // Generate the shader
            Shader = new Shader(vs, fs);
        }
    }
}
