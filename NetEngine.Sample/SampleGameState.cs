namespace NetEngine.Sample
{
    using Gameplay;
    using OpenTK.Mathematics;

    public class SampleGameState : GameState
    {
        public override void OnStartGameState()
        {
            GetWorld().SpawnGameObject<BasicGameObject>();
        }

        public override void Update()
        {
        }

        public override void FixedUpdate()
        {
        }

        public override void OnStopGameState()
        {
        }

        public override void OnPlayerConnected(PlayerController controller)
        {
            var pawn = GetWorld().SpawnGameObject<FlyingPawn>();
            pawn.AddLocalPosition(new Vector3(0.0F, 0.0F, -10.0F));
            controller.PossessPawn(pawn);
        }

        public override PlayerController GeneratePlayerController()
        {
            return GetWorld().CreateController<PlayerController>();
        }
    }
}
