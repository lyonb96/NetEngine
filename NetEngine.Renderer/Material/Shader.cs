namespace NetEngine.Renderer
{
    using System;
    using System.Collections.Generic;
    using OpenTK.Graphics.OpenGL4;

    /// <summary>
    /// Wraps initialization for vertex and fragment shaders.
    /// </summary>
    internal class Shader
    {
        /// <summary>
        /// OpenGL's shader handle.
        /// </summary>
        public int Handle { get; private set; }

        /// <summary>
        /// A mapping of uniform names to their locations on the shader.
        /// </summary>
        public Dictionary<string, int> UniformLocations { get; private set; }

        /// <summary>
        /// Constructs and initializes an instance of a Shader.
        /// </summary>
        /// <param name="vsSource">The vertex shader source code to use.</param>
        /// <param name="fsSource">The fragment shader source code to use.</param>
        public Shader(string vsSource, string fsSource)
        {
            // Generate and compile vertex shader
            var vs = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vs, vsSource);
            CompileShader(vs);

            // Generate and compile fragment shader
            var fs = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fs, fsSource);
            CompileShader(fs);

            // Create program handle and attach shaders
            Handle = GL.CreateProgram();
            GL.AttachShader(Handle, vs);
            GL.AttachShader(Handle, fs);

            // Link the program
            LinkProgram(Handle);

            // Clean up stale data
            GL.DetachShader(Handle, vs);
            GL.DetachShader(Handle, fs);
            GL.DeleteBuffer(vs);
            GL.DeleteBuffer(fs);

            // Get uniform locations
            GL.GetProgram(Handle, GetProgramParameterName.ActiveUniforms, out var numUniforms);
            UniformLocations = new Dictionary<string, int>();

            for (var i = 0; i < numUniforms; ++i)
            {
                var key = GL.GetActiveUniform(Handle, i, out _, out _);
                var location = GL.GetUniformLocation(Handle, key);
                UniformLocations[key] = location;
            }
        }

        /// <summary>
        /// Internal helper method to compile a GLSL shader and check for errors.
        /// </summary>
        /// <param name="shader">The shader to compile.</param>
        private void CompileShader(int shader)
        {
            GL.CompileShader(shader);

#if DEBUG
            GL.GetShader(shader, ShaderParameter.CompileStatus, out var code);
            if (code != (int)All.True)
            {
                var infoLog = GL.GetShaderInfoLog(shader);
                throw new Exception($"Shader compilation error: {infoLog}");
            }
#endif
        }

        /// <summary>
        /// Internal helper method to link an OpenGL shader and check for errors.
        /// </summary>
        /// <param name="program">The shader to link.</param>
        private void LinkProgram(int program)
        {
            GL.LinkProgram(program);

#if DEBUG
            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out var code);
            if (code != (int)All.True)
            {
                throw new Exception("Unable to link shaders.");
            }
#endif
        }
    }
}
