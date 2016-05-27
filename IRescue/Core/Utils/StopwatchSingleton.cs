// <copyright file="StopwatchSingleton.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace IRescue.Core.Utils
{
    using System.Diagnostics;

    /// <summary>
    ///  Singleton to get the current time.
    /// </summary>
    public class StopwatchSingleton
    {
        /// <summary>
        /// Stopwatch to get the time
        /// </summary>
        private static Stopwatch instance;

        /// <summary>
        ///  Prevents a default instance of the <see cref="StopwatchSingleton"/> class from being created
        /// </summary>
        private StopwatchSingleton()
        {
        }

        /// <summary>
        ///  Gets the current time
        /// </summary>
        public static long Time
        {
            get
            {
                if (instance == null)
                {
                    instance = new Stopwatch();
                    instance.Start();
                }

                return instance.ElapsedMilliseconds;
            }
        }
    }
}
