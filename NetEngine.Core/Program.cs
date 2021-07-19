namespace NetEngine.Core
{
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
    }
}
