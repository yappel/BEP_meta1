// <copyright file="LinearPredicter.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

using System;
using IRescue.Core.DataTypes;
using MathNet.Numerics.LinearAlgebra;

namespace UserLocalisation.PositionPrediction
{
    /// <summary>
    /// TODO
    /// </summary>
    public class LinearPredicter
    {
        private Pose prevprevpose;
        private Pose prevpose;
        private long prevprevtime;
        private long prevtime;

        public void addPose(Pose xyz, long timestamp)
        {
            prevprevpose = prevpose;
            prevprevtime = prevtime;
            prevpose = xyz;
            prevtime = timestamp;
        }

        public float[] predictPositionAt(long timestamp)
        {
            if (prevpose == null || prevprevpose == null)
            {
                return new float[6] { 0, 0, 0, 0, 0, 0 }; ;
            }
            if (timestamp <= prevtime)
            {
                throw new ArgumentException("The timestamp has to be larger then the timestamp of the last known position");
            }
            long dt1 = timestamp - prevtime;
            long dt2 = prevtime - prevprevtime;
            float[] pospredict = predict(prevpose.Position, prevprevpose.Position, dt1, dt2);
            float[] oripredict = predict(prevpose.Orientation, prevprevpose.Orientation, dt1, dt2);
            float[] result = new float[pospredict.Length + oripredict.Length];
            Array.Copy(pospredict, result, pospredict.Length);
            Array.Copy(oripredict, 0, result, pospredict.Length, oripredict.Length);
            return result;
        }

        public float[] predict(Vector3 prev, Vector3 prevprev, long dt1, long dt2)
        {
            Vector<float> temp = prev.Add(prevprev.Negate()).Multiply((float)dt1 / (float)dt2);
            return temp.ToArray();
        }
    }
}
