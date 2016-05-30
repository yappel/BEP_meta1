// <copyright file="WaterLevelController.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Assets.Scripts.Unity
{
    using UnityEngine;

    /// <summary>
    /// Keep track of the water level
    /// </summary>
    public class WaterLevelController : MonoBehaviour
    {
        private GameObject waterPlane;

        /// <summary>
        /// Initializes the water plane
        /// </summary>
        /// <param name="x">the width of the plane</param>
        /// <param name="z">the depth of the plane</param>
        public void Init(float x, float z)
        {
            this.gameObject.transform.position = new Vector3(x / 5, 0, z / 5);
            this.gameObject.transform.localScale = new Vector3(x, 1, z);
            this.gameObject.GetComponent<MeshRenderer>().material.shader = Shader.Find("Masked/Mask");
            this.gameObject.GetComponent<MeshRenderer>().material.renderQueue = 3020;
        }
    }
}
