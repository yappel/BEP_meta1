// <copyright file="AbstractState.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Assets.Scripts.Unity.ObjectPlacing.States
{
    using System.Collections.Generic;
    using IRescue.Core.Utils;
    using Meta;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// Abstract class for the implemented States.
    /// </summary>
    public abstract class AbstractState
    {
        /// <summary>
        /// Time before states should be able to switch
        /// </summary>
        private const long TimeBeforeSwitch = 1500;

        /// <summary>
        /// The list of all the buttons of the current interface.
        /// </summary>
        private List<GameObject> buttons;

        /// <summary>
        /// The timestamp of the constructor call
        /// </summary>
        private long initTimestamp;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractState"/> class.
        /// </summary>
        /// <param name="stateContext">The class that keeps track of the current active state</param>
        protected AbstractState(StateContext stateContext)
        {
            this.initTimestamp = StopwatchSingleton.Time;
            this.StateContext = stateContext;
            this.buttons = new List<GameObject>();
        }

        /// <summary>
        /// Gets the stateContext.
        /// Coupled state context which keeps track of the current active state.
        /// </summary>
        public StateContext StateContext { get; private set; }

        /// <summary>
        /// Method when a grab event has occurred. A closed fist.
        /// </summary>
        /// <param name="hand">The hand(s) which perform a grab gesture</param>
        public virtual void OnGrab(HandType hand)
        {
        }

        /// <summary>
        /// Method when a hand is open. An open hand.
        /// </summary>
        /// <param name="hand">The hand(s) which perform a pinch gesture</param>
        public virtual void OnOpen(HandType hand)
        {
        }

        /// <summary>
        /// Method when a pinch event has occurred. A pinch using the thumb and index finger.
        /// </summary>
        /// <param name="hand">The hand(s) which perform a pinch gesture</param>
        public virtual void OnPinch(HandType hand)
        {
        }

        /// <summary>
        /// Method when a point event has occurred to place a building (ground plane). A single finger fully extended.
        /// </summary>
        /// <param name="position">The position pointed towards</param>
        /// <param name="handType">The hand that is pointing</param>
        public virtual void OnPoint(Vector3 position, HandType handType)
        {
        }

        /// <summary>
        /// Method when a point event has occurred towards a building. A single finger fully extended.
        /// </summary>
        /// <param name="gameObject">The gameObject pointed at</param>
        /// <param name="handType">The hand that is pointing</param>
        public virtual void OnPoint(GameObject gameObject, HandType handType)
        {
        }

        /// <summary>
        /// Run an update on the state.
        /// </summary>
        public virtual void RunUpdate()
        {
        }

        /// <summary>
        /// Run a late update on the state.
        /// </summary>
        public virtual void RunLateUpdate()
        {
        }

        /// <summary>
        /// Discard the state, delete the interface
        /// </summary>
        public virtual void DiscardState()
        {
            for (int i = 0; i < this.buttons.Count; i++)
            {
                UnityEngine.Object.Destroy(this.buttons[i]);
            }
        }

        /// <summary>
        /// Check if the state can be switched
        /// </summary>
        /// <returns>if enough time has passed to switch states</returns>
        protected bool CanSwitchState()
        {
            return StopwatchSingleton.Time - this.initTimestamp > TimeBeforeSwitch;
        }

        /// <summary>
        /// Add a button to a state interface.
        /// </summary>
        /// <param name="buttonName">The name of the button located in Prefabs/Buttons/</param>
        /// <param name="action">The lambda calculus</param>
        /// <returns>The created button game object</returns>
        protected GameObject InitButton(string buttonName, UnityEngine.Events.UnityAction action)
        {
            GameObject button = this.GetButton(UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/Buttons/" + buttonName)));
            if (button.transform.GetComponentInChildren<Button>() != null)
            {
                button.transform.GetComponentInChildren<Button>().onClick.AddListener(action);
            }
            
            this.buttons.Add(button.transform.root.gameObject);
            button.transform.root.SetParent(ButtonWrapper.Wrapper, false);
            return button;
        }

        /// <summary>
        /// Create and set the text of a new text pane
        /// </summary>
        /// <param name="textPaneName">the name of the text pane in Prefabs/Buttons/</param>
        /// <param name="text">The text to set in the pane</param>
        /// <returns>The created button game object</returns>
        protected GameObject InitTextPane(string textPaneName, string text)
        {
            GameObject button = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/Buttons/" + textPaneName));
            button.GetComponentInChildren<Text>().text = text;
            this.buttons.Add(button.transform.root.gameObject);
            button.transform.root.SetParent(ButtonWrapper.Wrapper, false);
            return button;
        }

        /// <summary>
        /// Return the button in the MGUI canvas.
        /// </summary>
        /// <param name="buttonWrapper">The root of the game object</param>
        /// <returns>The MGUI.Button game objects</returns>
        private GameObject GetButton(GameObject buttonWrapper)
        {
            return buttonWrapper.transform.GetChild(0).gameObject;
        }
    }
}
