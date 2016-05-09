// <copyright file="InvalidVectorSizeException.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace IRescue.Core.Datatypes
{
    using System;

    /// <summary>
    ///  Exception when the given parameters of the Vector was not 3.
    /// </summary>
    public class InvalidVectorSizeException : Exception
    {
        /// <summary>
        ///   Initializes a new instance of the InvalidVectorSizeException class with a message.
        /// </summary>
        /// <param name="message">Custom message for the Exception.</param>
        public InvalidVectorSizeException(string message) : base(message)
        {
        }
    }
}
