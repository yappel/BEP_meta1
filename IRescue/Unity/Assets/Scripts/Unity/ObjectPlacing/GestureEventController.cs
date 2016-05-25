// <copyright file="GestureEventController.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Assets.Scripts.Unity.ObjectPlacing
{
    using IRescue.Core.Utils;
    using Meta;
    using States;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// This controller keeps track of gesture events and passes it to a statecontext.
    /// </summary>
    public class GestureEventController : MonoBehaviour
    {
        /// <summary>
        /// The time before buttons can be pressed after switching states.
        /// </summary>
        private const int TimeBeforeAction = 2000;

        /// <summary>
        /// The scale of the buttons for 2d.
        /// </summary>
        private const float TwoDScale = 1.8f;

        /// <summary>
        /// Coupled state context.
        /// </summary>
        private StateContext stateContext;

        /// <summary>
        /// Check if left hand is valid
        /// </summary>
        private bool validLeftHand;

        /// <summary>
        /// Check if right hand is valid
        /// </summary>
        private bool validRightHand;

        /// <summary>
        /// Boolean to keep track if a state can be changed.
        /// Used to stop immediate transitions from one state to another.
        /// </summary>
        private bool canSwitchState;

        /// <summary>
        /// Bool if watching in 3d or not;
        /// </summary>
        private bool threeDMode = true;

        /// <summary>
        /// Method called on start. Initialize the StateContext
        /// </summary>
        public void Init()
        {
            this.stateContext = new StateContext(this);
        }

        /// <summary>
        /// Method called on every frame update.
        /// </summary>
        public void Update()
        {
            this.MonoStereo();
            this.stateContext.CurrentState.RunUpdate();
            if (this.CanSwitch())
            {
                this.GrabEvent();
                this.OpenEvent();
                this.PinchEvent();
                this.PointEvent();
            }
        }

        /// <summary>
        /// Calls the RunLateUpdate of the current state
        /// </summary>
        public void LateUpdate()
        {
            this.stateContext.CurrentState.RunLateUpdate();
        }

        /// <summary>
        /// Event when a back button is pressed.
        /// </summary>
        public void BackButtonEvent()
        {
            if (this.canSwitchState)
            {
                this.stateContext.CurrentState.OnBackButton();
            }
        }

        /// <summary>
        /// Event when a toggle button is pressed.
        /// </summary>
        public void ToggleButtonEvent()
        {
            if (this.canSwitchState)
            {
                this.stateContext.CurrentState.OnToggleButton();
            }
        }

        /// <summary>
        /// Event when a confirm button is pressed
        /// </summary>
        public void ConfirmButtonEvent()
        {
            if (this.canSwitchState)
            {
                this.stateContext.CurrentState.OnConfirmButton();
            }
        }

        /// <summary>
        /// Event when a modify rotate button is pressed
        /// </summary>
        public void ModifyRotateButtonEvent()
        {
            if (this.canSwitchState)
            {
                this.stateContext.CurrentState.OnRotateButton();
            }
        }

        /// <summary>
        /// Event when a modify translate button is pressed
        /// </summary>
        public void ModifyTranslateButtonEvent()
        {
            if (this.canSwitchState)
            {
                this.stateContext.CurrentState.OnTranslateButton();
            }
        }

        /// <summary>
        /// Event when a modify scale button is pressed
        /// </summary>
        public void ModifyScaleButtonEvent()
        {
            if (this.canSwitchState)
            {
                this.stateContext.CurrentState.OnScaleButton();
            }
        }

        /// <summary>
        /// Event when a delete object button is pressed
        /// </summary>
        public void DeleteButtonEvent()
        {
            if (this.canSwitchState)
            {
                this.stateContext.CurrentState.OnDeleteButton();
            }
        }

        /// <summary>
        /// Event when another object has been selected
        /// </summary>
        /// <param name="resourcePath">Name of the object, which is located in /Resources/Objects/ that should be loaded</param>
        public void SelectObjectButtonEvent(string resourcePath)
        {
            if (this.canSwitchState)
            {
                this.stateContext.SwapObject("Objects/" + resourcePath);
            }
        }

        /// <summary>
        /// Changes the button size if the monocular state changes.
        /// </summary>
        private void MonoStereo()
        {
            if (threeDMode && Meta.MetaCameraMode.monocular)
            {
                this.threeDMode = false;
                this.stateContext.Buttons.SetScale(TwoDScale);
            }
            else if (!threeDMode && !Meta.MetaCameraMode.monocular)
            {
                this.threeDMode = true;
                this.stateContext.Buttons.SetScale(1);
            }
        }

        /// <summary>
        /// Returns if a gesture is being performed.
        /// </summary>
        /// <param name="hand">Hand of the user</param>
        /// <param name="gesture">The gesture to be performed</param>
        /// <returns>if the gesture is being performed by the hand</returns>
        private bool IsValid(Hand hand, MetaGesture gesture)
        {
            return hand.isValid && hand.gesture.type == gesture;
        }

        /// <summary>
        /// Return which hand performed the event
        /// </summary>
        /// <param name="leftHandEvent">boolean if the left hand performed the event</param>
        /// <param name="rightHandEvent">boolean if the right hand performed the event</param>
        /// <returns>Which HandType performs the gesture</returns>
        private HandType GetHandType(bool leftHandEvent, bool rightHandEvent)
        {
            if (leftHandEvent && rightHandEvent)
            {
                return HandType.EITHER;
            }
            else if (leftHandEvent)
            {
                return HandType.LEFT;
            }
            else if (rightHandEvent)
            {
                return HandType.RIGHT;
            }
            else
            {
                return HandType.UNKNOWN;
            }
        }

        /// <summary>
        /// Event when the user performs a grab gesture, fist is closed.
        /// </summary>
        private void GrabEvent()
        {
            HandType handType = this.GetHandType(this.IsValid(Hands.left, MetaGesture.GRAB), this.IsValid(Hands.right, MetaGesture.GRAB));
            if (handType != HandType.UNKNOWN)
            {
                this.stateContext.CurrentState.OnGrab(handType);
            }
        }

        /// <summary>
        /// Event when the user opens a hand. An open hand.
        /// </summary>
        private void OpenEvent()
        {
            HandType handType = this.GetHandType(this.IsValid(Hands.left, MetaGesture.OPEN), this.IsValid(Hands.right, MetaGesture.OPEN));
            if (handType != HandType.UNKNOWN)
            {
                this.stateContext.CurrentState.OnOpen(handType);
            }
        }

        /// <summary>
        /// Event when the user performs a pinch gesture. A pinch using the thumb and index finger.
        /// </summary>
        private void PinchEvent()
        {
            HandType handType = this.GetHandType(this.IsValid(Hands.left, MetaGesture.PINCH), this.IsValid(Hands.right, MetaGesture.PINCH));
            if (handType != HandType.UNKNOWN)
            {
                this.stateContext.CurrentState.OnPinch(handType);
            }
        }

        /// <summary>
        /// Method for the point event. A single finger fully extended.
        /// </summary>
        private void PointEvent()
        {
            Vector3 point = new Vector3();
            GameObject gameObject = null;
            Vector3 cameraPosition = this.gameObject.transform.position;
            if (this.IsValid(Hands.right, MetaGesture.POINT))
            {
                point = this.GetClosestPoint(Physics.RaycastAll(new Ray(cameraPosition, Hands.right.pointer.position - cameraPosition), Mathf.Infinity), out gameObject);
            }
            else if (this.IsValid(Hands.left, MetaGesture.POINT))
            {
                point = this.GetClosestPoint(Physics.RaycastAll(new Ray(cameraPosition, Hands.left.pointer.position - cameraPosition), Mathf.Infinity), out gameObject);
            }

            this.PointEvent(gameObject, point);
        }

        /// <summary>
        /// Triggers the event to the state if a collision with an object unequal to MetaWorld or WaterLevelController could be found.
        /// </summary>
        /// <param name="gameObject">The game object of the collision</param>
        /// <param name="point">The location of the collision</param>
        private void PointEvent(GameObject gameObject, Vector3 point)
        {
            if (gameObject != null && gameObject.GetComponent<WaterLevelController>() == null)
            {
                if (gameObject.GetComponent<MetaBody>() == null)
                {
                    this.stateContext.CurrentState.OnPoint(point);
                }
                else
                {
                    this.stateContext.CurrentState.OnPoint(gameObject);
                }
            }
        }

        /// <summary>
        /// Return the coordinate and game object of the closest object pointed at.
        /// </summary>
        /// <param name="hits">The hits of a ray cast</param>
        /// <param name="gameObject">the resulting game object</param>
        /// <returns>the location pointed at</returns>
        private Vector3 GetClosestPoint(RaycastHit[] hits, out GameObject gameObject)
        {
            Vector3 res = new Vector3();
            GameObject o = null;
            float minDistance = float.MaxValue;
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].transform.gameObject.GetComponent<Button>() != null)
                {
                    gameObject = null;
                    return new Vector3();
                }

                if (hits[i].distance < minDistance && (hits[i].transform.gameObject.GetComponent<GroundPlane>() != null || hits[i].transform.gameObject.GetComponent<MetaBody>() != null))
                {
                    minDistance = hits[i].distance;
                    res = hits[i].point;
                    o = hits[i].transform.gameObject;
                }
            }

            gameObject = o;
            return res;
        }

        /// <summary>
        /// Check if enough time passed since switching between states.
        /// </summary>
        /// <returns>boolean if enough time has passed</returns>
        private bool CanSwitch()
        {
            this.canSwitchState = StopwatchSingleton.Time - this.stateContext.PreviousSwitchTime > TimeBeforeAction;
            return this.canSwitchState;
        }
    }
}
