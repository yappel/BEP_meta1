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
        }

        // Update is called once per frame
        void Update()
        {
            Pose pose = localizer.CalculatePose(StopwatchSingleton.Time);
            Quaternion rotation = Quaternion.Euler(pose.Orientation.X, pose.Orientation.Y, pose.Orientation.Z);
            Matrix4x4 m = Matrix4x4.identity;
            UnityEngine.Vector3 translation = new UnityEngine.Vector3(pose.Position.X, pose.Position.Y, pose.Position.Z);
            UnityEngine.Vector3 scale = new UnityEngine.Vector3(1, 1, 1);
            m.SetTRS(translation, rotation, scale);
        }
    }
}
