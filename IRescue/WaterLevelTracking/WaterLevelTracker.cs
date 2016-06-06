// <copyright file="WaterLevelTracker.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace WaterLevelTracking
{
    using System.Drawing;
    using IRescue.Core.DataTypes;

    /// <summary>
    /// This class calculates or predicts the water level based on measurements
    /// </summary>
    public class WaterLevelTracker
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WaterLevelTracker"/> class.
        /// </summary>
        public WaterLevelTracker()
        {
        }

        /// <summary>
        /// Segment the input and add the measurement of the water level
        /// </summary>
        /// <param name="image">The image of the marker and the pole with stripes</param>
        /// <param name="markerPosition">The pixel position of the marker in the image input</param>
        /// <param name="markerSize">The size in pixels of the marker as seen in the image input</param>
        /// <param name="rotation">The rotation of the marker in the image input in degrees</param>
        /// <param name="timestamp">The timestamp of the measurement</param>
        public void AddMeasurement(Bitmap image, Vector3 markerPosition, float markerSize, float rotation, long timestamp)
        {
            //// Perform calculations here, calculate the new linear lambda
        }

        /// <summary>
        /// Segment the input and add the measurement of the water level if the rotation of the marker is not known.
        /// </summary>
        /// <param name="image">The image of the marker and the pole with stripes</param>
        /// <param name="markerPosition">The pixel position of the marker in the image input</param>
        /// <param name="markerSize">The size in pixels of the marker as seen in the image input</param>
        /// <param name="timestamp">The timestamp of the measurement</param>
        public void AddMeasurement(Bitmap image, Vector3 markerPosition, float markerSize, long timestamp)
        {
            //// Calculate the rotation, call AddMeasurements(5).
        }

        /// <summary>
        /// Segment the input and add the measurement of the water level if the position of the marker in the image input is not known.
        /// </summary>
        /// <param name="image">The image of the marker and the pole with stripes</param>
        /// <param name="rotation">The rotation of the marker in the image input in degrees</param>
        /// <param name="timestamp">The timestamp of the measurement</param>
        public void AddMeasurement(Bitmap image, float rotation, long timestamp)
        {
            //// Calculate the position, call AddMeasurements(5).
        }

        /// <summary>
        /// Segment the input and add the measurement of the water level if it is only known that a marker is visible.
        /// </summary>
        /// <param name="image">The image of the marker and the pole with stripes</param>
        /// <param name="timestamp">The timestamp of the measurement</param>
        public void AddMeasurement(Bitmap image, long timestamp)
        {
            //// Calculate the rotation and position, call AddMeasurements(5).
        }

        /// <summary>
        /// Get the prediction of the new water level
        /// </summary>
        /// <param name="timestamp">the timestamp of the call</param>
        /// <returns>the water level in meters</returns>
        public float GetWaterLevel(long timestamp)
        {
            //// Calculate the new water level based on the current lambda and the given timestamp
            return 0;
        }
    }
}
