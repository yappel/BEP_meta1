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
            float distanceXZ = CalculateDistance(relativePose.Position.X, relativePose.Position.Z);
            float distanceXY = CalculateDistance(relativePose.Position.X, relativePose.Position.Y);
            float distanceYZ = CalculateDistance(relativePose.Position.Y, relativePose.Position.Z);

            Vector3 rotation = CalculateOrientation(absolutePose.Orientation, relativePose.Orientation);
            Vector3 orientation = CalculateOrientation(rotation, relativePose.Position, distanceXZ, distanceYZ);

            return new Pose(new Vector3(
                absolutePose.Position.X + (distanceXZ * (float)Math.Cos(Trig.DegreeToRadian(rotation.Y))),
                absolutePose.Position.Y + (distanceXY * (float)Math.Sin(Trig.DegreeToRadian(rotation.Z))),
                absolutePose.Position.Z + (distanceXZ * (float)Math.Sin(Trig.DegreeToRadian(rotation.Y)))), orientation);
        }

        /// <summary>
        ///   Calculates the 2D absolute location of relatePosition with an absolute maker location and the relative location to it.
        /// </summary>
        /// <param name="absolutePose">The known absolute pose.</param>
        /// <param name="relativePose">TThe measured pose.</param>
        /// <returns>The location of the calculated absolute position.</returns>
        public static Pose GetLocation2D(Pose absolutePose, Pose relativePose)
        {
            float distanceXZ = CalculateDistance(relativePose.Position.X, relativePose.Position.Z);
            float distanceXY = CalculateDistance(relativePose.Position.X, relativePose.Position.Y);
            float distanceYZ = CalculateDistance(relativePose.Position.Y, relativePose.Position.Z);

            Vector3 rotation = CalculateOrientation(absolutePose.Orientation, relativePose.Orientation);
            Vector3 orientation = CalculateOrientation(rotation, relativePose.Position, distanceXY, distanceYZ);
            return new Pose(new Vector3(
                absolutePose.Position.X + (distanceXY * (float)Math.Cos(Trig.DegreeToRadian(rotation.Y))),
                absolutePose.Position.Y,
                absolutePose.Position.Z + (distanceXY * (float)Math.Sin(Trig.DegreeToRadian(rotation.Y)))), orientation);

        }

        /// <summary>
        /// Calculate the orientation of the user.
        /// </summary>
        /// <param name="rotation">The angle from the normal of the marker to the user</param>
        /// <param name="relativePosition">The position of the user</param>
        /// <param name="distanceXZ">The distance between x and z</param>
        /// <param name="distanceYZ">The distance between y and z</param>
        /// <returns></returns>
        private static Vector3 CalculateOrientation(Vector3 rotation, Vector3 relativePosition, float distanceXZ, float distanceYZ)
        {
            if (distanceXZ > epsilon && distanceYZ > epsilon)
            {
                return new Vector3(
                    rotation.X - 180 - (float)Math.Acos(relativePosition.Z / distanceYZ),
                    rotation.Y - 180 - (float)Math.Acos(relativePosition.Z / distanceXZ), rotation.Z);
            }
            else
            {
                return new Vector3(rotation.X - 180, rotation.Y - 180, rotation.Z);
            }
        }

        /// <summary>
        ///   Calculate the Absolute position.
        /// </summary>
        /// <param name="abPose">Pose of the known point</param>
        /// <param name="relPose">Pose of the measured point</param>
        /// <returns>The expected position</returns>
        private static Pose CalculatePose(Pose abPose, Pose relPose)
        {
            float distanceXZ = CalculateDistance(relPose.Position.X, relPose.Position.Z);
            float distanceXY = CalculateDistance(relPose.Position.X, relPose.Position.Y);
            Vector3 rotation = CalculateOrientation(abPose.Orientation, relPose.Orientation);
            return new Pose(new Vector3(
                abPose.Position.X + (distanceXZ * (float)Math.Cos(Trig.DegreeToRadian(rotation.Y))),
                abPose.Position.Y + (distanceXY * (float)Math.Sin(Trig.DegreeToRadian(rotation.Z))),
                abPose.Position.Z + (distanceXZ * (float)Math.Sin(Trig.DegreeToRadian(rotation.Y)))),
                new Vector3(
                    rotation.X - 180 - (float)Math.Acos(relPose.Position.Z / (1)),
                    rotation.Y - 180 - (float)Math.Acos(relPose.Position.Z / (1)),
                    rotation.Z - 180 - (float)Math.Acos(relPose.Position.Z / (1))));
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