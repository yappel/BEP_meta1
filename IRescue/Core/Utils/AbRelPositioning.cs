// <copyright file="AbRelPositioning.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace IRescue.Core.Utils
{
    using System;
    using DataTypes;
    using MathNet.Numerics;

    /// <summary>
    ///   Utility class to calculate the absolute pose of a relative pose.
    /// </summary>
    public static class AbRelPositioning
    {
        /// <summary>
        /// The epsilon
        /// </summary>
        private static float epsilon = 0.00000001f;

        /// <summary>
        ///   Calculates the absolute location of relatePosition with an absolute maker location and the relative location to it.
        /// </summary>
        /// <param name="absolutePose">The known absolute pose.</param>
        /// <param name="relativePose">The pose relative to the absolute location.</param>
        /// <returns>The location of the calculated absolute position.</returns>
        public static Pose GetLocation(Pose absolutePose, Pose relativePose)
        {
            Vector3 position = CalculatePosition(absolutePose, relativePose);
            Vector3 orientation = CalculateOrientation(absolutePose, relativePose);

            return new Pose(position, orientation);
        }

        /// <summary>
        ///   Calculates the 2D absolute location of relatePosition with an absolute maker location and the relative location to it.
        /// </summary>
        /// <param name="absolutePose">The known absolute pose.</param>
        /// <param name="relativePose">TThe measured pose.</param>
        /// <returns>The location of the calculated absolute position.</returns>
        public static Pose GetLocation2D(Pose absolutePose, Pose relativePose)
        {
            Vector3 position = CalculateLocation2D(absolutePose, relativePose);
            Vector3 orientation = CalculateOrientation(absolutePose, relativePose);

            return new Pose(position, orientation);
        }

        /// <summary>
        /// Calculate the absolute orientation of the relative pose
        /// </summary>
        /// <param name="abPose">the absolute pose</param>
        /// <param name="relPose">the measured pose</param>
        /// <returns>pose of the calculated position</returns>
        private static Vector3 CalculateLocation2D(Pose abPose, Pose relPose)
        {
            float distanceXY = CalculateDistance(relPose.Position.X, relPose.Position.Z);
            Vector3 rotation = CalculateOrientation(abPose.Orientation, relPose.Orientation);
            return new Vector3(
                abPose.Position.X + (distanceXY * (float)Math.Cos(Trig.DegreeToRadian(rotation.Y))),
                abPose.Position.Y,
                abPose.Position.Z + (distanceXY * (float)Math.Sin(Trig.DegreeToRadian(rotation.Y))));
        }

        /// <summary>
        ///   Calculate the Absolute position.
        /// </summary>
        /// <param name="abPose">Pose of the known point</param>
        /// <param name="relPose">Pose of the measured point</param>
        /// <returns>The expected position</returns>
        private static Vector3 CalculatePosition(Pose abPose, Pose relPose)
        {
            float distanceXZ = CalculateDistance(relPose.Position.X, relPose.Position.Z);
            float distanceXY = CalculateDistance(relPose.Position.X, relPose.Position.Y);
            Vector3 rotation = CalculateOrientation(abPose.Orientation, relPose.Orientation);
            return new Vector3(
                abPose.Position.X + (distanceXZ * (float)Math.Cos(Trig.DegreeToRadian(rotation.Y))),
                abPose.Position.Y + (distanceXY * (float)Math.Sin(Trig.DegreeToRadian(rotation.Z))),
                abPose.Position.Z + (distanceXZ * (float)Math.Sin(Trig.DegreeToRadian(rotation.Y))));
        }

        /// <summary>
        /// Calculate the orientation of the relative pose
        /// </summary>
        /// <param name="abPose">The absolute pose</param>
        /// <param name="relPose">The relative pose</param>
        /// <returns>Vector3 orientation</returns>
        private static Vector3 CalculateOrientation(Pose abPose, Pose relPose)
        {
            if (relPose.Position.Z > epsilon && relPose.Position.Y > epsilon && relPose.Position.Z > epsilon)
            {
                return new Vector3(
                    270 + (float)Math.Atan(relPose.Position.Y / relPose.Position.Z) + abPose.Orientation.X - relPose.Orientation.X,
                    270 + (float)Math.Atan(relPose.Position.X / relPose.Position.Z) + abPose.Orientation.Y - relPose.Orientation.Y,
                    270 + (float)Math.Atan(relPose.Position.X / relPose.Position.Y) + abPose.Orientation.Z - relPose.Orientation.Z);
            }
            else
            {
                Console.WriteLine("WARNING: measured position cotained a 0.");
                return new Vector3(0, 0, 0);
            }
        }

        /// <summary>
        /// Return the distance in 2D from a point (0,0)
        /// </summary>
        /// <param name="pos1">first value</param>
        /// <param name="pos2">second value</param>
        /// <returns>The distance between (0,0) and (value, value)</returns>
        private static float CalculateDistance(float pos1, float pos2)
        {
            return (float)Math.Sqrt((pos1 * pos1) + (pos2 * pos2));
        }

        /// <summary>
        /// Calculate the absolute orientation between 2 vectors
        /// </summary>
        /// <param name="absoluteRotation">absolute orientation</param>
        /// <param name="relativeRotation">measured orientation</param>
        /// <returns>the absolute orientation of the relative position</returns>
        private static Vector3 CalculateOrientation(Vector3 absoluteRotation, Vector3 relativeRotation)
        {
            return new Vector3(
                absoluteRotation.X - relativeRotation.X,
                absoluteRotation.Y - relativeRotation.Y,
                absoluteRotation.Z - relativeRotation.Z);
        }
    }
}