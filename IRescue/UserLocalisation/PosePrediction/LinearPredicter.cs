// <copyright file="LinearPredicter.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

using System;
using IRescue.Core.DataTypes;

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

        public Pose predictPositionAt(long timestamp)
        {
            if (prevpose == null || prevprevpose == null)
            {
                return new Pose();
            }
            if (timestamp < prevtime)
            {
                throw new ArgumentException("The timestamp has to be larger then the timestamp of the last known position");
            }
            long dt1 = timestamp - prevtime;
            long dt2 = prevtime - prevprevtime;
            Vector3 pospredict = predict(prevpose.Position, prevprevpose.Position, dt1, dt2);
            Vector3 oripredict = predict(prevpose.Orientation, prevprevpose.Orientation, dt1, dt2);
            return new Pose(pospredict, oripredict); ;
        }

        private Vector3 predict(Vector3 prev, Vector3 prevprev, long dt1, long dt2)
        {
            return prev.Add(prevprev.Negate()).Multiply(dt1 / dt2) as Vector3;
        }
    }
}
