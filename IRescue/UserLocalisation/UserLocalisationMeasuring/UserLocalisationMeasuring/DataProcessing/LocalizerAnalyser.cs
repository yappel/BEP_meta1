// <copyright file="LocalizerAnalyser.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using IRescue.Core.DataTypes;
using IRescue.UserLocalisation;

namespace IRescue.UserLocalisationMeasuring.DataProcessing
{
    /// <summary>
    /// Analyzes the a localisation filter and calculates metrics.
    /// </summary>
    public class LocalizerAnalyser
    {
        /// <summary>
        /// The average runtime the filter in milliseconds
        /// </summary>
        public long AverageRuntime { get; private set; }

        public double Precision { get; private set; }

        public double Accuracy { get; private set; }

        public LocalizerAnalyser(int repititions, int cycleamount, AbstractUserLocalizer filter)
        {
            List<Result> results = GenerateResults(repititions, cycleamount, filter);
            this.AverageRuntime = CalculateAverageRuntime(results);
            this.Precision = CalculatePrecision(results);
            this.Accuracy = CalculateAccuracy(results);
        }

        private List<Result> GenerateResults(int repititions, int cycleamount, AbstractUserLocalizer filter)
        {
            List<Result> results = new List<Result>(repititions);
            for (int i = 0; i < repititions; i++)
            {
                results[i] = GenerateResult(cycleamount, filter);
            }
            return results;
        }

        private Result GenerateResult(int cycleamount, AbstractUserLocalizer filter)
        {
            throw new NotImplementedException();
        }

        private long CalculateAverageRuntime(List<Result> results)
        {
            throw new NotImplementedException();
        }

        private double CalculatePrecision(List<Result> results)
        {
            throw new NotImplementedException();
        }

        private float CalculateAccuracy(List<Result> results)
        {
            throw new NotImplementedException();
        }

        private struct Result
        {
            /// <summary>
            /// The time it took for the localizer to come to this <see cref="Result"/>
            /// </summary>
            public long Runtime;

            /// <summary>
            /// The location the localizer guessed.
            /// </summary>
            public Pose Location;

        }
    }

}
