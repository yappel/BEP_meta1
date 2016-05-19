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
        public void Init()
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
                if (hits[i].distance < minDistance && hits[i].transform.gameObject.GetComponent<GroundPlane>() != null)
                {
                    minDistance = hits[i].distance;
                    res = hits[i].point;
                    o = hits[i].transform.gameObject;
                }
            }

            gameObject = o;
            return res;
        }
    }
}
