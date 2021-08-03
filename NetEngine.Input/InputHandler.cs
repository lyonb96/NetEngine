namespace NetEngine.Input
{
    using System.Collections.Generic;
    using OpenTK.Mathematics;
    using OpenTK.Windowing.GraphicsLibraryFramework;

    /// <summary>
    /// Custom input handler, for decoupling input from rendering and to assist in implementing bindings.
    /// </summary>
    public class InputHandler
    {
        /// <summary>
        /// The current state of all axis inputs.
        /// </summary>
        private Dictionary<Input, float> AxisStates;

        /// <summary>
        /// The state of all axis inputs at the end of last frame.
        /// </summary>
        private Dictionary<Input, float> LastAxisStates;

        /// <summary>
        /// The current state of all action inputs.
        /// </summary>
        private Dictionary<Input, bool> ActionStates;

        /// <summary>
        /// The state of all action inputs at the end of last frame.
        /// </summary>
        private Dictionary<Input, bool> LastActionStates;

        /// <summary>
        /// The current mouse position.
        /// </summary>
        private Vector2 MousePosition;

        /// <summary>
        /// Constructs an instance of the input handler.
        /// </summary>
        public InputHandler()
        {
            AxisStates = new Dictionary<Input, float>();
            LastAxisStates = new Dictionary<Input, float>();
            ActionStates = new Dictionary<Input, bool>();
            LastActionStates = new Dictionary<Input, bool>();
            MousePosition = new Vector2();
        }

        /// <summary>
        /// Called once a frame is complete to swap state buffers.
        /// </summary>
        public void PostFrame()
        {
            // Roll action states to last known set but keep the current state the same
            // since a "press" or "repeat" event is not guaranteed to occur every frame
            // for keys that are held
            LastActionStates = new Dictionary<Input, bool>(ActionStates);
            // For axis, we roll the state to the last known and clear the current state
            LastAxisStates = new Dictionary<Input, float>(AxisStates);
            AxisStates = new Dictionary<Input, float>();
        }

        #region GLFW Callbacks
        /// <summary>
        /// Configures callbacks for input from GLFW
        /// </summary>
        /// <param name="window"></param>
        public unsafe void ConfigureCallbacks(Window* window)
        {
            GLFW.SetKeyCallback(window, KeyCallback);
            GLFW.SetCharCallback(window, CharCallback);
            GLFW.SetMouseButtonCallback(window, MouseButtonCallback);
            GLFW.SetScrollCallback(window, ScrollCallback);
            GLFW.SetJoystickCallback(JoystickCallback);
            GLFW.SetCursorPosCallback(window, CursorPosCallback);
            GLFW.SetCursorEnterCallback(window, CursorEnterCallback);
        }

        /// <summary>
        /// Called when a key is pressed or released.
        /// </summary>
        /// <param name="window">Window pointer.</param>
        /// <param name="key">The key that triggered the event.</param>
        /// <param name="scancode">The scancode of the key.</param>
        /// <param name="action">The action taken (pressed, released, etc).</param>
        /// <param name="mods">Key modifiers (alt, ctrl, etc).</param>
        private unsafe void KeyCallback(Window* window, Keys key, int scancode, InputAction action, KeyModifiers mods)
        {
            if (key == Keys.Unknown) return;
            var input = KeyToInput(key);
            if (input == Input.Unknown) return;
            if (action == InputAction.Release)
            {
                ActionStates[input] = false;
            }
            else
            {
                ActionStates[input] = true;
            }
        }

        /// <summary>
        /// Called when a character is entered
        /// </summary>
        /// <param name="window">Window pointer.</param>
        /// <param name="codepoint">The unicode codepoint that was entered.</param>
        private unsafe void CharCallback(Window* window, uint codepoint)
        {
            // TODO
        }

        /// <summary>
        /// Called when a mouse button is pressed or released.
        /// </summary>
        /// <param name="window">Window pointer.</param>
        /// <param name="button">The mouse button that triggered the event.</param>
        /// <param name="action">The action taken (pressed, released, etc).</param>
        /// <param name="mods">Key modifiers (alt, ctrl, etc).</param>
        private unsafe void MouseButtonCallback(Window* window, MouseButton button, InputAction action, KeyModifiers mods)
        {
            var input = MouseButtonToInput(button);
            if (input == Input.Unknown) return;

            if (action == InputAction.Release)
            {
                ActionStates[input] = false;
            }
            else
            {
                ActionStates[input] = true;
            }
        }

        /// <summary>
        /// Called when the scroll wheel is moved.
        /// </summary>
        /// <param name="window">Window pointer.</param>
        /// <param name="offsetX">Scroll movement on X axis.</param>
        /// <param name="offsetY">Scroll movement on Y axis.</param>
        private unsafe void ScrollCallback(Window* window, double offsetX, double offsetY)
        {
            if (offsetX > 0)
            {
                ActionStates[Input.MOUSE_ScrollUp] = true;
            }
            else if (offsetX < 0)
            {
                ActionStates[Input.MOUSE_ScrollDown] = true;
            }
            SafeIncrementAxis(Input.MOUSE_ScrollAxisX, (float)offsetX);
            SafeIncrementAxis(Input.MOUSE_ScrollAxisY, (float)offsetY);
        }

        /// <summary>
        /// Called when a joystick event occurs.
        /// </summary>
        /// <param name="joy">The index of the joystick.</param>
        /// <param name="eventCode">The event that occurred.</param>
        private unsafe void JoystickCallback(int joy, ConnectedState eventCode)
        {
            // TODO
        }

        /// <summary>
        /// Called when the mouse is moved.
        /// </summary>
        /// <param name="window">Window pointer.</param>
        /// <param name="posX">The mouse's new X position.</param>
        /// <param name="posY">The mouse's new Y position.</param>
        private unsafe void CursorPosCallback(Window* window, double posX, double posY)
        {
            // Calculate movement delta
            var newPos = new Vector2((float)posX, (float)posY);
            var delta = newPos - MousePosition;

            // Store new position of mouse
            MousePosition = newPos;

            // Increment axis values
            SafeIncrementAxis(Input.MOUSE_Axis_X, delta.X);
            SafeIncrementAxis(Input.MOUSE_Axis_Y, delta.Y);
        }

        /// <summary>
        /// Called when the cursor enters or leaves the window.
        /// </summary>
        /// <param name="window">Window pointer.</param>
        /// <param name="entered">True if the mouse entered the window, false if it left.</param>
        private unsafe void CursorEnterCallback(Window* window, bool entered)
        {
            // TODO
        }
        #endregion

        /// <summary>
        /// Gets the state of the given input as an action. If the given input is an axial input,
        /// this method will return true for all non-zero values.
        /// </summary>
        /// <param name="trigger">The input to check.</param>
        /// <returns>True if the input is active, false if not.</returns>
        public bool GetStateAsAction(Input trigger)
        {
            if (IsAction(trigger))
            {
                return GetActionState(trigger);
            }
            else
            {
                return GetAxisState(trigger) != 0.0F;
            }
        }

        /// <summary>
        /// Gets the last state of the given input as an action. If the given input is an axial input,
        /// this method will return true for all non-zero values.
        /// </summary>
        /// <param name="trigger">The input to check.</param>
        /// <returns>True if the input is active, false if not.</returns>
        public bool GetLastStateAsAction(Input trigger)
        {
            if (IsAction(trigger))
            {
                return GetLastActionState(trigger);
            }
            else
            {
                return GetLastAxisState(trigger) != 0.0F;
            }
        }

        /// <summary>
        /// Gets the state of the given input as an axis. If the given input is an action input,
        /// this method will return 1.0F for pressed actions, and 0.0F otherwise.
        /// </summary>
        /// <param name="trigger">The input to check.</param>
        /// <returns>A float value representing the current state of the trigger.</returns>
        public float GetStateAsAxis(Input trigger)
        {
            if (IsAction(trigger))
            {
                return GetActionState(trigger)
                    ? 1.0F
                    : 0.0F;
            }
            else
            {
                return GetAxisState(trigger);
            }
        }

        /// <summary>
        /// Gets the last state of the given input as an axis. If the given input is an action input,
        /// this method will return 1.0F for pressed actions, and 0.0F otherwise.
        /// </summary>
        /// <param name="trigger">The input to check.</param>
        /// <returns>A float value representing the last state of the trigger.</returns>
        public float GetLastStateAsAxis(Input trigger)
        {
            if (IsAction(trigger))
            {
                return GetLastActionState(trigger)
                    ? 1.0F
                    : 0.0F;
            }
            else
            {
                return GetLastAxisState(trigger);
            }
        }

        /// <summary>
        /// Safely gets the current state of an action trigger.
        /// </summary>
        /// <param name="trigger">The trigger to check.</param>
        /// <returns>True if the trigger is pressed, false if not.</returns>
        public bool GetActionState(Input trigger)
        {
            return ActionStates.ContainsKey(trigger)
                ? ActionStates[trigger]
                : false;
        }

        /// <summary>
        /// Safely gets the last state of an action trigger.
        /// </summary>
        /// <param name="trigger">The trigger to check.</param>
        /// <returns>True if the trigger was pressed, false if not.</returns>
        public bool GetLastActionState(Input trigger)
        {
            return LastActionStates.ContainsKey(trigger)
                ? LastActionStates[trigger]
                : false;
        }

        /// <summary>
        /// Safely gets the current state of an axis trigger.
        /// </summary>
        /// <param name="trigger">The trigger to check.</param>
        /// <returns>A float value representing the current state of the trigger.</returns>
        public float GetAxisState(Input trigger)
        {
            return AxisStates.ContainsKey(trigger)
                ? AxisStates[trigger]
                : 0.0F;
        }

        /// <summary>
        /// Safely gets the last state of an axis trigger.
        /// </summary>
        /// <param name="trigger">The trigger to check.</param>
        /// <returns>A float value representing the previous state of the trigger.</returns>
        public float GetLastAxisState(Input trigger)
        {
            return LastAxisStates.ContainsKey(trigger)
                ? LastAxisStates[trigger]
                : 0.0F;
        }

        #region Helper Methods
        #region Mappers
        /// <summary>
        /// Maps a GLFW key to a NetEngine Input.
        /// </summary>
        /// <param name="in">The key to map.</param>
        /// <returns>The equivalent NetEngine Input.</returns>
        private static Input KeyToInput(Keys @in)
        {
            if (@in >= Keys.A && @in <= Keys.Z)
            {
                // handle alphabetical keys
                return (Input)@in - 65;
            }
            else if (@in >= Keys.D0 && @in <= Keys.D9)
            {
                // handle number row keys
                return (Input)@in - 20;
            }
            else if (@in >= Keys.F1 && @in <= Keys.F25)
            {
                // handle F keys
                return (Input)@in - 231;
            }
            else if (@in >= Keys.KeyPad0 && @in <= Keys.KeyPadEqual)
            {
                // handle numpad
                return (Input)@in - 222;
            }
            // handle manual mappings
            return @in switch
            {
                Keys.Space => Input.KEY_Space,
                Keys.Apostrophe => Input.KEY_Apostrophe,
                Keys.Comma => Input.KEY_Comma,
                Keys.Minus => Input.KEY_Minus,
                Keys.Period => Input.KEY_Period,
                Keys.Slash => Input.KEY_ForwardSlash,
                Keys.Semicolon => Input.KEY_Semicolon,
                Keys.Equal => Input.KEY_Equal,
                Keys.LeftBracket => Input.KEY_LeftBracket,
                Keys.RightBracket => Input.KEY_RightBracket,
                Keys.Backslash => Input.KEY_Backslash,
                Keys.GraveAccent => Input.KEY_Tilde,
                Keys.Escape => Input.KEY_Escape,
                Keys.Enter => Input.KEY_Enter,
                Keys.Tab => Input.KEY_Tab,
                Keys.Backspace => Input.KEY_Backspace,
                Keys.Insert => Input.KEY_Insert,
                Keys.Delete => Input.KEY_Delete,
                Keys.Home => Input.KEY_Home,
                Keys.End => Input.KEY_End,
                Keys.PageUp => Input.KEY_PageUp,
                Keys.PageDown => Input.KEY_PageDown,
                Keys.Up => Input.KEY_UpArrow,
                Keys.Down => Input.KEY_DownArrow,
                Keys.Left => Input.KEY_LeftArrow,
                Keys.Right => Input.KEY_RightArrow,
                Keys.CapsLock => Input.KEY_CapsLock,
                Keys.ScrollLock => Input.KEY_ScrollLock,
                Keys.NumLock => Input.KEY_NumLock,
                Keys.PrintScreen => Input.KEY_PrintScreen,
                Keys.Pause => Input.KEY_Pause,
                Keys.LeftShift => Input.KEY_LShift,
                Keys.LeftControl => Input.KEY_LCtrl,
                Keys.LeftAlt => Input.KEY_LAlt,
                Keys.RightShift => Input.KEY_RShift,
                Keys.RightControl => Input.KEY_RCtrl,
                Keys.RightAlt => Input.KEY_RAlt,
                Keys.Menu => Input.KEY_Menu,
                _ => Input.Unknown,
            };
        }

        /// <summary>
        /// Maps a GLFW mouse button to a NetEngine input.
        /// </summary>
        /// <param name="in">The mouse button to map.</param>
        /// <returns>The equivalent NetEngine Input.</returns>
        private static Input MouseButtonToInput(MouseButton @in)
        {
            return @in switch
            {
                MouseButton.Left => Input.MOUSE_LeftButton,
                MouseButton.Middle => Input.MOUSE_MiddleButton,
                MouseButton.Right => Input.MOUSE_RightButton,
                MouseButton.Button4 => Input.MOUSE_MouseButton4,
                MouseButton.Button5 => Input.MOUSE_MouseButton5,
                _ => Input.Unknown,
            };
        }
        #endregion

        /// <summary>
        /// Checks if the given input is an action trigger or not.
        /// </summary>
        /// <param name="input">The input to check.</param>
        /// <returns>True if the given input is an action input, false if not.</returns>
        private static bool IsAction(Input input)
        {
            return input >= 0 && input < Input.ACTION_MAX;
        }

        /// <summary>
        /// Helper method to safely increment an axis state, or store a new value if it's not set yet.
        /// </summary>
        /// <param name="axis">The axis to update or set.</param>
        /// <param name="incr">The value to store or add.</param>
        private void SafeIncrementAxis(Input axis, float incr)
        {
            if (AxisStates.ContainsKey(axis))
            {
                AxisStates[axis] += incr;
            }
            else
            {
                AxisStates[axis] = incr;
            }
        }
        #endregion
    }
}
