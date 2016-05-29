// <copyright file="AccelerationPositionKalmanFilter.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

using System;
using IRescue.Core.DataTypes;
using IRescue.UserLocalisation.Sensors;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace IRescue.UserLocalisation.Kalman
{
    public class AccelerationPositionKalmanFilter : AbstractUserLocalizer, IAccelerationReceiver, IPositionReceiver
    {
        /// <summary>
        /// List storing all registered acceleration sources for the localiser.
        /// </summary>
        private List<IAccelerationSource> accelerationSources;

        /// <summary>
        /// List storing all registered position sources for the localiser
        /// </summary>
        private List<IPositionSource> positionSources;

        // TODO float or double?
        private Matrix<double> stateTransition;
        private Matrix<double> covarianceStateVectorEstimate;
        private Matrix<double> processNoiseCovariance;
        private Matrix<double> measurementNoiseCovariance;
        private Matrix<double> observationMatrix;
        private Vector<double> stateVector;

        // TODO
        private long previousTimeStamp = -1;

        public AccelerationPositionKalmanFilter(FieldSize fieldsize) : base(fieldsize)
        {
            this.accelerationSources = new List<IAccelerationSource>();
            this.positionSources = new List<IPositionSource>();
        }

        /// <summary>
        /// Add an <see cref="IAccelerationSource"/> to the filter from which acceleration
        /// data can be accessed.
        /// </summary>
        /// <param name="source">The source to be added.</param>
        public void AddAccelerationSource(IAccelerationSource source)
        {
            this.accelerationSources.Add(source);
        }

        /// <summary>
        /// Add an <see cref="IPositionSource"/> to the filter from which position information
        /// can be obtained.
        /// </summary>
        /// <param name="source"></param>
        public void AddPositionSource(IPositionSource source)
        {
            this.positionSources.Add(source);
        }

        public override Pose CalculatePose(long timeStamp)
        {
            List<Measurement<Vector3>> positions = new List<Measurement<Vector3>>();
            for (int i = 0; i < this.positionSources.Count; i++)
            {
                positions.AddRange(this.positionSources[i].GetPositions(this.previousTimeStamp, timeStamp));
            }
            List<Measurement<Vector3>> accelerations = new List<Measurement<Vector3>>();
            for (int i = 0; i < this.accelerationSources.Count; i++)
            {
                accelerations.AddRange(this.accelerationSources[i].GetAccelerations(this.previousTimeStamp, timeStamp));
            }

            // Predict
            this.SetDeltaTStateTransition(timeStamp);
            Vector<double> predictedState = this.stateTransition.Multiply(this.stateVector);
            Matrix<double> predictedCovariance = this.stateTransition.Multiply(this.covarianceStateVectorEstimate).Multiply(this.stateTransition.Transpose()); // + Q
            // Measurements
            // calc noise
            throw new NotImplementedException();
        }

        private void InitialiseMatrices()
        {
            this.stateTransition = Matrix<double>.Build.DenseDiagonal(9, 9, 1);
            this.covarianceStateVectorEstimate = Matrix<double>.Build.DenseDiagonal(9, 9, 1);
        }

        private void SetDeltaTStateTransition(long timeStamp)
        {
            double deltaT = (timeStamp - this.previousTimeStamp) / 1000.0;
            this.stateTransition[0, 3] = deltaT;
            this.stateTransition[0, 6] = 0.5 * deltaT * deltaT;
            this.stateTransition[1, 4] = deltaT;
            this.stateTransition[1, 7] = 0.5 * deltaT * deltaT;
            this.stateTransition[2, 5] = deltaT;
            this.stateTransition[2, 8] = 0.5 * deltaT * deltaT;
        }
    }
}
