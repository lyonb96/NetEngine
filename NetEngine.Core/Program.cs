namespace NetEngine.Core
{
    using Utilities;

    class Program
    {
        static void Main(string[] args)
        {
            Engine.InitializeEngine<TestModule>();
        }
    }

    class TestModule : IGameModule
    {
        public string Name { get; private set; } = "Test Module";

        public AssetManager AssetManager { get; set; }

        public void OnGameStart()
        {
        }
    }
}
