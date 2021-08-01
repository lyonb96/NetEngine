namespace NetEngine.Core
{
    using Gameplay;

    class Program
    {
        static void Main(string[] args)
        {
            Engine.InitializeEngine<TestModule>();
        }
    }

    class TestGameObject : GameObject
    {
        private StaticMeshComponent Mesh;

        public TestGameObject()
            : base()
        {
            Mesh = AddComponent(() => new StaticMeshComponent("TestMesh"));
            SetRootComponent(Mesh);
        }
    }

    class TestModule : GameModule
    {
        private TestGameObject Test;

        public override string Name => "Test Game";

        public override void OnGameStart()
        {
            Test = World.SpawnGameObject<TestGameObject>();
        }

        public override void Update()
        {
        }

        public override void FixedUpdate()
        {
        }

        public override void OnGameShutdown()
        {
        }
    }
}
