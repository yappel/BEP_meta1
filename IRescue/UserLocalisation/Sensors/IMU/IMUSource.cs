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
                for (int i = 0; i < currentSize; i++)
                {
                    if (this.timeStamps[i] == timeStamp)
                    {
                        return new Measurement<Vector3>(this.accelerations[i], this.accelerationStd, timeStamp);
                    }
                }
            }
            return null;
        }

        public List<Measurement<Vector3>> GetAccelerations(long startTimeStamp, long endTimeStamp)
        {
            List<Measurement<Vector3>> res = new List<Measurement<Vector3>>();
            if (this.currentSize > 0)
            {
                // Oldest element in the measurement buffer
                int first = (this.pointer - this.currentSize + 1) % measurementBufferSize;
                for(int i = 0; i < currentSize; i++)
                {
                    int index = (first + i) % measurementBufferSize;
                    if (this.timeStamps[index] >= startTimeStamp && this.timeStamps[index] <= endTimeStamp)
                    {
                        res.Add(new Measurement<Vector3>(this.accelerations[index], accelerationStd, this.timeStamps[index]));
                    }
                }
                return res;
            }
            else
            {
                return res;
            }
        }

        public List<Measurement<Vector3>> GetAllAccelerations()
        {
            throw new NotImplementedException();
        }

        public List<Measurement<Vector3>> GetAllOrientations()
        {
            throw new NotImplementedException();
        }

        public Measurement<Vector3> GetDisplacement(long startTimeStamp, long endTimeStamp)
        {
            throw new NotImplementedException();
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
        /// <returns></returns>
        public Measurement<Vector3> GetOrientation(long timeStamp)
        {
            if (this.currentSize > 0)
            {
                for (int i = 0; i < currentSize; i++)
                {
                    if (this.timeStamps[i] == timeStamp)
                    {
                        return new Measurement<Vector3>(this.orientations[i], this.orientationStd, timeStamp);
                    }
                }
            }
            return null;
        }

        public List<Measurement<Vector3>> GetOrientations(long startTimeStamp, long endTimeStamp)
        {
            throw new NotImplementedException();
        }
    }
}
