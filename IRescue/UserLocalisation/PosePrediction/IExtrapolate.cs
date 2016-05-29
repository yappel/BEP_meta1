// <copyright file="IExtrapolate.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace IRescue.UserLocalisation.PosePrediction
{
    using Core.DataTypes;

    /// <summary>
    /// Predicts the <see cref="Pose"/> at a certain timestamp
    /// </summary>
    public interface IExtrapolate
    {
        /// <summary>
        /// Predicts the <see cref="Pose"/> at next timestamp.
        /// </summary>
        /// <param name="timeStamp">The timestamp to predict the <see cref="Pose"/> at</param>
        /// <returns>returns the position XYZ and orientation XYZ values in an array</returns>
        double PredictValueAt(long x);

        /// <summary>
        /// Tells the predictor what the pose at was at a certain time stamp.
        /// </summary>
        /// <param name="timeStamp">The time stamp of the given pose</param>
        /// <param name="pose">The given pose</param>
        void AddData(long x, double y);

        double PredictChange(long xfrom, long xto);
    }
}
