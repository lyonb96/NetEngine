namespace NetEngine.Sample
{
    using System.Diagnostics;
    using Gameplay;
    using OpenTK.Mathematics;
    using Utilities;

    public class FlyingPawn : Pawn
    {
        private CameraComponent Camera { get; set; }

        private float moveSpeed;

        private Vector3 moveVector;

        public FlyingPawn()
            : base()
        {
            moveSpeed = 5.0F;
            moveVector = Vector3.Zero;
            Camera = AddComponent<CameraComponent>();
            SetRootComponent(Camera);
        }

        public override void SetupPlayerInput(PlayerController playerController)
        {
            base.SetupPlayerInput(playerController);
            playerController.BindAxis("MoveForward", MoveForward);
            playerController.BindAxis("MoveRight", MoveRight);
            playerController.BindAxis("MoveUp", MoveUp);
            playerController.BindAxis("LookUp", LookUp);
            playerController.BindAxis("LookRight", LookRight);
        }

        private void MoveForward(float value)
        {
            moveVector.Z = value;
        }

        private void MoveRight(float value)
        {
            moveVector.X = -value;
        }

        private void MoveUp(float value)
        {
            moveVector.Y = value;
        }

        private void LookUp(float value)
        {
            AddLocalRotation(Quaternion.FromAxisAngle(Vector3.UnitX, value * 0.01F));
        }

        private void LookRight(float value)
        {
            AddWorldRotation(Quaternion.FromAxisAngle(Vector3.UnitY, -value * 0.01F));
        }

        public override void Update()
        {
            base.Update();
            var rotatedMoveValue = GetWorldRotation() * moveVector;
            AddLocalPosition(rotatedMoveValue.CapLength() * moveSpeed * Time.DeltaTime);
        }
    }
}
