namespace NetEngine.Sample
{
    using Gameplay;
    using OpenTK.Mathematics;
    using Utilities;

    public class FlyingPawn : Pawn
    {
        private float moveSpeed;

        private Vector3 moveVector;

        public FlyingPawn()
            : base()
        {
            moveSpeed = 5.0F;
            moveVector = Vector3.Zero;
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
            moveVector.X = value;
        }

        private void MoveUp(float value)
        {
            moveVector.Y = value;
        }

        private void LookUp(float value)
        {
            AddLocalRotation(Quaternion.FromAxisAngle(GetRightAxis(), value));
        }

        private void LookRight(float value)
        {
            AddLocalRotation(Quaternion.FromAxisAngle(Vector3.UnitY, value));
        }

        public override void Update()
        {
            base.Update();
            var rotatedMoveValue = GetWorldRotation() * moveVector;
            AddLocalPosition(rotatedMoveValue.Normalized() * moveSpeed * Time.DeltaTime);
        }
    }
}
