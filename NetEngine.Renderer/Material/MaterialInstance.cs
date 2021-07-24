namespace NetEngine.Renderer
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using OpenTK.Mathematics;
    using Utilities;

    /// <summary>
    /// A material instance provides an interface to set values on materials to vary the
    /// behavior of a base material across different instances.
    /// </summary>
    public class MaterialInstance : IAsset
    {
        /// <summary>
        /// The material this instance is derived from.
        /// </summary>
        public Material BaseMaterial { get; protected set; }

        /// <summary>
        /// A map of applied uniform values for this material instance.
        /// </summary>
        private Dictionary<string, Action> Uniforms { get; set; }

        /// <summary>
        /// Constructs a material instance.
        /// </summary>
        public MaterialInstance()
        {
            Uniforms = new Dictionary<string, Action>();
        }

        /// <summary>
        /// Sets this material instance as the active one on the pipeline.
        /// </summary>
        internal void SetActive()
        {
            BaseMaterial.SetActive();
            foreach (var uniformApply in Uniforms.Values)
            {
                uniformApply();
            }
        }

        #region Uniforms
        /// <summary>
        /// Sets a new float value for the given uniform.
        /// </summary>
        /// <param name="name">The name of the uniform to set.</param>
        /// <param name="value">The new value to assign to the uniform.</param>
        public void SetUniformFloat(string name, float value)
        {
            Uniforms[name] = () =>
            {
                BaseMaterial.SetUniformFloat_Implementation(name, value);
            };
        }

        /// <summary>
        /// Sets a new Vector2 value for the given uniform.
        /// </summary>
        /// <param name="name">The name of the uniform to set.</param>
        /// <param name="value">The new value to assign to the uniform.</param>
        public void SetUniformFloat2(string name, Vector2 value)
        {
            Uniforms[name] = () =>
            {
                BaseMaterial.SetUniformFloat2_Implementation(name, value);
            };
        }

        /// <summary>
        /// Sets a new Vector3 value for the given uniform.
        /// </summary>
        /// <param name="name">The name of the uniform to set.</param>
        /// <param name="value">The new value to assign to the uniform.</param>
        public void SetUniformFloat3(string name, Vector3 value)
        {
            Uniforms[name] = () =>
            {
                BaseMaterial.SetUniformFloat3_Implementation(name, value);
            };
        }

        /// <summary>
        /// Sets a new Vector4 value for the given uniform.
        /// </summary>
        /// <param name="name">The name of the uniform to set.</param>
        /// <param name="value">The new value to assign to the uniform.</param>
        public void SetUniformFloat4(string name, Vector4 value)
        {
            Uniforms[name] = () =>
            {
                BaseMaterial.SetUniformFloat4_Implementation(name, value);
            };
        }
        #endregion

        /// <inheritdoc/>
        public void LoadFromBinary(BinaryReader stream, AssetManager assetManager)
        {
            // Get the name of the material
            var mat = stream.ReadString();
            BaseMaterial = assetManager.ResolveMaterial(mat);
        }
    }
}
