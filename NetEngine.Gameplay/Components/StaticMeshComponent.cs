namespace NetEngine.Gameplay
{
    using Models;
    using NetEngine.Renderer;
    using RenderManager;

    /// <summary>
    /// Static Mesh Components allow you to attach a non-animated mesh to a game object.
    /// </summary>
    public class StaticMeshComponent : SceneComponent, ISceneGraphDrawable
    {
        /// <summary>
        /// The static mesh model to draw.
        /// </summary>
        protected StaticMeshModel Model { get; set; }

        /// <summary>
        /// Constructs an instance of a static mesh component, and loads the model from the given name.
        /// </summary>
        /// <param name="modelName">The name of the model to load.</param>
        public StaticMeshComponent(string modelName)
            : base()
        {
            var vs = @"
#version 330 core
layout (location = 0) in vec3 aPos;
layout (location = 1) in vec4 aTangent;
layout (location = 2) in vec2 aTexCoord;

uniform mat4 WorldMatrix;
uniform mat4 ViewMatrix;
uniform mat4 ProjectionMatrix;

out vec3 Normal;

void main()
{
    Normal = (vec4(aTangent.xyz, 0.0) * WorldMatrix).xyz;
    gl_Position = vec4(aPos, 1.0) * WorldMatrix * ViewMatrix * ProjectionMatrix;
}";
            var fs = @"
#version 330 core

in vec3 Normal;

out  vec4 FragColor;

void main()
{
    float light = dot(normalize(Normal), normalize(vec3(1, 1, 1)));
    vec3 val = vec3(1.0) * light;
    FragColor = vec4(val, 1.0);
}";

            Model = new StaticMeshModel
            {
                Mesh = GetWorld().GetAssetManager().LoadAsset<StaticMesh>(modelName),
                Material = new MaterialInstance(new Material(vs, fs))
            };
        }

        /// <summary>
        /// Constructs an instance of a static mesh component using the specified model.
        /// </summary>
        /// <param name="model">The model to draw for this component.</param>
        public StaticMeshComponent(StaticMeshModel model)
            : base()
        {
            Model = model;
        }

        /// <inheritdoc/>
        public IDrawableObject GetDrawable()
        {
            return Model;
        }
    }
}
