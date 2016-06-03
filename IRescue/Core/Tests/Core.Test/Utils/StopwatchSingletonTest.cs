// <copyright file="StopwatchSingletonTest.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Core.Test
{
    using IRescue.Core.Utils;
    using NUnit.Framework;

    /// <summary>
    /// Test class for testing the <see cref="IRescue.Core.Utils.StopwatchSingleton"/> class.
    /// </summary>
    [TestFixture]
    public class StopwatchSingletonTest
    {
        /// <summary>
        /// Test that the first call is not null
        /// </summary>
        [Test]
        public void FirstCallNotNullTest()
        {
            Assert.IsNotNull(StopwatchSingleton.Time);
        }

        /// <summary>
        /// test if the timer has started
        /// </summary>
        [Test]
        public void TimerStartedTest()
        {
            Assert.AreNotEqual(0, StopwatchSingleton.Time);
        }
    }
}