﻿// <copyright file="IMUSource.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>
namespace IRescue.UserLocalisation.Sensors.IMU
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using IRescue.Core.DataTypes;
    using IRescue.Core.Distributions;
    using IRescue.Core.Utils;
    using IRescue.UserLocalisation.Feedback;

    /// <summary>
    ///     The IMU Source provides data from sensors in an IMU and computes _values which can be derived from this.
    ///     Implements <see cref="IAccelerationSource" />, <see cref="IDisplacementSource" />, <see cref="IVelocitySource" />
    ///     and <see cref="IOrientationSource" /> to provide data on acceleration, displacement, orientation and velocity.
    /// </summary>
    public class IMUSource : IAccelerationSource, IDisplacementSource, IOrientationSource, IVelocitySource, IVelocityFeedbackReceiver
    {
        /// <summary>
        ///     The type of probability distribution belonging to the measurements of the acceleration.
        /// </summary>
        private Normal accDistType;

        /// <summary>
        ///     Array storing the acceleration measurements with size of <see cref="measurementBufferSize" />.
        /// </summary>
        private Vector3[] accelerations;

        /// <summary>
        ///     The gravity acceleration vector of the environment, defaults to Earth's gravity.
        /// </summary>
        private Vector3 gravity = new Vector3(new[] { 0f, -9.81f, 0 });

        /// <summary>
        ///     The size of the buffer in which data can be stored, default value 10;
        /// </summary>
        private int measurementBufferSize = 10;

        /// <summary>
        ///     Pointer to the last added acceleration, time stamp and orientation measurement.
        /// </summary>
        private int measurementPointer = -1;

        /// <summary>
        ///     Size of the currently stored measurements.
        /// </summary>
        private int measurementSize;

        /// <summary>
        ///     The type of probability distribution belonging to the measurements of the orientation.
        /// </summary>
        private Normal oriDistType;

        /// <summary>
        ///     Array storing the orientation measurements with a size of <see cref="measurementBufferSize" />.
        /// </summary>
        private Vector3[] orientations;

        /// <summary>
        ///     The time stamps at which the measurements are taken.
        /// </summary>
        private long[] timeStamps;

        /// <summary>
        ///     Array storing all derived velocity measurements with size of <see cref="measurementBufferSize" />.
        /// </summary>
        private Vector3[] velocity;

        /// <summary>
        ///     Pointer to the last added velocity measurement.
        /// </summary>
        private int velocityPointer;

        /// <summary>
        ///     Size of the currently stored velocities.
        /// </summary>
        private int velocitySize;

        /// <summary>
        ///     Array storing the standard deviations of all derived velocity measurements with size of <see cref="measurementBufferSize"/>.
        /// </summary>
        private float[] velocityStd;

        /// <summary>
        ///     Initializes a new instance of the <see cref="IMUSource" /> class. Measurements can be added to the source and the
        ///     last
        ///     number of measurements specified by <see cref="measurementBufferSize" /> are stored. Acceleration and orientation
        ///     both
        ///     have an accompanying standard deviation belonging to the sensor:.
        ///     Initializes default Earth gravity and starting velocity of 0.
        /// </summary>
        /// <param name="accDistType">The type of probability distribution belonging to the measurements of the acceleration.</param>
        /// <param name="oriDistType">The type of probability distribution belonging to the measurements of the orientation.</param>
        /// <param name="measurementBufferSize">The number of measurements to store in a buffer.</param>
        public IMUSource(Normal accDistType, Normal oriDistType, int measurementBufferSize)
        {
            if (measurementBufferSize > 0)
            {
                this.measurementBufferSize = measurementBufferSize;
            }

            //// Create measurement buffers
            this.accelerations = new Vector3[this.measurementBufferSize];
            this.orientations = new Vector3[this.measurementBufferSize];
            this.velocity = new Vector3[this.measurementBufferSize];
            this.timeStamps = new long[this.measurementBufferSize];

            //// Set parameters
            this.accDistType = accDistType;
            this.oriDistType = oriDistType;

            //// No starting velocity specified, init on 0
            this.velocity[0] = new Vector3(0, 0, 0);
            this.velocitySize = 1;
            this.velocityPointer = 0;
            this.velocityStd = new float[this.measurementBufferSize];
            this.velocityStd[0] = 0;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="IMUSource" /> class. Measurements can be added to the source and the
        ///     last
        ///     number of measurements specified by <see cref="measurementBufferSize" /> are stored.
        ///     Initializes starting velocity of 0.
        /// </summary>
        /// <param name="accDistType">The type of probability distribution belonging to the measurements of the acceleration.</param>
        /// <param name="oriDistType">The type of probability distribution belonging to the measurements of the orientation.</param>
        /// <param name="measurementBufferSize">The number of measurements to store in a buffer.</param>
        /// <param name="gravity">The gravity acceleration vector to use in the world.</param>
        public IMUSource(Normal accDistType, Normal oriDistType, int measurementBufferSize, Vector3 gravity)
            : this(accDistType, oriDistType, measurementBufferSize)
        {
            this.gravity = gravity;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="IMUSource" /> class. Measurements can be added to the source and the
        ///     last
        ///     number of measurements specified by <see cref="measurementBufferSize" /> are stored.
        /// </summary>
        /// <param name="accDistType">The type of probability distribution belonging to the measurements of the acceleration.</param>
        /// <param name="oriDistType">The type of probability distribution belonging to the measurements of the orientation.</param>
        /// <param name="measurementBufferSize">The number of measurements to store in a buffer.</param>
        /// <param name="gravity">The gravity acceleration vector to use in the world.</param>
        /// <param name="startVelocity">The starting velocity of the IMU.</param>
        public IMUSource(Normal accDistType, Normal oriDistType, int measurementBufferSize, Vector3 gravity, Vector3 startVelocity)
            : this(accDistType, oriDistType, measurementBufferSize, gravity)
        {
            this.velocity[0] = startVelocity;
            this.velocitySize = 1;
            this.velocityPointer = 0;
        }

        /// <summary>
        ///     Adds an acceleration and orientation measurement from the specified time stamp to the buffered measurements.
        ///     When the buffer, <see cref="measurementBufferSize" />, is full the oldest entry is overwritten.
        /// </summary>
        /// <param name="timeStamp">The time stamp at which the measurements were taken.</param>
        /// <param name="acceleration">The acceleration measurement.</param>
        /// <param name="orientation">The orientation measurement. The orientation should be </param>
        public void AddMeasurements(long timeStamp, Vector3 acceleration, Vector3 orientation)
        {
            this.measurementPointer = this.Mod(this.measurementPointer + 1, this.measurementBufferSize);
            this.timeStamps[this.measurementPointer] = timeStamp;
            Vector3 acc = new Vector3(0, 0, 0);
            VectorMath.RotateVector(acceleration, orientation.X, orientation.Y, orientation.Z, acc);
            acc.Subtract(this.gravity, acc);
            this.accelerations[this.measurementPointer] = acc;
            this.orientations[this.measurementPointer] = orientation;
            if (this.measurementSize < this.measurementBufferSize)
            {
                this.measurementSize++;
            }

            //// If there are more than 2 measurements, calculate velocity and velocity std.
            if (this.measurementSize > 1)
            {
                this.velocityPointer = this.Mod(this.velocityPointer + 1, this.measurementBufferSize);
                Vector3 vel = this.CalculateDeltaV(this.accelerations[this.Mod(this.measurementPointer - 1, this.measurementBufferSize)], this.accelerations[this.measurementPointer], this.timeStamps[this.Mod(this.measurementPointer - 1, this.measurementBufferSize)], this.timeStamps[this.measurementPointer]);
                vel.Add(this.velocity[this.Mod(this.velocityPointer - 1, this.measurementBufferSize)], vel);
                this.velocity[this.velocityPointer] = vel;
                this.velocityStd[this.velocityPointer] =
                    (float)Math.Sqrt(
                        Math.Pow(this.velocityStd[this.Mod(this.velocityPointer - 1, this.measurementBufferSize)], 2) +
                        Math.Pow(this.accDistType.Stddev, 2));
                if (this.velocitySize < this.measurementBufferSize)
                {
                    this.velocitySize++;
                }
            }
        }

        /// <summary>
        ///     Returns the acceleration measurement and standard deviation from the specified time stamp.
        /// </summary>
        /// <param name="timeStamp">The time stamp to take the measurement from.</param>
        /// <returns>Acceleration vector and the standard deviation of the measurement, null when there is no measurement.</returns>
        public Measurement<Vector3> GetAcceleration(long timeStamp)
        {
            if (this.measurementSize > 0)
            {
                for (int i = 0; i < this.measurementSize; i++)
                {
                    if (this.timeStamps[i] == timeStamp)
                    {
                        return new Measurement<Vector3>(this.accelerations[i], timeStamp, this.accDistType);
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Gets all the measurements closets to a given time stamp and within a given range of that time stamp.
        /// </summary>
        /// <param name="timeStamp">The time stamp in milliseconds of the desired measurements.</param>
        /// <param name="range">The amount of milliseconds that the actual returned may differ from the desired time stamp.</param>
        /// <returns>A list of all measurements that have the the smallest difference in time stamp.</returns>
        public List<Measurement<Vector3>> GetAccelerationClosestTo(long timeStamp, long range)
        {
            List<Measurement<Vector3>> res = new List<Measurement<Vector3>>();
            long mindiff = long.MaxValue;
            for (int i = 0; i < this.measurementSize; i++)
            {
                Measurement<Vector3> measurement = new Measurement<Vector3>(this.accelerations[i], this.timeStamps[i], this.accDistType);
                long diff = Math.Abs(measurement.TimeStamp - timeStamp);
                if (diff == mindiff)
                {
                    res.Add(measurement);
                }
                else if (diff < mindiff)
                {
                    res.Clear();
                    mindiff = diff;
                    res.Add(measurement);
                }
            }

            return res;
        }

        /// <summary>
        ///     Get the acceleration measurements from the specified starting time stamp up to and including the ending time stamp.
        /// </summary>
        /// <param name="startTimeStamp">The time stamp to include measurements from.</param>
        /// <param name="endTimeStamp">The time stamps to include measurements up to.</param>
        /// <returns>List of all acceleration measurements and their time stamps and standard deviation.</returns>
        public List<Measurement<Vector3>> GetAccelerations(long startTimeStamp, long endTimeStamp)
        {
            List<Measurement<Vector3>> res = new List<Measurement<Vector3>>();
            int first = this.GetOldestMeasurementIndex(this.measurementPointer, this.measurementSize);
            for (int i = 0; i < this.measurementSize; i++)
            {
                int index = this.Mod(first + i, this.measurementBufferSize);
                if ((this.timeStamps[index] >= startTimeStamp) && (this.timeStamps[index] <= endTimeStamp))
                {
                    res.Add(new Measurement<Vector3>(this.accelerations[index], this.timeStamps[index], this.accDistType));
                }
            }

            return res;
        }

        /// <summary>
        ///     Get all the the acceleration measurements currently in the buffer with their time stamp and the standard deviation.
        /// </summary>
        /// <returns>A list with all the acceleration measurements.</returns>
        public List<Measurement<Vector3>> GetAllAccelerations()
        {
            List<Measurement<Vector3>> res = new List<Measurement<Vector3>>(this.measurementSize);
            int first = this.GetOldestMeasurementIndex(this.measurementPointer, this.measurementSize);
            for (int i = 0; i < this.measurementSize; i++)
            {
                int index = this.Mod(first + i, this.measurementBufferSize);
                res.Add(new Measurement<Vector3>(this.accelerations[index], this.timeStamps[index], this.accDistType));
            }

            return res;
        }

        /// <summary>
        ///     Get all the the orientation measurements currently in the buffer with their time stamp and the standard deviation.
        /// </summary>
        /// <returns>A list with all the orientation measurements.</returns>
        public List<Measurement<Vector3>> GetAllOrientations()
        {
            List<Measurement<Vector3>> res = new List<Measurement<Vector3>>(this.measurementSize);
            int first = this.GetOldestMeasurementIndex(this.measurementPointer, this.measurementSize);
            for (int i = 0; i < this.measurementSize; i++)
            {
                int index = this.Mod(first + i, this.measurementBufferSize);
                res.Add(new Measurement<Vector3>(this.orientations[index], this.timeStamps[index], this.oriDistType));
            }

            return res;
        }

        /// <summary>
        ///     Get all the velocity measurements stored in the source.
        /// </summary>
        /// <returns>List of all velocity measurements with time stamps and standard deviations.</returns>
        public List<Measurement<Vector3>> GetAllVelocities()
        {
            List<Measurement<Vector3>> res = new List<Measurement<Vector3>>(this.velocitySize);
            int first = this.GetOldestMeasurementIndex(this.velocityPointer, this.velocitySize);
            for (int i = 0; i < this.velocitySize; i++)
            {
                int index = this.Mod(first + i, this.measurementBufferSize);
                res.Add(new Measurement<Vector3>(this.velocity[index], this.timeStamps[index], new Normal(this.velocityStd[index])));
            }

            return res;
        }

        /// <summary>
        ///     Get the displacement over the specified time period.
        ///     Performs double integration of acceleration: inaccurate errors accumulate over time.
        /// </summary>
        /// <param name="startTimeStamp">The start time stamp to calculate displacement from.</param>
        /// <param name="endTimeStamp">The end time stamp to calculate displacement to.</param>
        /// <returns>Displacement with standard deviation and time stamp of last velocity measurement.</returns>
        public Measurement<Vector3> GetDisplacement(long startTimeStamp, long endTimeStamp)
        {
            List<Measurement<Vector3>> vel = this.GetVelocities(startTimeStamp, endTimeStamp);
            Vector3 displacement = new Vector3(0, 0, 0);
            Vector3 temp = new Vector3(0, 0, 0);
            float std = 0;
            if (vel.Count > 1)
            {
                for (int i = 0; i < vel.Count - 1; i++)
                {
                    Vector3 v1 = vel[i].Data;
                    Vector3 v2 = vel[i + 1].Data;
                    long t1 = vel[i].TimeStamp;
                    long t2 = vel[i + 1].TimeStamp;
                    v1.Add(v2, temp);
                    temp.Divide(2, temp);
                    temp.Multiply(this.MilliSecondsToSeconds(t2 - t1), temp);
                    displacement.Add(temp, displacement);
                    std += (float)Math.Pow(((Normal)vel[i].DistributionType).Stddev, 2);
                }

                std = (float)Math.Sqrt(std + Math.Pow(((Normal)vel[vel.Count - 1].DistributionType).Stddev, 2));
                return new Measurement<Vector3>(displacement, vel[vel.Count - 1].TimeStamp, new Normal(std));
            }

            return new Measurement<Vector3>(displacement, endTimeStamp, new Normal(double.MaxValue));
        }

        /// <summary>
        ///     Get the last added acceleration measurement and its time stamp.
        /// </summary>
        /// <returns>The acceleration vector with time stamp and standard deviation or null when there is no measurement.</returns>
        public Measurement<Vector3> GetLastAcceleration()
        {
            if (this.measurementSize > 0)
            {
                return new Measurement<Vector3>(this.accelerations[this.measurementPointer], this.timeStamps[this.measurementPointer], this.accDistType);
            }

            return null;
        }

        /// <summary>
        ///     Get the last added orientation measurement and its time stamp.
        /// </summary>
        /// <returns>The orientation vector with time stamp and standard deviation or null when there is no measurement.</returns>
        public Measurement<Vector3> GetLastOrientation()
        {
            if (this.measurementSize > 0)
            {
                return new Measurement<Vector3>(this.orientations[this.measurementPointer], this.timeStamps[this.measurementPointer], this.oriDistType);
            }

            return null;
        }

        /// <summary>
        ///     Get the last added velocity measurement.
        /// </summary>
        /// <returns>The velocity measurement with time stamp and standard deviation, null when there is no measurement.</returns>
        public Measurement<Vector3> GetLastVelocity()
        {
            if (this.velocitySize > 0)
            {
                return new Measurement<Vector3>(this.velocity[this.velocityPointer], this.timeStamps[this.velocityPointer], new Normal(this.velocityStd[this.velocityPointer]));
            }

            return null;
        }

        /// <summary>
        ///     Return the buffer size where the measurements can be stored.
        /// </summary>
        /// <returns>Integer of the buffer size.</returns>
        public int GetMeasurementBufferSize()
        {
            return this.measurementBufferSize;
        }

        /// <summary>
        ///     Get the orientation measurement and standard deviation from the specified time stamp.
        /// </summary>
        /// <param name="timeStamp">The time stamp to get the measurement from.</param>
        /// <returns>The orientation measurement at the time stamp with standard deviation.</returns>
        public Measurement<Vector3> GetOrientation(long timeStamp)
        {
            if (this.measurementSize > 0)
            {
                for (int i = 0; i < this.measurementSize; i++)
                {
                    if (this.timeStamps[i] == timeStamp)
                    {
                        return new Measurement<Vector3>(this.orientations[i], timeStamp, this.oriDistType);
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Gets all the measurements closets to a given time stamp and within a given range of that time stamp.
        /// </summary>
        /// <param name="timeStamp">The time stamp in milliseconds of the desired measurements.</param>
        /// <param name="range">The amount of milliseconds that the actual returned may differ from the desired time stamp.</param>
        /// <returns>A list of all measurements that have the the smallest difference in time stamp.</returns>
        public List<Measurement<Vector3>> GetOrientationClosestTo(long timeStamp, long range)
        {
            return this.GetSourceClosestTo(timeStamp, range, this.orientations, Enumerable.Repeat(this.oriDistType, this.orientations.Length).ToArray());
        }

        /// <summary>
        ///     Get the orientation measurements from the specified starting time stamp up to and including the ending time stamp.
        /// </summary>
        /// <param name="startTimeStamp">The time stamp to include measurements from.</param>
        /// <param name="endTimeStamp">The time stamps to include measurements up to.</param>
        /// <returns>List of all orientation measurements and their time stamps and standard deviation.</returns>
        public List<Measurement<Vector3>> GetOrientations(long startTimeStamp, long endTimeStamp)
        {
            List<Measurement<Vector3>> res = new List<Measurement<Vector3>>();
            int first = this.GetOldestMeasurementIndex(this.measurementPointer, this.measurementSize);
            for (int i = 0; i < this.measurementSize; i++)
            {
                int index = this.Mod(first + i, this.measurementBufferSize);
                if ((this.timeStamps[index] >= startTimeStamp) && (this.timeStamps[index] <= endTimeStamp))
                {
                    res.Add(new Measurement<Vector3>(this.orientations[index], this.timeStamps[index], this.oriDistType));
                }
            }

            return res;
        }

        /// <summary>
        ///     Gets all the velocities between the specified time stamps.
        /// </summary>
        /// <param name="startTimeStamp">The time stamp to include measurements from.</param>
        /// <param name="endTimeStamp">The time stamp to include measurements up to.</param>
        /// <returns>List of all the velocities and their time stamps and deviations.</returns>
        public List<Measurement<Vector3>> GetVelocities(long startTimeStamp, long endTimeStamp)
        {
            List<Measurement<Vector3>> res = new List<Measurement<Vector3>>();
            int first = this.GetOldestMeasurementIndex(this.velocityPointer, this.velocitySize);
            for (int i = 0; i < this.velocitySize; i++)
            {
                int index = this.Mod(first + i, this.measurementBufferSize);
                if ((this.timeStamps[index] >= startTimeStamp) && (this.timeStamps[index] <= endTimeStamp))
                {
                    res.Add(new Measurement<Vector3>(this.velocity[index], this.timeStamps[index], new Normal(this.velocityStd[index])));
                }
            }

            return res;
        }

        /// <summary>
        ///     Get the velocity measurement at the specified time stamp.
        /// </summary>
        /// <param name="timeStamp">The time stamp to take the measurement from.</param>
        /// <returns>Velocity measurement with time stamp and standard deviation, null when there is no such measurement.</returns>
        public Measurement<Vector3> GetVelocity(long timeStamp)
        {
            if (this.velocitySize > 0)
            {
                for (int i = 0; i < this.velocitySize; i++)
                {
                    if (this.timeStamps[i] == timeStamp)
                    {
                        return new Measurement<Vector3>(this.velocity[i], timeStamp, new Normal(this.velocityStd[i]));
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Gets all the measurements closets to a given time stamp and within a given range of that time stamp.
        /// </summary>
        /// <param name="timeStamp">The time stamp in milliseconds of the desired measurements.</param>
        /// <param name="range">The amount of milliseconds that the actual returned may differ from the desired time stamp.</param>
        /// <returns>A list of all measurements that have the the smallest difference in time stamp.</returns>
        public List<Measurement<Vector3>> GetVelocityClosestTo(long timeStamp, long range)
        {
            Normal[] dists = new Normal[this.velocityStd.Length];
            for (int i = 0; i < this.velocityStd.Length; i++)
            {
                dists[i] = new Normal(this.velocityStd[i]);
            }

            return this.GetSourceClosestTo(timeStamp, range, this.velocity, dists);
        }

        /// <inheritdoc/>
        public void NotifyVelocityFeedback(FeedbackData<Vector3> data)
        {
            int index = -1;
            for (int i = 0; i < Math.Min(this.measurementBufferSize, this.measurementSize); i++)
            {
                if (this.timeStamps[i] < data.TimeStamp)
                {
                    continue;
                }

                index = data.TimeStamp - this.timeStamps[i - 1] >= this.timeStamps[i] - data.TimeStamp ? i - 1 : i;
                if (this.velocityStd[index] <= data.Stddev)
                {
                    return;
                }

                break;
            }

            if (index < 0)
            {
                return;
            }

            this.velocity[this.Mod(index, this.measurementSize)] = data.Data;
            this.velocityStd[this.Mod(index, this.measurementSize)] = data.Stddev;
            int range = index > this.velocityPointer ? index - this.velocityPointer : (this.velocityPointer + this.measurementBufferSize) - index;

            for (int i = 1; i < range; i++)
            {
                int current = this.Mod(index + i, this.measurementSize);
                int prev = this.Mod((index + i) - 1, this.measurementSize);
                Vector3 dv = this.CalculateDeltaV(this.accelerations[prev], this.accelerations[current], this.timeStamps[prev], this.timeStamps[current]);
                this.velocity[this.Mod(current - 1, this.measurementSize)].Add(dv, this.velocity[current]);
                this.velocityStd[current] = (float)Math.Sqrt(Math.Pow(this.velocityStd[prev], 2) + Math.Pow(this.accDistType.Stddev, 2));
            }
        }

        /// <summary>
        ///     Returns the difference in velocity between two acceleration measurements at two time stamp.
        /// </summary>
        /// <param name="acc1">The first acceleration measurement.</param>
        /// <param name="acc2">The second acceleration measurement.</param>
        /// <param name="t1">The time stamp of the first time stamp.</param>
        /// <param name="t2">The time stamp of the second measurement.</param>
        /// <returns>Vector containing the difference in velocity over the time period.</returns>
        private Vector3 CalculateDeltaV(Vector3 acc1, Vector3 acc2, long t1, long t2)
        {
            Vector3 dv = new Vector3(0, 0, 0);
            acc1.Add(acc2, dv);
            dv.Divide(2, dv);
            dv.Multiply(this.MilliSecondsToSeconds(t2 - t1), dv);
            return dv;
        }

        /// <summary>
        ///     Get the index of the oldest added measurement in the buffer.
        /// </summary>
        /// <param name="pointer">The pointer in the array of the last measurement.</param>
        /// <param name="size">The size of the stored measurements.</param>
        /// <returns>The index of the oldest measurement.</returns>
        private int GetOldestMeasurementIndex(int pointer, int size)
        {
            return this.Mod((pointer - size) + 1, this.measurementBufferSize);
        }

        /// <summary>
        /// Gets all the measurements closets to a given time stamp and within a given range of that time stamp.
        /// </summary>
        /// <param name="timeStamp">The time stamp in milliseconds of the desired measurements.</param>
        /// <param name="range">The amount of milliseconds that the actual returned may differ from the desired time stamp.</param>
        /// <param name="measurements">The measurements for a source.</param>
        /// <param name="distType">The distribution type of the source.</param>
        /// <returns>A list of all measurements that have the the smallest difference in time stamp.</returns>
        private List<Measurement<Vector3>> GetSourceClosestTo(long timeStamp, long range, Vector3[] measurements, Normal[] distType)
        {
            List<Measurement<Vector3>> res = new List<Measurement<Vector3>>();
            long mindiff = long.MaxValue;
            for (int i = 0; i < this.measurementSize; i++)
            {
                Measurement<Vector3> measurement = new Measurement<Vector3>(measurements[i], this.timeStamps[i], distType[i]);
                long diff = Math.Abs(measurement.TimeStamp - timeStamp);
                if (diff == mindiff && diff <= range)
                {
                    res.Add(measurement);
                }
                else if (diff < mindiff && diff <= range)
                {
                    res.Clear();
                    mindiff = diff;
                    res.Add(measurement);
                }
            }

            return res;
        }

        /// <summary>
        ///     Converts milliseconds to seconds.
        /// </summary>
        /// <param name="ms">The milliseconds to convert.</param>
        /// <returns>Converted milliseconds in seconds</returns>
        private float MilliSecondsToSeconds(long ms)
        {
            return ms / 1000f;
        }

        /// <summary>
        ///     Perform the modulo operation returning a value between 0 and b-1.
        /// </summary>
        /// <param name="a">The number to perform modulo on.</param>
        /// <param name="b">The number to divide by.</param>
        /// <returns>The modulo result.</returns>
        private int Mod(int a, int b)
        {
            return ((a % b) + b) % b;
        }
    }
}