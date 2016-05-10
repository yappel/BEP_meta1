// <copyright file="IMUSource.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace IRescue.UserLocalisation.Sensors.IMU
{
    using System;
    using System.Collections.Generic;
    using Core.DataTypes;

    /// <summary>
    /// The IMU Source provides data from sensors in an IMU and computes values which can be derived from this.
    /// Implements <see cref="IAccelerationSource"/>, <see cref="IDisplacementSource"/> and <see cref="IOrientationSource"/> 
    /// to provide data on acceleration, displacement, orientation and velocity.
    /// </summary>
    public class IMUSource : IAccelerationSource, IDisplacementSource, IOrientationSource
    {
        /// <summary>
        /// Pointer to the last added measurement.
        /// </summary>
        private int pointer = -1;

        /// <summary>
        /// Size of the currently stored measurements.
        /// </summary>
        private int currentSize = 0;

        /// <summary>
        /// The size of the buffer in which data can be stored.
        /// </summary>
        private int measurementBufferSize;

        /// <summary>
        /// Array storing the acceleration measurements with size of <see cref="measurementBufferSize"/>.
        /// </summary>
        private Vector3[] accelerations;

        /// <summary>
        /// The standard deviation belonging to the measurements of the acceleration.
        /// </summary>
        private float accelerationStd;

        /// <summary>
        /// Array storing the orientation measurements with a size of <see cref="measurementBufferSize"/>.
        /// </summary>
        private Vector3[] orientations;

        /// <summary>
        /// The standard deviation belonging to the measurements of the orientation.
        /// </summary>
        private float orientationStd;

        /// <summary>
        /// The time stamps at which the measurements are taken.
        /// </summary>
        private long[] timeStamps;

        /// <summary>
        /// Initializes a new instance of the <see cref="IMUSource"/> class. Measurements can be added to the source and the last 
        /// number of measurements specified by <see cref="measurementBufferSize"/> are stored. Acceleration and orientation both 
        /// have an accompanying standard deviation belonging to the sensor: <see cref="accelerationStd"/> and <see cref="orientationStd"/>.
        /// </summary>
        /// <param name="accelerationStd">The standard deviation of the acceleration sensor of the IMU.</param>
        /// <param name="orientationStd">The standard deviation of the orientation sensor of the IMU.</param>
        /// <param name="measurementBufferSize">The number of measurements to store in a buffer.</param>
        public IMUSource(float accelerationStd, float orientationStd, int measurementBufferSize)
        {
            this.accelerations = new Vector3[measurementBufferSize];
            this.orientations = new Vector3[measurementBufferSize];
            this.timeStamps = new long[measurementBufferSize];
            this.measurementBufferSize = measurementBufferSize;
            this.accelerationStd = accelerationStd;
            this.orientationStd = orientationStd;
        }

        /// <summary>
        /// Adds an acceleration and orientation measurement from the specified time stamp to the buffered measurements. 
        /// When the buffer, <see cref="measurementBufferSize"/>, is full the oldest entry is overwritten.
        /// </summary>
        /// <param name="timeStamp">The time stamp at which the measurements were taken.</param>
        /// <param name="acceleration">The acceleration measurement.</param>
        /// <param name="orientation">The orientation measurement.</param>
        public void AddMeasurements(long timeStamp, Vector3 acceleration, Vector3 orientation)
        {
            this.pointer = (this.pointer + 1) % this.measurementBufferSize;
            this.timeStamps[this.pointer] = timeStamp;
            this.accelerations[this.pointer] = acceleration;
            this.orientations[this.pointer] = orientation;
            if (this.currentSize < this.measurementBufferSize)
            {
                this.currentSize++;
            }
        }

        /// <summary>
        /// Returns the acceleration measurement and standard deviation from the specified time stamp.
        /// </summary>
        /// <param name="timeStamp">The time stamp to take the measurement from.</param>
        /// <returns>Acceleration vector and the standard deviation of the measurement, null when there is no measurement.</returns>
        public Measurement<Vector3> GetAcceleration(long timeStamp)
        {
            if (this.currentSize > 0)
            {
                for (int i = 0; i < this.currentSize; i++)
                {
                    if (this.timeStamps[i] == timeStamp)
                    {
                        return new Measurement<Vector3>(this.accelerations[i], this.accelerationStd, timeStamp);
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Get the acceleration measurements from the specified starting time stamp up to and including the ending time stamp.
        /// </summary>
        /// <param name="startTimeStamp">The time stamp to include measurements from.</param>
        /// <param name="endTimeStamp">The time stamps to include measurements up to.</param>
        /// <returns>List of all acceleration measurements and their time stamps and standard deviation.</returns>
        public List<Measurement<Vector3>> GetAccelerations(long startTimeStamp, long endTimeStamp)
        {
            List<Measurement<Vector3>> res = new List<Measurement<Vector3>>();
            int first = this.GetOldestMeasurementIndex();
            for (int i = 0; i < this.currentSize; i++)
            {
                int index = (first + i) % this.measurementBufferSize;
                if (this.timeStamps[index] >= startTimeStamp && this.timeStamps[index] <= endTimeStamp)
                {
                    res.Add(new Measurement<Vector3>(this.accelerations[index], this.accelerationStd, this.timeStamps[index]));
                }
            }

            return res;
        }

        /// <summary>
        /// Get all the the acceleration measurements currently in the buffer with their time stamp and the standard deviation.
        /// </summary>
        /// <returns>A list with all the acceleration measurements.</returns>
        public List<Measurement<Vector3>> GetAllAccelerations()
        {
            List<Measurement<Vector3>> res = new List<Measurement<Vector3>>(this.currentSize);
            int first = this.GetOldestMeasurementIndex();
            for (int i = 0; i < this.currentSize; i++)
            {
                int index = (first + i) % this.measurementBufferSize;
                res.Add(new Measurement<Vector3>(this.accelerations[index], this.accelerationStd, this.timeStamps[index]));
            }

            return res;
        }

        /// <summary>
        /// Get all the the orientation measurements currently in the buffer with their time stamp and the standard deviation.
        /// </summary>
        /// <returns>A list with all the orientation measurements.</returns>
        public List<Measurement<Vector3>> GetAllOrientations()
        {
            List<Measurement<Vector3>> res = new List<Measurement<Vector3>>(this.currentSize);
            int first = this.GetOldestMeasurementIndex();
            for (int i = 0; i < this.currentSize; i++)
            {
                int index = (first + i) % this.measurementBufferSize;
                res.Add(new Measurement<Vector3>(this.orientations[index], this.orientationStd, this.timeStamps[index]));
            }

            return res;
        }

        public Measurement<Vector3> GetDisplacement(long startTimeStamp, long endTimeStamp)
        {
            List<Measurement<Vector3>> acc = this.GetAccelerations(startTimeStamp, endTimeStamp);
            if (acc.Count > 2)
            {
                List<Measurement<Vector3>> ori = this.GetOrientations(startTimeStamp, endTimeStamp);
                List<Measurement<Vector3>> worldAcc = new List<Measurement<Vector3>>(acc.Count);
                for (int i = 0; i < acc.Count; i++)
                {
                    worldAcc.Add(new Measurement<Vector3>(IRescue.Core.Utils.VectorMath.RotateVector(acc[i].Data, -1 * ori[i].Data.X, -1 * ori[i].Data.Y, -1 * ori[i].Data.Z), acc[i].Std, acc[i].TimeStamp));
                }
                Vector3 displacement = new Vector3(0, 0, 0);
                Vector3 vel1 = new Vector3(0, 0, 0);
                Vector3 vel2 = new Vector3(0, 0, 0);
                // TODO fix std with integration
                float std = acc[0].Std;
                long t1;
                long t2;
                for (int i = 1; i < (worldAcc.Count - 1); i++)
                {
                    t1 = worldAcc[i - 1].TimeStamp + ((worldAcc[i].TimeStamp - worldAcc[i - 1].TimeStamp) / 2);
                    t2 = worldAcc[i].TimeStamp + ((worldAcc[i + 1].TimeStamp - worldAcc[i].TimeStamp) / 2);
                    vel1.X = (worldAcc[i - 1].Data.X + worldAcc[i].Data.X) / 2 * (float)(worldAcc[i].TimeStamp - worldAcc[i-1].TimeStamp) * 1000;
                    vel2.X = (worldAcc[i].Data.X + worldAcc[i+1].Data.X) / 2 * (float)(worldAcc[i+1].TimeStamp - worldAcc[i].TimeStamp) * 1000;
                    vel1.Y = (worldAcc[i - 1].Data.Y + worldAcc[i].Data.Y) / 2 * (float)(worldAcc[i].TimeStamp - worldAcc[i - 1].TimeStamp) * 1000;
                    vel2.Y = (worldAcc[i].Data.Y + worldAcc[i + 1].Data.Y) / 2 * (float)(worldAcc[i + 1].TimeStamp - worldAcc[i].TimeStamp) * 1000;
                    vel1.Z = (worldAcc[i - 1].Data.Z + worldAcc[i].Data.Z) / 2 * (float)(worldAcc[i].TimeStamp - worldAcc[i - 1].TimeStamp) * 1000;
                    vel2.Z = (worldAcc[i].Data.Z + worldAcc[i + 1].Data.Z) / 2 * (float)(worldAcc[i + 1].TimeStamp - worldAcc[i].TimeStamp) * 1000;

                    displacement.X += (vel1.X + vel2.X) / 2 * (float)(t2 - t1) * 1000;
                    displacement.Y += (vel1.Y + vel2.Y) / 2 * (float)(t2 - t1) * 1000;
                    displacement.Z += (vel1.Z + vel2.Z) / 2 * (float)(t2 - t1) * 1000;
                }
                return new Measurement<Vector3>(displacement, std, (startTimeStamp + endTimeStamp) / 2);
            }
            return null;
        }

        /// <summary>
        /// Get the last added acceleration measurement and its time stamp.
        /// </summary>
        /// <returns>The acceleration vector with time stamp and standard deviation or null when there is no measurement.</returns>
        public Measurement<Vector3> GetLastAcceleration()
        {
            if (this.currentSize > 0)
            {
                return new Measurement<Vector3>(this.accelerations[this.pointer], this.accelerationStd, this.timeStamps[this.pointer]);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get the last added orientation measurement and its time stamp.
        /// </summary>
        /// <returns>The orientation vector with time stamp and standard deviation or null when there is no measurement.</returns>
        public Measurement<Vector3> GetLastOrientation()
        {
            if (this.currentSize > 0)
            {
                return new Measurement<Vector3>(this.orientations[this.pointer], this.orientationStd, this.timeStamps[this.pointer]);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get the orientation measurement and standard deviation from the specified time stamp.
        /// </summary>
        /// <param name="timeStamp">The time stamp to get the measurement from.</param>
        /// <returns>The orientation measurement at the time stamp with standard deviation.</returns>
        public Measurement<Vector3> GetOrientation(long timeStamp)
        {
            if (this.currentSize > 0)
            {
                for (int i = 0; i < this.currentSize; i++)
                {
                    if (this.timeStamps[i] == timeStamp)
                    {
                        return new Measurement<Vector3>(this.orientations[i], this.orientationStd, timeStamp);
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Get the orientation measurements from the specified starting time stamp up to and including the ending time stamp.
        /// </summary>
        /// <param name="startTimeStamp">The time stamp to include measurements from.</param>
        /// <param name="endTimeStamp">The time stamps to include measurements up to.</param>
        /// <returns>List of all orientation measurements and their time stamps and standard deviation.</returns>
        public List<Measurement<Vector3>> GetOrientations(long startTimeStamp, long endTimeStamp)
        {
            List<Measurement<Vector3>> res = new List<Measurement<Vector3>>();
            int first = this.GetOldestMeasurementIndex();
            for (int i = 0; i < this.currentSize; i++)
            {
                int index = (first + i) % this.measurementBufferSize;
                if (this.timeStamps[index] >= startTimeStamp && this.timeStamps[index] <= endTimeStamp)
                {
                    res.Add(new Measurement<Vector3>(this.accelerations[index], this.accelerationStd, this.timeStamps[index]));
                }
            }

            return res;
        }

        /// <summary>
        /// Get the index of the oldest added measurement in the buffer.
        /// </summary>
        /// <returns>The index of the oldest measurement.</returns>
        private int GetOldestMeasurementIndex()
        {
            return (this.pointer - this.currentSize + 1) % this.measurementBufferSize;
        }
    }
}
