// <copyright file="WorldBox.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Assets.Scripts.Unity
{
    using IRescue.Core.DataTypes;
    using IRescue.Core.Utils;
    using IRescue.UserLocalisation;
    using UnityEngine;

    /// <summary>
    /// This component initializes the other components for the world box game object
    /// </summary>
    public class WorldBox : MonoBehaviour
    {
        /// <summary>
        /// Reference the user localizer class used in the application.
        /// </summary>
        private AbstractUserLocalizer localizer;

        /// <summary>
        /// The field size used in the application.
        /// </summary>
        private FieldSize fieldSize;

        /// <summary>
        /// Initializes the world box, setting the localizer and the field size
        /// </summary>
        /// <param name="localizer">The used filter method</param>
        /// <param name="fieldSize">The size of the game field</param>
        public void Init(AbstractUserLocalizer localizer, FieldSize fieldSize)
        {
            this.localizer = localizer;
            this.fieldSize = fieldSize;
            this.InitPlanes();

            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.parent = this.transform;
            cube.transform.localPosition = new UnityEngine.Vector3(1.5f, 1.5f, 3);
            cube.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }

        /// <summary>
        /// Set the rotation and position of the world box based on the user
        /// </summary>
        public void LateUpdate()
        {
            Pose pose = this.localizer.CalculatePose(StopwatchSingleton.Time);
            UnityEngine.Vector3 rot = new UnityEngine.Vector3(pose.Orientation.X, pose.Orientation.Y, pose.Orientation.Z);
            UnityEngine.Vector3 pos = new UnityEngine.Vector3(pose.Position.X, pose.Position.Y, pose.Position.Z);

            Debug.Log("Pos: " + pos.ToString());
            Debug.Log("Rot: " + rot.ToString());
            
            rot = new UnityEngine.Vector3(0, 0, 0);

            Quaternion rotation = Quaternion.Inverse(Quaternion.Euler(rot));
            this.transform.position = -1 * (rotation * pos);
            this.transform.rotation = rotation;
        }

        /// <summary>
        /// Create the ground and water plane
        /// </summary>
        private void InitPlanes()
        {
            GameObject groundPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
            groundPlane.AddComponent<GroundPlane>().Init(this.fieldSize.Xmax - this.fieldSize.Xmin, this.fieldSize.Zmax - this.fieldSize.Zmin);
            groundPlane.transform.parent = this.transform;
            GameObject waterPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
            waterPlane.AddComponent<WaterLevelController>().Init(this.fieldSize.Xmax - this.fieldSize.Xmin, this.fieldSize.Zmax - this.fieldSize.Zmin);
            waterPlane.transform.parent = this.transform;
        }
    }
}
