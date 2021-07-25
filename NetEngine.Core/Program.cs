namespace NetEngine.Core
{
    using System.Diagnostics;
    using Gameplay;
    using Utilities;

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
            Mesh = AddComponent<StaticMeshComponent>();
        }

        public override void Update()
        {
            base.Update();
            Debug.Print("Hello :)");
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
