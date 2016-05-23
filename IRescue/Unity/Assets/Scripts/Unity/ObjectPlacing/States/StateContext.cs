// <copyright file="StateContext.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Assets.Scripts.Unity.ObjectPlacing.States
{
    using IRescue.Core.Utils;
    using UnityEngine;

    /// <summary>
    ///  Context for keeping track of the current states.
    /// </summary>
    public class StateContext
    {
        /// <summary>
        /// Path for the default object
        /// </summary>
        public const string DefaultObjectPath = "Objects/DefaultObject";

        /// <summary>
        /// Initializes a new instance of the <see cref="StateContext"/> class.
        /// </summary>
        /// <param name="controller">The state controller which tracks events</param>
        public StateContext(StateController controller)
        {
            this.Buttons = new ButtonHandler(controller);
            this.CurrentState = new NeutralState(this);
            this.SwapObject(DefaultObjectPath);
        }

        /// <summary>
        /// Gets the time of the last state switch
        /// </summary>
        public long PreviousSwitchTime { get; private set; }

        /// <summary>
        /// Gets or sets the selected building path
        /// </summary>
        public string SelectedBuilding { get; set; }

        /// <summary>
        /// Gets the button handler
        /// </summary>
        public ButtonHandler Buttons { get; private set; }

        /// <summary>
        /// Gets the currentState.
        /// </summary>
        public AbstractState CurrentState { get; private set; }

        /// <summary>
        /// Sets the currentState.
        /// </summary>
        /// <param name="newState">The new current state</param>
        public void SetState(AbstractState newState)
        {
            this.PreviousSwitchTime = StopwatchSingleton.Time;
            this.CurrentState = newState;
        }

        /// <summary>
        /// Swap the object that will be placed when placing a new object.
        /// </summary>
        /// <param name="gameObjectPath">The path of the new selected object for the object to place</param>
        public void SwapObject(string gameObjectPath)
        {
            this.SelectedBuilding = gameObjectPath;
        }
    }
}
