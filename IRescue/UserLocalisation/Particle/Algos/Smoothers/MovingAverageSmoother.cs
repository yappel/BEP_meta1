// <copyright file="MovingAverageSmoother.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>
namespace IRescue.UserLocalisation.Particle.Algos.Smoothers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

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
        public Vector3 GetSmoothedResult(Vector3 rawResult, long timeStamp, Func<float[], float> averageFunction)
        {
            this.buffer.Add(new Result { vector3 = rawResult, TimeStamp = timeStamp });
            List<Vector3> allResults = new List<Vector3>();
            List<int> toremove = new List<int>();
            foreach (Result result in this.buffer)
            {
                if (result.TimeStamp + this.buffersize < timeStamp)
                {
                    toremove.Add(this.buffer.IndexOf(result));
                }
                else
                {
                    allResults.Add(result.vector3);
                }
            }

            for (int index = 0; index < toremove.Count; index++)
            {
                this.buffer.RemoveAt(toremove[index]);
            }

            return new Vector3(
                averageFunction(allResults.Select<Vector3, float>((v) => v.X).ToArray()),
                averageFunction(allResults.Select<Vector3, float>((v) => v.Y).ToArray()),
                averageFunction(allResults.Select<Vector3, float>((v) => v.Z).ToArray())
                );
        }

        /// <summary>Creates a new object that is a copy of the current instance.</summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public ISmoother Clone()
        {
            return new MovingAverageSmoother(this.buffersize);
        }
    }
}