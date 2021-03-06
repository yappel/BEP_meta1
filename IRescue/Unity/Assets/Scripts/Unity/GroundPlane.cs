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
        private bool debug = false;

        /// <summary>
        /// Initializes the ground plane
        /// </summary>
        /// <param name="fieldSize">aspects of the game field</param>
        public void Init(IRescue.Core.DataTypes.FieldSize fieldSize)
        {
            this.gameObject.name = "GroundPlane";
            this.gameObject.transform.position = new Vector3((fieldSize.Xmax + fieldSize.Xmin) / 2, 0, (fieldSize.Zmax + fieldSize.Zmin) / 2);
            this.gameObject.transform.localScale = new Vector3((fieldSize.Xmax - fieldSize.Xmin) / 10f, 1, (fieldSize.Zmax - fieldSize.Zmin) / 10f);
            this.gameObject.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/grid");
            this.gameObject.GetComponent<MeshRenderer>().material.mainTextureScale = new Vector2(fieldSize.Xmax - fieldSize.Xmin, fieldSize.Zmax - fieldSize.Zmin);
            if (!this.debug)
            {
                this.gameObject.GetComponent<MeshRenderer>().enabled = false;
            }
        }

        /// <summary>
        /// Called on every tick. Press d to enable or disable the grid.
        /// </summary>
        public void Update()
        {
            if (Input.GetKeyDown("d"))
            {
                this.debug = !this.debug;
            }
            
            this.gameObject.GetComponent<MeshRenderer>().enabled = this.debug;
        }
    }
}
