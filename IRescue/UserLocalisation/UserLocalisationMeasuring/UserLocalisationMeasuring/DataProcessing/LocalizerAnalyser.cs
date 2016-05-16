// <copyright file="LocalizerAnalyser.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using IRescue.Core.DataTypes;
using IRescue.Core.Utils;
using IRescue.UserLocalisation;
using IRescue.UserLocalisationMeasuring.DataGeneration;

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

        /// <summary>
        /// The mean absolute deviation of the localizer, an indication how big the spread of the results is.
        /// </summary>
        public double Precision { get; private set; }

        /// <summary>
        /// The accuracy of the localizer, the absolute deviation of the average result.
        /// </summary>
        public double Accuracy { get; private set; }

        public LocalizerAnalyser(int repititions, int cycleamount, AbstractUserLocalizer filter, PositionScenario posscen, OrientationScenario oriscen)
        {
            List<Result> results = GenerateResults(repititions, cycleamount, filter);
            Pose averageResult = CalculateAverageResult(results);
            this.AverageRuntime = CalculateAverageRuntime(results);
            this.Precision = CalculatePrecision(results, posscen, oriscen, filter.Fieldsize, cycleamount);
            this.Accuracy = CalculateAccuracy(averageResult, posscen, oriscen, filter.Fieldsize, cycleamount);
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
            long begintime = StopwatchSingleton.Time;
            Result res = new Result();
            for (int time = 1; time < cycleamount; time++)
            {
                filter.CalculatePose(time);
            }

            res.Pose = filter.CalculatePose(cycleamount);
            res.Runtime = StopwatchSingleton.Time - begintime;
            return res;
        }

        private Pose CalculateAverageResult(List<Result> results)
        {
            Vector3 oriSum = new Vector3();
            Vector3 posSum = new Vector3();
            foreach (Result result in results)
            {
                oriSum.Add(result.Pose.Position, oriSum);
                posSum.Add(result.Pose.Orientation, posSum);
            }
            oriSum.Divide(results.Count, oriSum);
            posSum.Divide(results.Count, posSum);
            return new Pose(posSum, oriSum);
        }

        private long CalculateAverageRuntime(List<Result> results)
        {
            long sum = results.Sum(result => result.Runtime);
            return sum / results.Count;
        }

        private double CalculatePrecision(List<Result> results, PositionScenario posscen, OrientationScenario oriscen, FieldSize fieldsize, long cycleamount)
        {
            float sum = 0;
            foreach (Result result in results)
            {
                sum += Math.Abs(result.Pose.Orientation.X - posscen.RealX(cycleamount - 1)) / fieldsize.Xmax;
                sum += Math.Abs(result.Pose.Orientation.Y - posscen.RealY(cycleamount - 1)) / fieldsize.Ymax;
                sum += Math.Abs(result.Pose.Orientation.Z - posscen.RealZ(cycleamount - 1)) / fieldsize.Zmax;
                sum += Math.Abs(result.Pose.Position.X - oriscen.RealX(cycleamount - 1)) / 360;
                sum += Math.Abs(result.Pose.Position.Y - oriscen.RealY(cycleamount - 1)) / 360;
                sum += Math.Abs(result.Pose.Position.Z - oriscen.RealZ(cycleamount - 1)) / 360;
            }
            return sum;
        }

        private float CalculateAccuracy(Pose average, PositionScenario posscen, OrientationScenario oriscen, FieldSize fieldsize, long cycleamount)
        {
            float sum = 0;
            sum += Math.Abs(average.Orientation.X - posscen.RealX(cycleamount - 1)) / fieldsize.Xmax;
            sum += Math.Abs(average.Orientation.Y - posscen.RealY(cycleamount - 1)) / fieldsize.Ymax;
            sum += Math.Abs(average.Orientation.Z - posscen.RealZ(cycleamount - 1)) / fieldsize.Zmax;
            sum += Math.Abs(average.Position.X - oriscen.RealX(cycleamount - 1)) / 360;
            sum += Math.Abs(average.Position.Y - oriscen.RealY(cycleamount - 1)) / 360;
            sum += Math.Abs(average.Position.Z - oriscen.RealZ(cycleamount - 1)) / 360;
            return sum;
        }

        private struct Result
        {
            /// <summary>
            /// The time it took for the localizer to come to this <see cref="Result"/>
            /// </summary>
            public long Runtime;

            /// <summary>
            /// The pose the localizer guessed.
            /// </summary>
            public Pose Pose;

        }
    }

}
