// <copyright file="AbRelPositioning.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace IRescue.Core.Utils
{
    using System;
    using DataTypes;

    /// <summary>
    ///   Utility class to calculate the absolute position of a relative position.
    /// </summary>
    /// TODO test this class after user testing.
    public static class AbRelPositioning
    { 
        /// <summary>
        ///   Convert a radian to a degree angle.
        /// </summary>
        private static double toRadian = Math.PI / 180;

        /// <summary>
        ///   Calculates the absolute location of relatePosition with an absolute maker location and the relative location to it.
        /// </summary>
        /// <param name="absoluteLocation">The known absolute pose.</param>
        /// <param name="relativeLocation">The pose relative to the absolute location.</param>
        /// <returns>The location of the calculated absolute position.</returns>
        public static Pose GetLocation(Pose absoluteLocation, Pose relativeLocation)
        {
            Vector3 absolutePosition = absoluteLocation.Position;
            Vector3 absoluteRotation = absoluteLocation.Orientation;
            Vector3 relativeRotation = relativeLocation.Orientation;
            float distance = CalculateDistance(absolutePosition, relativeLocation.Position);
            Pose newLocation = CalculateLocation(absolutePosition, absoluteRotation, relativeRotation, distance);

            return newLocation;
        }

        /// <summary>
        ///   Calculates the 2D absolute location of relatePosition with an absolute maker location and the relative location to it.
        /// </summary>
        /// <param name="absoluteLocation">The known absolute pose.</param>
        /// <param name="relativeLocation">The pose relative to the absolute location.</param>
        /// <returns>The location of the calculated absolute position.</returns>
        public static Pose GetLocation2D(Pose absoluteLocation, Pose relativeLocation)
        {
            Vector3 absolutePosition = absoluteLocation.Position;
            float distanceXY = (float)Math.Sqrt(relativeLocation.Position.X * relativeLocation.Position.X + relativeLocation.Position.Y + relativeLocation.Position.Y);
            Vector3 rotation = CalculateRotation(absoluteLocation.Orientation, relativeLocation.Orientation);
            Vector3 position = new Vector3(
                absolutePosition.X + (float)(distanceXY * Math.Sin(rotation.Y * toRadian)),
                absolutePosition.Y,
                absolutePosition.Z + (float)(distanceXY * Math.Cos(rotation.Y * toRadian)));

            return new Pose(position, rotation);
        }

        /// <summary>
        ///   Calculate the Euclidean distance.
        /// </summary>
        /// <param name="pos1">First coordinate</param>
        /// <param name="pos2">Second coordinate</param>
        /// <returns>3D euclidean distance</returns>
        private static float CalculateDistance(Vector3 pos1, Vector3 pos2)
        {
            float xValue = pos1.X - pos2.X;
            float yValue = pos1.Y - pos2.Y;
            float zValue = pos1.Z - pos2.Z;

            return (float)Math.Sqrt((xValue * xValue) + (yValue * yValue) + (zValue * zValue));
        }

        private static Vector3 CalculateRotation(Vector3 absoluteRotation, Vector3 relativeRotation)
        {
            return new Vector3(
                relativeRotation.X - absoluteRotation.X,
                relativeRotation.Y - absoluteRotation.Y,
                relativeRotation.Z - absoluteRotation.Z);
        }

        /// <summary>
        ///   Calculate the Absolute location.
        /// </summary>
        /// <param name="absolutePosition">Position of the known point</param>
        /// <param name="absoluteRotation">Rotation of the known point</param>
        /// <param name="relativeRotation">Rotation of the relative point</param>
        /// <param name="distance">Distance to the absolute point as seen from the relative point</param>
        /// <returns>Location and rotation of the relatively given location</returns>
        private static Pose CalculateLocation(
            Vector3 absolutePosition, Vector3 absoluteRotation, Vector3 relativeRotation, float distance)
        {
            Vector3 rotation = CalculateRotation(absoluteRotation, relativeRotation);
            Vector3 position = new Vector3(
                absolutePosition.X + (float)(distance * Math.Cos(rotation.Z * toRadian) * Math.Sin(rotation.Y * toRadian)), 
                absolutePosition.Y + (float)(distance * Math.Sin(rotation.Z * toRadian)), 
                absolutePosition.Z + (float)(distance * Math.Cos(rotation.Z * toRadian) * Math.Cos(rotation.Y * toRadian)));

            return new Pose(position, rotation);
        }
    }
}