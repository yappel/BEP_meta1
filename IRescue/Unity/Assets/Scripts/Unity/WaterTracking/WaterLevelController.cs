// <copyright file="WaterLevelController.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Assets.Scripts.Unity
{
    using IRescue.Core.Utils;
    using System;
    using UnityEngine;

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
        /// Amount of milliseconds before the water level should be calculated
        /// </summary>
        private const long IntervalTime = 1000;

        /// <summary>
        /// Amount of measurements to buffer
        /// </summary>
        private const int BufferSize = 10;

        /// <summary>
        /// The timestamp in milliseconds of the previous calculation
        /// </summary>
        private long previousCalculation = 0;

        /// <summary>
        /// The water plane game object which will rise to occlude objects
        /// </summary>
        private GameObject waterPlane;

        /// <summary>
        /// Array of the returned water level calculations
        /// </summary>
        private float[] measurements;

        /// <summary>
        /// Current pointer to the measurements array
        /// </summary>
        private int pointer = 0;

        /// <summary>
        /// Initializes the water plane
        /// </summary>
        /// <param name="x">the width of the plane</param>
        /// <param name="z">the depth of the plane</param>
        public void Init(float x, float z)
        {
            this.waterPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
            this.waterPlane.name = "WaterPlane";
            this.waterPlane.transform.position = new Vector3(x * 5, -10, z * 5);
            this.waterPlane.transform.localScale = new Vector3(x, 1, z);
            this.waterPlane.GetComponent<MeshRenderer>().material.shader = Shader.Find("Masked/Mask");
            this.waterPlane.GetComponent<MeshRenderer>().material.renderQueue = 2990;
            this.measurements = new float[BufferSize];
            for (int i = 0; i < BufferSize; i++)
            {
                this.measurements[i] = -1;
            }
        }

        /// <summary>
        /// Track the water level on every frame update
        /// </summary>
        public void Update()
        {
            if (StopwatchSingleton.Time - this.previousCalculation > IntervalTime)
            {
                this.previousCalculation = StopwatchSingleton.Time;

                // TODO for each visible marker
                //      double waterLevel = WaterLevelTracker::CalculateWaterLevel(input);
                //      if (waterLevel > 0) {
                //          this.measurements[pointer % BufferSize] = waterLevel;
                //          this.pointer++;
                //      }
                this.SetWaterPositionY();
            }
        }

        /// <summary>
        /// Change the height of the water plane
        /// </summary>
        private void SetWaterPositionY()
        {
            float[] sorted = this.measurements;
            Array.Sort(sorted);
            this.waterPlane.transform.position = new Vector3(this.waterPlane.transform.position.x, sorted[sorted.Length / 2], this.waterPlane.transform.position.z);
        }
    }
}
