﻿// <copyright file="ImuSensorController.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Assets.Scripts.Unity.SensorControllers
{
    using IRescue.Core.Distributions;
    using IRescue.UserLocalisation.Sensors;
    using IRescue.UserLocalisation.Sensors.IMU;
    using Meta;
    using Vector3 = IRescue.Core.DataTypes.Vector3;

    /// <summary>
    ///   This class keeps track of visible markers and the probable user location based on that.
    /// </summary>
    public class ImuSensorController : AbstractSensorController
    {
        /// <summary>
        ///   Keeps track of the user location and calculates the new one on update.
        /// </summary>
        private IMUSource imuSource;

        private const int IMUBuffersize = 60;

        /// <summary>
        /// TODO fix distributions
        /// </summary>
        private Normal imoOriDist = new Normal(1);

        /// <summary>
        /// TODO fix distributions
        /// </summary>
        private Normal imuPosDist = new Normal(0.1);


        /// <summary>Adds the source to the source controller.</summary>
        public void Init()
        {
            this.imuSource = new IMUSource(this.imuPosDist, this.imoOriDist, IMUBuffersize);
        }

        /// <summary>
        ///   Return the Orientation source.
        /// </summary>
        /// <returns>The IOrientationSource</returns>
        public override IOrientationSource GetOrientationSource()
        {
            return this.imuSource;
        }

        /// <summary>
        ///   Method called on every frame.
        /// </summary>
        public void Update()
        {
            if (this.ImuInitialized())
            {
                UnityEngine.Vector3 unity_acc = IMULocalizer.Instance.accelerometerValues;
                UnityEngine.Vector3 unity_ori = IMULocalizer.Instance.localizerOrientation;
                Vector3 acc = new Vector3(unity_acc.x * (float)(9.81 / 8192), unity_acc.y * (float)(9.81 / 8192), unity_acc.z * (float)(9.81 / 8192));
                Vector3 ori = new Vector3(unity_ori.x, unity_ori.y, unity_ori.z);
                this.imuSource.AddMeasurements(IRescue.Core.Utils.StopwatchSingleton.Time, acc, ori);
            }
        }

        /// <summary>
        ///   Return the acceleration source.
        /// </summary>
        /// <returns>The IAccelerationSource</returns>
        public override IAccelerationSource GetAccelerationSource()
        {
            return this.imuSource;
        }

        /// <summary>
        ///   Return the Displacement source.
        /// </summary>
        /// <returns>The IDisplacementSource</returns>
        public override IDisplacementSource GetDisplacementSource()
        {
            return this.imuSource;
        }

        /// <summary>
        ///   Return the velocity source.
        /// </summary>
        /// <returns>the IVelocitySource</returns>
        public override IVelocitySource GetVelocitySource()
        {
            return this.imuSource;
        }
    }
}