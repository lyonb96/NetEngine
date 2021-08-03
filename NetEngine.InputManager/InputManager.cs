namespace NetEngine.InputManager
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Input;
    using Core;
    using Newtonsoft.Json;
    using OpenTK.Mathematics;

    /// <summary>
    /// The Input Manager wraps input binding functionality and acts as the go-between
    /// for gameplay-level calls and low-level input state checks.
    /// </summary>
    public class InputManager
    {
        /// <summary>
        /// The low-level input handler.
        /// </summary>
        private readonly InputHandler InputHandler;
        /// <summary>
        /// Registered action bindings.
        /// </summary>
        private Dictionary<string, ActionBinding> ActionBindings;
        /// <summary>
        /// Registered axis bindings.
        /// </summary>
        private Dictionary<string, AxisBinding> AxisBindings;

        /// <summary>
        /// Constructs an instance of the input manager and populates internal structures.
        /// </summary>
        /// <param name="handler">The input handler to use.</param>
        public InputManager(InputHandler handler)
        {
            InputHandler = handler;
            ActionBindings = new Dictionary<string, ActionBinding>();
            AxisBindings = new Dictionary<string, AxisBinding>();
        }

        #region Save/Load
        /// <summary>
        /// Attempts to load input bindings from the saved config.
        /// </summary>
        public void LoadBindings()
        {
            var path = GetBindingFilePath();
            if (!File.Exists(path))
            {
                // TODO: load default bindings and save them to the file
                return;
            }
            var bindings = JsonConvert.DeserializeObject<SaveLoadBindingWrapper>(File.ReadAllText(path));
            if (bindings == null)
            {
                return;
            }
            ActionBindings = bindings.ActionBindings
                .ToDictionary(x => x.Name, x => x);
            AxisBindings = bindings.AxisBindings
                .ToDictionary(x => x.Name, x => x);
        }

        /// <summary>
        /// Attempts to save input bindings to the binding config file.
        /// </summary>
        public void SaveBindings()
        {
            var path = GetBindingFilePath();
            var bindings = new SaveLoadBindingWrapper
            {
                ActionBindings = ActionBindings.Values.ToList(),
                AxisBindings = AxisBindings.Values.ToList(),
            };
            File.WriteAllText(path, JsonConvert.SerializeObject(bindings));
        }
        #endregion

        /// <summary>
        /// Handles post-frame input cleanup stuff.
        /// </summary>
        public void PostFrame()
        {
            InputHandler.PostFrame();
        }

        #region Binding Management
        /// <summary>
        /// Registers a new action binding with the given name and triggers.
        /// </summary>
        /// <param name="name">The new name for the action binding.</param>
        /// <param name="inputs">Preset inputs for the binding.</param>
        /// <remarks>This will overwrite existing bindings of the same name!</remarks>
        public void RegisterActionBinding(string name, params Input[] inputs)
        {
            ActionBindings[name] = new ActionBinding(name)
            {
                Triggers = inputs?.ToList() ?? new List<Input>(),
            };
        }

        /// <summary>
        /// Registers a new axis binding with the given name and trigger.
        /// </summary>
        /// <param name="name">The new name for the axis binding.</param>
        /// <param name="trigger">Preset trigger for the binding.</param>
        /// <param name="multiplier">Multiplier for the preset trigger.</param>
        /// <remarks>This will overwrite existing bindings of the same name!</remarks>
        public void RegisterAxisBinding(string name, Input trigger, float multiplier = 1.0F)
        {
            AxisBindings[name] = new AxisBinding(name);
            AxisBindings[name].Triggers.Add(new AxisTrigger
            {
                Trigger = trigger,
                Multiplier = multiplier,
            });
        }

        /// <summary>
        /// Sets a new trigger for an action binding.
        /// </summary>
        /// <param name="name">The name of the binding to add the trigger to.</param>
        /// <param name="trigger">The new trigger to set.</param>
        /// <param name="bindingIndex">(Optional) index to set the trigger on, default is end of the list.</param>
        public void SetActionBindingTrigger(string name, Input trigger, int bindingIndex = -1)
        {
            if (ActionBindings.TryGetValue(name, out var binding))
            {
                if (bindingIndex == -1)
                {
                    // -1 = just append it to the list
                    binding.Triggers.Add(trigger);
                }
                else if (bindingIndex >= binding.Triggers.Count)
                {
                    // Populate empty entries below this one with "Unknown" then set this one
                    while (binding.Triggers.Count < bindingIndex)
                    {
                        binding.Triggers.Add(Input.Unknown);
                    }
                    binding.Triggers.Add(trigger);
                }
                else
                {
                    // Overwrite the value stored at the index
                    binding.Triggers[bindingIndex] = trigger;
                }
            }
            else
            {
                ActionBindings[name] = new ActionBinding(name);
                SetActionBindingTrigger(name, trigger, bindingIndex);
            }
        }

        /// <summary>
        /// Sets a new trigger for an axis binding.
        /// </summary>
        /// <param name="name">The name of the binding to add the trigger to.</param>
        /// <param name="trigger">The new trigger to set.</param>
        /// <param name="multiplier">(Optional) multiplier for the trigger, defaults to 1.0.</param>
        /// <param name="bindingIndex">(Optional) index to set the trigger on, default is end of the list.</param>
        public void SetAxisBindingTrigger(string name, Input trigger, float multiplier = 1.0F, int bindingIndex = -1)
        {
            if (AxisBindings.TryGetValue(name, out var binding))
            {
                if (bindingIndex == -1)
                {
                    // -1 = just append it to the list
                    binding.Triggers.Add(new AxisTrigger
                    {
                        Trigger = trigger,
                        Multiplier = multiplier,
                    });
                }
                else if (bindingIndex >= binding.Triggers.Count)
                {
                    // Populate empty entries below this one with "Unknown" then set this one
                    while (binding.Triggers.Count < bindingIndex)
                    {
                        binding.Triggers.Add(new AxisTrigger
                        {
                            Trigger = Input.Unknown,
                            Multiplier = 1.0F,
                        });
                    }
                    binding.Triggers.Add(new AxisTrigger
                    {
                        Trigger = trigger,
                        Multiplier = multiplier,
                    });
                }
                else
                {
                    binding.Triggers[bindingIndex] = new AxisTrigger
                    {
                        Trigger = trigger,
                        Multiplier = multiplier,
                    };
                }
            }
            else
            {
                AxisBindings[name] = new AxisBinding(name);
                SetAxisBindingTrigger(name, trigger, multiplier, bindingIndex);
            }
        }
        #endregion

        #region State Checks
        /// <summary>
        /// Checks if an action binding is currently pressed.
        /// </summary>
        /// <param name="actionName">The binding to check.</param>
        /// <returns>Returns true if the binding is currently pressed, false if not.</returns>
        public bool IsPressed(string actionName)
        {
            if (ActionBindings.TryGetValue(actionName, out var binding))
            {
                foreach (var trigger in binding.Triggers)
                {
                    if (InputHandler.GetStateAsAction(trigger))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Checks if an action binding is currently pressed and was not previously pressed.
        /// </summary>
        /// <param name="actionName">The binding to check.</param>
        /// <returns>True if the action is pressed this frame and not pressed last frame, false otherwise.</returns>
        public bool IsJustPressed(string actionName)
        {
            if (ActionBindings.TryGetValue(actionName, out var binding))
            {
                foreach (var trigger in binding.Triggers)
                {
                    if (InputHandler.GetStateAsAction(trigger)
                        && !InputHandler.GetLastStateAsAction(trigger))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Checks if an action binding is currently released and was previously pressed.
        /// </summary>
        /// <param name="actionName">The binding to check.</param>
        /// <returns>True if the action is released this frame and pressed last frame, false otherwise.</returns>
        public bool IsJustReleased(string actionName)
        {
            if (ActionBindings.TryGetValue(actionName, out var binding))
            {
                foreach (var trigger in binding.Triggers)
                {
                    if (!InputHandler.GetStateAsAction(trigger)
                        && InputHandler.GetLastStateAsAction(trigger))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Checks the current state of an axis binding.
        /// </summary>
        /// <param name="axisName">The binding to check.</param>
        /// <returns>
        /// The "largest" axis state (by absolute value, scaled by the multiplier) of the binding,
        /// or 0.0F if the binding is invalid.
        /// </returns>
        public float GetAxisState(string axisName)
        {
            if (AxisBindings.TryGetValue(axisName, out var binding))
            {
                var max = 0.0F;
                foreach (var trigger in binding.Triggers)
                {
                    var triggerState = InputHandler.GetStateAsAxis(trigger.Trigger) * trigger.Multiplier;
                    if (MathHelper.Abs(triggerState) > MathHelper.Abs(max))
                    {
                        max = triggerState;
                    }
                }
                return max;
            }
            return 0.0F;
        }
        #endregion

        /// <summary>
        /// Helper method for building the path to the binding file.
        /// </summary>
        /// <returns>The path to the input binding file.</returns>
        private string GetBindingFilePath()
        {
            return Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                GlobalConfig.Instance.InstanceName,
                "bindings.json");
        }
    }
}
