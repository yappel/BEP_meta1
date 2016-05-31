// <copyright file="IExtrapolate.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace IRescue.UserLocalisation.PosePrediction
{
    using Core.DataTypes;

    /// <summary>
    /// Extrapolates values.
    /// </summary>
    public interface IExtrapolate
    {
        /// <summary>
        /// Predict the value at timestamp x using extrapolation.
        /// </summary>
        /// <param name="x">The x to predict y at.</param>
        /// <returns>The predicted y.</returns>
        double PredictValueAt(long x);

        /// <summary>
        ///  Add data to the buffer that can be used for the extrapolation.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        void AddData(long x, double y);

        /// <summary>
        /// Predict the difference in y values between to x values.
        /// </summary>
        /// <param name="xfrom">The first x coordinate</param>
        /// <param name="xto">The second x coordinate</param>
        /// <returns>The difference in y coordinates between the two x coordinates.</returns>
        double PredictChange(long xfrom, long xto);
    }
}
