// <copyright file="WorldBox.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Assets.Scripts.Unity
{
    using UnityEngine;
    using IRescue.UserLocalisation;
    using IRescue.Core.DataTypes;
    using IRescue.Core.Utils;

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

        public void Init(AbstractUserLocalizer localizer, FieldSize fieldSize)
        {
            this.localizer = localizer;
            this.fieldSize = fieldSize;
        }

        // Use this for initialization
        void Start()
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.parent = this.transform;
            cube.transform.localPosition = new UnityEngine.Vector3(1.5f, 1.5f, 3);
            cube.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }

        // Update is called once per frame
        void Update()
        {
            Pose pose = this.localizer.CalculatePose(StopwatchSingleton.Time);
            UnityEngine.Vector3 rot = new UnityEngine.Vector3(pose.Orientation.X, pose.Orientation.Y, pose.Orientation.Z);
            UnityEngine.Vector3 pos = new UnityEngine.Vector3(pose.Position.X, pose.Position.Y, pose.Position.Z);

            Debug.Log("Pos: " + pos.ToString());
            Debug.Log("Rot: " + rot.ToString());
            
            rot = new UnityEngine.Vector3(0,0,0);

            Quaternion rotation = Quaternion.Inverse(Quaternion.Euler(rot));
            this.transform.position = -1 * (rotation * pos);
            this.transform.rotation = rotation;
        }
    }
}
