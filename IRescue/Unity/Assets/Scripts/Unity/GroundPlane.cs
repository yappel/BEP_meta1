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
        /// <param name="x">width of the ground plane in meters</param>
        /// <param name="z">depth of the ground plane in meter</param>
        public void Init(float x, float z)
        {
            bool debug = false;
            this.gameObject.name = "GroundPlane";
            this.gameObject.transform.localScale = new Vector3(x / 10f, 1, z / 10f);
            this.gameObject.transform.position = new Vector3(x / 2, 0, z / 2);
            if (debug)
            {
                this.gameObject.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/grid");
                this.gameObject.GetComponent<MeshRenderer>().material.mainTextureScale = new Vector2(x, z);
            }
            else
            {
                this.gameObject.GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }
}
