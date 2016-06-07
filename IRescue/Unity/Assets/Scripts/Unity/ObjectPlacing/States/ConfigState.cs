// <copyright file="ConfigState.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Assets.Scripts.Unity.ObjectPlacing.States
{
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    ///  State when the application is running and no more buildings have to be places.
    /// </summary>
    public class ConfigState : AbstractState
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigState"/> class.
        /// </summary>
        /// <param name="stateContext">State context</param>
        public ConfigState(StateContext stateContext) : base(stateContext)
        {
            this.InitListeners(this.InitTextPane("ConfigWindow", "Configuration"));
            this.InitButton("BackButton", () => this.OnBackButton());
        }

        /// <summary>
        /// Add listeners to the buttons in the configuration window
        /// </summary>
        /// <param name="configWindow">the initialized configuration window prefab</param>
        private void InitListeners(GameObject configWindow)
        {
            configWindow.GetComponentInChildren<Button>().onClick.AddListener(() => this.OnEditModeButton());
            Toggle[] toggles = configWindow.GetComponentsInChildren<Toggle>();
            for (int i = 0; i < toggles.Length; i++)
            {
                switch (toggles[i].name)
                {
                    case "2DButton":
                        if (Meta.MetaCameraMode.monocular)
                        {
                            toggles[i].isOn = true;
                        }

                        toggles[i].onValueChanged.AddListener((x) => this.Set2D(true));
                        break;
                    case "3DButton":
                        if (!Meta.MetaCameraMode.monocular)
                        {
                            toggles[i].isOn = true;
                        }
                        toggles[i].onValueChanged.AddListener((x) => this.Set2D(false));
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Sets the camera mode to 2d or 3d
        /// </summary>
        /// <param name="set2d">boolean if the camera mode should be set to 2d or 3d elsewise</param>
        private void Set2D(bool set2d)
        {
            Meta.MetaCameraMode.monocular = set2d;
        }

        /// <summary>
        /// Return to the running state
        /// </summary>
        private void OnBackButton()
        {
            if (this.CanSwitchState())
            {
                this.StateContext.SetState(new RunningState(this.StateContext));
            }
        }

        /// <summary>
        /// Return to the neutral state
        /// </summary>
        private void OnEditModeButton()
        {
            if (this.CanSwitchState())
            {
                this.StateContext.SetState(new NeutralState(this.StateContext));
            }
        }
    }
}
