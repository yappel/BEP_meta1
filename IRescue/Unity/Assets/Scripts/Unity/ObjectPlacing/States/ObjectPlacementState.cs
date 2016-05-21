// <copyright file="ObjectPlacementState.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Assets.Scripts.Unity.ObjectPlacing.States
{
    using Assets.Unity.Navigation;
    using IRescue.Core.Utils;
    using Meta;
    using UnityEngine;

    /// <summary>
    /// State when placing a building by pointing.
    /// </summary>
    public class ObjectPlacementState : AbstractState
    {
        /// <summary>
        ///  The time that will be kept to not immediately place buildings.
        /// </summary>
        private long hoverTime;

        /// <summary>
        /// True if a building is being moved, else false
        /// </summary>
        private bool translateModification;

        /// <summary>
        /// The game object with mesh that the user has chosen that will be placed.
        /// </summary>
        private GameObject gameObject;

        /// <summary>
        /// The original position of a building that will be moved.
        /// </summary>
        private Vector3 previousPosition;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectPlacementState"/> class.
        /// </summary>
        /// <param name="stateContext">State context</param>
        /// <param name="location">First indicated position of the placement</param>
        /// <param name="gameObject">The game object that has to be placed or moved</param>
        public ObjectPlacementState(StateContext stateContext, Vector3 location, GameObject gameObject) : base(stateContext)
        {
            gameObject.SetActive(true);
            this.translateModification = gameObject.GetComponent<MetaBody>() != null;
            if (this.translateModification)
            {
                UnityEngine.Object.Destroy(gameObject.GetComponent<MetaBody>());
                UnityEngine.Object.Destroy(gameObject.GetComponent<GroundPlane>());
            }

            this.hoverTime = StopwatchSingleton.Time;
            this.gameObject = gameObject;
            UnityEngine.Object.Instantiate(Resources.Load("Prefabs/Buttons/BackButton"));
            this.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
            this.gameObject.transform.position = location;
        }

        /// <summary>
        /// Shows the position of the to be placed object and places it after having hovered for 3 seconds.
        /// </summary>
        /// <param name="position">position of the to be placed building</param>
        public override void OnPoint(Vector3 position)
        {
            long time = StopwatchSingleton.Time;
            if ((position - this.gameObject.transform.position).magnitude > (position.magnitude / 50f))
            {
                this.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.yellow);
                this.hoverTime = time;
            }
            else
            {
                this.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
            }

            this.gameObject.transform.position = position;
            if (time - this.hoverTime > 2500)
            {
                if (time - this.hoverTime < 2750)
                {
                    this.PlaceBuilding();
                } 
                else
                {
                    this.hoverTime = time;
                }
            }
        }

        /// <summary>
        /// If colliding with an other object, the indicator turns red and you cannot place a building.
        /// </summary>
        /// <param name="gameObject">the game object that was pointed at</param>
        public override void OnPoint(GameObject gameObject)
        {
            this.hoverTime = StopwatchSingleton.Time;
            this.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
        }

        /// <summary>
        /// Go back to the neutral or modify state based on the state that called it.
        /// </summary>
        public override void OnBackButton()
        {
            UnityEngine.Object.Destroy(GameObject.FindObjectOfType<BackButton>().transform.root.gameObject);
            if (this.translateModification)
            {
                this.gameObject.transform.position = this.previousPosition;
                this.PlaceBuilding();
            }
            else
            {
                UnityEngine.Object.Destroy(this.gameObject);
                this.StateContext.SetState(new NeutralState(this.StateContext));
            }
        }

        /// <summary>
        /// Place a new building at the pointed location.
        /// </summary>
        private void PlaceBuilding()
        {
            this.gameObject.AddComponent<GroundPlane>();
            this.gameObject.AddComponent<MetaBody>();
            this.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.gray);
            this.StateContext.SetState(new ModifyState(this.StateContext, this.gameObject));
            UnityEngine.Object.Destroy(GameObject.FindObjectOfType<BackButton>().transform.root.gameObject);
        }
    }
}
