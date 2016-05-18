// <copyright file="StateController.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Assets.Scripts.Unity.ObjectPlacing
{
    using Meta;
    using States;
    using UnityEngine;

    /// <summary>
    ///  Controller for holding track of the gestures and states.
    /// </summary>
    public class StateController : MonoBehaviour
    {
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
        /// Method called on start. Initialize the StateContext
        /// </summary>
        public void Start()
        {
            this.stateContext = new StateContext();
        }

        /// <summary>
        /// Method called on every frame update.
        /// </summary>
        public void Update()
        {
            this.stateContext.CurrentState.RunUpdate();
            this.GrabEvent();
            this.OpenEvent();
            this.PinchEvent();
            this.PointEvent();
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
            if (this.IsValid(Hands.right, MetaGesture.POINT))
            {
                point = Hands.right.pointer.objectOfInterest.transform.position;
                gameObject = Hands.right.pointer.objectOfInterest.transform.gameObject;
            }
            else if (this.IsValid(Hands.left, MetaGesture.POINT))
            {
                point = Hands.left.pointer.objectOfInterest.transform.position;
                gameObject = Hands.left.pointer.objectOfInterest.transform.gameObject;
            }

            this.PointEvent(gameObject, point);
        }

        /// <summary>
        /// Triggers the event to the state if a collision with an object unequal to MetaWorld or WaterLevelController could be found.
        /// </summary>
        /// <param name="gameObject">The gameobject of the collision</param>
        /// <param name="point">The location of the collision</param>
        private void PointEvent(GameObject gameObject, Vector3 point)
        {
            if (gameObject != null && gameObject.name != "MetaWorld" && gameObject.GetComponent<WaterLevelController>() == null)
            {
                if (gameObject.GetComponent<GroundPlane>() != null)
                {
                    this.stateContext.CurrentState.OnPoint(point);
                }
                else
                {
                    this.stateContext.CurrentState.OnPoint(gameObject);
                }
            }
        }
    }
}
