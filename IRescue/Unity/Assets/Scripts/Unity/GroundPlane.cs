﻿// <copyright file="GroundPlane.cs" company="Delft University of Technology">
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
            this.gameObject.transform.position = new Vector3(x, -1.6f, z);
            this.gameObject.transform.localScale = new Vector3(x, 1, z);
            this.gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
