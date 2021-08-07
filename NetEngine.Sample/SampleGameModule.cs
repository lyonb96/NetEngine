namespace NetEngine.Sample
{
    using Core;
    using Input;

    public class SampleGameModule : GameModule
    {
        public override string Name => "NetEngine Sample Game";

        public override void OnGameStart()
        {
            // quick 'n dirty - set some default input bindings
            GetInputManager().SetAxisBindingTrigger("MoveForward", Input.KEY_W, 1.0F);
            GetInputManager().SetAxisBindingTrigger("MoveForward", Input.KEY_S, -1.0F);
            GetInputManager().SetAxisBindingTrigger("MoveRight", Input.KEY_D, 1.0F);
            GetInputManager().SetAxisBindingTrigger("MoveRight", Input.KEY_A, -1.0F);
            GetInputManager().SetAxisBindingTrigger("MoveUp", Input.KEY_Space, 1.0F);
            GetInputManager().SetAxisBindingTrigger("MoveUp", Input.KEY_LCtrl, -1.0F);
            GetInputManager().SetAxisBindingTrigger("LookUp", Input.MOUSE_Axis_Y);
            GetInputManager().SetAxisBindingTrigger("LookRight", Input.MOUSE_Axis_X);
            //GetWorld().SetGameState<SampleGameState>();
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
