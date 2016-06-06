// <copyright file="WaterLevelController.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Assets.Scripts.Unity
{
    using Meta;
    using UnityEngine;
    using WaterLevelTracking;

    /// <summary>
    /// Keep track of the water level
    /// </summary>
    public class WaterLevelController : MonoBehaviour
    {
        /// <summary>
        /// Indicates to take the color camera feed
        /// </summary>
        private const int SourceDevice = 0;

        /// <summary>
        /// The water plane game object which will rise to occlude objects
        /// </summary>
        private GameObject waterPlane;

        /// <summary>
        /// The water level tracker which calculates the water level
        /// </summary>
        private WaterLevelTracker waterLevelTracker;

        /// <summary>
        /// Initializes the water plane
        /// </summary>
        /// <param name="parent">Parent of the water plane, world box</param>
        /// <param name="x">the width of the plane in meters</param>
        /// <param name="z">the depth of the plane in meters</param>
        public void Init(Transform parent, float x, float z)
        {
            this.waterLevelTracker = new WaterLevelTracker();
            this.waterPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
            this.waterPlane.name = "WaterPlane";
            this.waterPlane.transform.position = new Vector3(x, -10, z);
            this.waterPlane.transform.localScale = new Vector3(x / 5f, 1, z / 5f);
            this.waterPlane.GetComponent<MeshRenderer>().material.shader = Shader.Find("Masked/Mask");
            this.waterPlane.GetComponent<MeshRenderer>().material.renderQueue = 3020;
            this.waterPlane.transform.parent = parent;
        }

        /// <summary>
        /// Track the water level on every frame update
        /// </summary>
        public void Update()
        {
            /*
             * ts: long
             * Foreach visible marker
             *  Get camera position (projection of xyz)
             *  Get rotation (z)
             *  waterLevelTracker calculate level (camerafeed, markerlocation, markersize, rotation, ts)
             *  
             * this.gameObject.position = new Vector3(this.gameObject.position.x, waterLevelTracker.GetWaterLevel(), this.gameObject.position.z)
             */
        }
    }
}
