// <copyright file="LinearRegression.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace IRescue.UserLocalisation.PosePrediction
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using MathNet.Numerics;
    using MathNet.Numerics.Interpolation;

    /// <summary>
    /// Predicts values using linear regression.
    /// </summary>
    public class LinearRegression : IExtrapolate
    {
        private const int Buffersize = 150;

        private readonly SortedList<double, double> data;

        /// <summary>
        /// Initializes a new instance of the <see cref="LinearRegression"/> class.
        /// </summary>
        public LinearRegression()
        {
            this.data = new SortedList<double, double>();
        }

        /// <inheritdoc/>
        public void AddData(long x, double y)
        {
            this.data.Add(x, y);

            while (this.data.First().Key < x - Buffersize)
            {
                this.data.RemoveAt(0);
            }
        }

        /// <inheritdoc/>
        public double PredictChange(long xfrom, long xto)
        {
            double yfrom = this.PredictValueAt(xfrom);
            double yto = this.PredictValueAt(xto);
            return yto - yfrom;
        }

        /// <inheritdoc/>
        public double PredictValueAt(long x)
        {
            if (this.data.Count < 3)
            {
                return 0;
            }

            Tuple<double, double> linearparams = Fit.Line(this.data.Keys.ToArray(), this.data.Values.ToArray());
            double a = linearparams.Item1;
            double b = linearparams.Item2;
            return a + (b * x);
        }
    }
}