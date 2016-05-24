// <copyright file="StateContext.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Assets.Scripts.Unity.ObjectPlacing.States
{
    using UnityEngine;

    /// <summary>
    ///  Context for keeping track of the current states.
    /// </summary>
    public class StateContext : MonoBehaviour
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StateContext"/> class.
        /// </summary>
        public StateContext()
        {
            this.Buttons = new ButtonHandler(this);
            this.CurrentState = new NeutralState(this);
            this.SwapObject(Resources.Load<GameObject>("Objects/DefaultObject/Instance"));
            this.SelectedBuilding.SetActive(false);
        }

        /// <summary>
        /// Gets or sets the selected building
        /// </summary>
        public GameObject SelectedBuilding { get; set; }

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
            this.CurrentState = newState;
        }

        /// <summary>
        /// Swap the object that will be placed when placing a new object.
        /// </summary>
        /// <param name="gameObject">The new selected object for the object to place</param>
        public void SwapObject(GameObject gameObject)
        {
            UnityEngine.Object.Destroy(this.SelectedBuilding);
            UnityEngine.Object.Instantiate<GameObject>(gameObject);
            gameObject.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            this.SelectedBuilding = gameObject;
            this.SelectedBuilding.SetActive(false);
        }
    }
}
