// <copyright file="MovingAverageSmoother.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>
namespace IRescue.UserLocalisation.Particle.Algos.Smoothers
{
    using System.Collections.Generic;

    using IRescue.Core.DataTypes;

    /// <summary>
    /// Smooths results using a simple weighted average algorithm.
    /// </summary>
    public class MovingAverageSmoother : ISmoother
    {
        private readonly List<Result> buffer;

        private readonly int buffersize;

        /// <summary>
        /// Initializes a new instance of the <see cref="MovingAverageSmoother"/> class.
        /// </summary>
        /// <param name="buffersize">The amount of milliseconds the algorithm looks in the past to calculate the average.</param>
        public MovingAverageSmoother(int buffersize)
        {
            this.buffer = new List<Result>();
            this.buffersize = buffersize;
        }

        /// <summary>
        /// Calculates the smoothed result.
        /// </summary>
        /// <param name="rawResult">The unsmoothed result.</param>
        /// <param name="timeStamp">The timestamp of the (un)smoothed result.</param>
        /// <returns>The smoothed result</returns>
        public Pose GetSmoothedResult(Pose rawResult, long timeStamp)
        {
            this.buffer.Add(new Result { Pose = rawResult, TimeStamp = timeStamp });
            Vector3 averagePos = new Vector3();
            Vector3 averageOri = new Vector3();
            float count = 0;
            foreach (Result result in this.buffer)
            {
                if (result.TimeStamp + this.buffersize < timeStamp)
                {
                    this.buffer.Remove(result);
                }
                else
                {
                    count++;
                    averagePos.Add(result.Pose.Position, averagePos);
                    averageOri.Add(result.Pose.Orientation, averageOri);
                }
            }

            averagePos.Divide(count, averagePos);
            averageOri.Divide(count, averagePos);
            return new Pose(averagePos, averageOri);
        }
    }
}