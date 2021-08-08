namespace NetEngine.Sample
{
    using Gameplay;
    using OpenTK.Mathematics;
    using Utilities;

    public class BasicGameObject : GameObject
    {
        private StaticMeshComponent mesh;

        public BasicGameObject()
            : base()
        {
            mesh = AddComponent(() => new StaticMeshComponent("TestMesh"));
            SetRootComponent(mesh);
        }
    }
}
