// <copyright file="LinearPredicter.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace IRescue.UserLocalisation.PosePrediction
{
    using System;
    using Core.DataTypes;
    using MathNet.Numerics.LinearAlgebra;

    /// <summary>
    /// Predicts the next <see cref="Pose"/> based on previous poses
    /// </summary>
    public class LinearPredicter
    {
        /// <summary>
        /// The <see cref="Pose"/> before the last <see cref="Pose"/>
        /// </summary>
        private Pose prevprevpose;

        /// <summary>
        /// The previous <see cref="Pose"/>
        /// </summary>
        private Pose prevpose;

        /// <summary>
        /// The timestamp of <see cref="prevprevpose"/>
        /// </summary>
        private long prevprevtime;

        /// <summary>
        /// The timestamp of <see cref="prevpose"/>
        /// </summary>
        private long prevtime;

        /// <summary>
        /// Registers a new <see cref="Pose"/> with a given timestamp.
        /// </summary>
        /// <param name="xyz">The <see cref="Pose"/> to register</param>
        /// <param name="timestamp">The timestamp of the <see cref="Pose"/></param>
        public void AddPose(Pose xyz, long timestamp)
        {
            this.prevprevpose = this.prevpose;
            this.prevprevtime = this.prevtime;
            this.prevpose = xyz;
            this.prevtime = timestamp;
        }

        /// <summary>
        /// Predicts the <see cref="Pose"/> at next timestamp.
        /// </summary>
        /// <param name="timestamp">The timestamp to predict the <see cref="Pose"/> at</param>
        /// <returns>returns the position XYZ and orientation XYZ values in an array</returns>
        public float[] PredictPositionAt(long timestamp)
        {
            if (this.prevpose == null || this.prevprevpose == null)
            {
                return new float[6] { 0, 0, 0, 0, 0, 0 };
            }

            if (timestamp <= this.prevtime)
            {
                throw new ArgumentException("The timestamp has to be larger then the timestamp of the last known position");
            }

            long dt1 = timestamp - this.prevtime;
            long dt2 = this.prevtime - this.prevprevtime;
            float[] pospredict = this.Predict(this.prevpose.Position, this.prevprevpose.Position, dt1, dt2);
            float[] oripredict = this.Predict(this.prevpose.Orientation, this.prevprevpose.Orientation, dt1, dt2);
            float[] result = new float[pospredict.Length + oripredict.Length];
            Array.Copy(pospredict, result, pospredict.Length);
            Array.Copy(oripredict, 0, result, pospredict.Length, oripredict.Length);
            return result;
        }

        /// <summary>
        /// Calculates the linear difference between 2 locations at given the timestamps of the locations.
        /// </summary>
        /// <param name="prev">The second location</param>
        /// <param name="prevprev">The first location</param>
        /// <param name="dt1">The timestamp of the first location</param>
        /// <param name="dt2">The timestamp of the second location</param>
        /// <returns>The linear difference per time unit</returns>
        public float[] Predict(Vector3 prev, Vector3 prevprev, long dt1, long dt2)
        {
            Vector<float> temp = prev.Add(prevprev.Negate()).Multiply((float)dt1 / (float)dt2);
            return temp.ToArray();
        }
    }
}
