// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EulerAnglesConversion.cs" company="">
//   
// </copyright>
// <summary>
//   Utility class to convert different type of Euler angles.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts.Unity.Utils
{
    using IRescue.Core.DataTypes;

    using UnityEngine;

    using Vector3 = UnityEngine.Vector3;

    /// <summary>
    /// Utility class to convert different type of Euler angles.
    /// </summary>
    public static class EulerAnglesConversion
    {
        /// <summary>
        /// Converts XYZ Tait-Bryan angles to a <see cref="Quaternion"/>.
        /// </summary>
        /// <param name="xyzAngles">The XYZ angles in degrees</param>
        /// <returns>The created <see cref="Quaternion"/></returns>
        public static Quaternion XYZtoQuaternion(Vector3 xyzAngles)
        {
            RotationMatrix rotmat = new RotationMatrix(xyzAngles.x, xyzAngles.y, xyzAngles.z);
            IRescue.Core.DataTypes.Vector3 forward = new IRescue.Core.DataTypes.Vector3(0, 0, 1);
            IRescue.Core.DataTypes.Vector3 upward = new IRescue.Core.DataTypes.Vector3(0, 1, 0);
            rotmat.Multiply(forward, forward);
            rotmat.Multiply(upward, upward);
            return Quaternion.LookRotation(IRescueVec3ToUnityVec3(forward), IRescueVec3ToUnityVec3(upward));
        }

        /// <summary>
        /// Converts ZXY Tait-Bryan angles to XYZ Tait-Bryan angles.
        /// </summary>
        /// <param name="zxyAngles">The ZXY angles in degrees</param>
        /// <returns>The XYZ angles in degrees</returns>
        public static Vector3 ZXYtoXYZ(Vector3 zxyAngles)
        {
            Vector3 axis;
            float angle;
            Quaternion.Euler(zxyAngles).ToAngleAxis(out angle, out axis);
            RotationMatrix rm = new RotationMatrix(
                new IRescue.Core.DataTypes.Vector3(axis.x, axis.y, axis.z),
                angle);
            return IRescueVec3ToUnityVec3(rm.EulerAngles);
        }

        /// <summary>
        /// Converts a <see cref="IRescue.Core.DataTypes.Vector3"/> to a <see cref="UnityEngine.Vector3"/>.
        /// </summary>
        /// <param name="irescuevector">The <see cref="IRescue.Core.DataTypes.Vector3"/> to convert.</param>
        /// <returns>The <see cref="UnityEngine.Vector3"/>.</returns>
        private static Vector3 IRescueVec3ToUnityVec3(IRescue.Core.DataTypes.Vector3 irescuevector)
        {
            return new Vector3(irescuevector.X, irescuevector.Y, irescuevector.Z);
        }
    }
}