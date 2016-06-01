// <copyright file="GroundPlane.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Assets.Scripts.Unity
{
    using UnityEngine;

    /// <summary>
    /// Ground plane of the game.
    /// </summary>
    public class GroundPlane : MonoBehaviour
    {
        /// <summary>
        /// Initializes the ground plane
        /// </summary>
        /// <param name="x">width of the ground plane</param>
        /// <param name="z">depth of the ground plane</param>
        public void Init(float x, float z)
        {
            this.gameObject.name = "GroundPlane";
            this.gameObject.transform.position = new Vector3(0, -1.6f, 0);
            // TODO change this, but should be the same for all 3 values else the scaling gets weird.
            this.gameObject.transform.localScale = new Vector3(x, x, x);
            this.gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
