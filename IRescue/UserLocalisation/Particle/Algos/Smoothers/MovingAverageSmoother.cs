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

        /// <summary>Creates a new object that is a copy of the current instance.</summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public ISmoother Clone()
        {
            return new MovingAverageSmoother(this.buffersize);
        }

        /// <inheritdoc/>
        public Vector3 GetSmoothedResult(Vector3 rawResult, long timeStamp, Func<float[], float> averageFunction)
        {
            this.buffer.Add(new Result { Vector3 = rawResult, TimeStamp = timeStamp });
            List<Vector3> allResults = this.GetResults(timeStamp);

            return new Vector3(
                averageFunction(allResults.Select(v => v.X).ToArray()),
                averageFunction(allResults.Select(v => v.Y).ToArray()),
                averageFunction(allResults.Select(v => v.Z).ToArray()));
        }

        /// <summary>
        /// Removes exess values out the buffer and return a list of all vectors still in the buffer.
        /// </summary>
        /// <param name="timeStamp">The timestamp of the last item of the set of results to calculate the average of.</param>
        /// <returns>List of all vectors in the buffer.</returns>
        private List<Vector3> GetResults(long timeStamp)
        {
            List<Vector3> allResults = new List<Vector3>();
            List<int> toremove = new List<int>();
            for (int i = 0; i < this.buffer.Count; i++)
            {
                if (this.buffer[i].TimeStamp + this.buffersize < timeStamp)
                {
                    toremove.Add(this.buffer.IndexOf(this.buffer[i]));
                }
                else
                {
                    allResults.Add(this.buffer[i].Vector3);
                }
            }

            this.RemoveFromBuffer(toremove);
            return allResults;
        }

        /// <summary>
        /// Removes items from the buffer.
        /// </summary>
        /// <param name="toremove">Indexes of the items to remove.</param>
        private void RemoveFromBuffer(IList<int> toremove)
        {
            for (int index = 0; index < toremove.Count; index++)
            {
                this.buffer.RemoveAt(toremove[index]);
            }
        }
    }
}