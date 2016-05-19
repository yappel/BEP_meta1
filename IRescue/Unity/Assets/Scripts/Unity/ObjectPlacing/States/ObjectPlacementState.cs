// <copyright file="ObjectPlacementState.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Assets.Scripts.Unity.ObjectPlacing.States
{
    using IRescue.Core.Utils;
    using Meta;
    using UnityEngine;

    /// <summary>
    /// State when placing a building by pointing.
    /// </summary>
    public class ObjectPlacementState : AbstractState
    {
        /// <summary>
        /// Coupled state context.
        /// </summary>
        private StateContext stateContext;

        /// <summary>
        /// The indication for where the building will be placed.
        /// </summary>
        private GameObject buildingIndication;

        /// <summary>
        ///  The time that will be kept to not immediately place buildings.
        /// </summary>
        private long hoverTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectPlacementState"/> class.
        /// </summary>
        /// <param name="stateContext">State context</param>
        /// <param name="location">First indicated position of the placement</param>
        public ObjectPlacementState(StateContext stateContext, Vector3 location)
        {
            this.stateContext = stateContext;
            this.hoverTime = StopwatchSingleton.Time;
            this.InitIndicator(location);
        }

        /// <summary>
        /// Shows the position of the to be placed object and places it after having hovered for 3 seconds.
        /// </summary>
        /// <param name="position">position of the to be placed building</param>
        public override void OnPoint(Vector3 position)
        {
            long time = StopwatchSingleton.Time;
            if ((position - this.buildingIndication.transform.position).magnitude > (position.magnitude / 50f))
            {
                this.buildingIndication.GetComponent<Renderer>().material.SetColor("_Color", Color.yellow);
                this.hoverTime = time;
            }
            else
            {
                this.buildingIndication.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
            }

            this.buildingIndication.transform.position = position;
            if (time - this.hoverTime > 2000)
            {
                if (time - this.hoverTime < 2250)
                {
                    this.PlaceBuilding(this.buildingIndication.transform.position);
                } 
                else
                {
                    this.hoverTime = time;
                }
            }
        }

        /// <summary>
        /// Sets the state to modify an object.
        /// </summary>
        /// <param name="gameObject">the game object that was pointed at</param>
        public override void OnPoint(GameObject gameObject)
        {
            Object.Destroy(this.buildingIndication);
            this.stateContext.SetState(new ModifyState(this.stateContext, gameObject));
        }

        /// <summary>
        /// Creates a placement indicator
        /// </summary>
        /// <param name="location">The location where to place the indicator</param>
        private void InitIndicator(Vector3 location)
        {
            this.buildingIndication = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            this.buildingIndication.transform.localScale = new Vector3(0.25f, 0.1f, 0.25f);
            this.buildingIndication.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
            this.buildingIndication.transform.position = location;
        }

        /// <summary>
        /// Place a new building at a given location.
        /// </summary>
        /// <param name="position">Coordinate of the position to place the building</param>
        private void PlaceBuilding(Vector3 position)
        {
            GameObject newBuilding = GameObject.CreatePrimitive(PrimitiveType.Cube);
            newBuilding.AddComponent<GroundPlane>();
            newBuilding.AddComponent<MetaBody>();
            newBuilding.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            newBuilding.transform.position = new Vector3(position.x, position.y + 0.1f, position.z);
            Object.Destroy(this.buildingIndication);
            this.stateContext.SetState(new ModifyState(this.stateContext, newBuilding));
        }
    }
}
