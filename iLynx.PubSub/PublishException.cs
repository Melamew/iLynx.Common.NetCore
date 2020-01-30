using System;

namespace iLynx.PubSub
{
    /// <summary>
    /// Represents an exception when attempting to publish a message
    /// </summary>
    public class PublishException : Exception
    {
        /// <summary>
        /// Initializes a new instance of <see cref="PublishException"/> with the specified error message
        /// </summary>
        /// <param name="message">The error message</param>
        public PublishException(string message) : base(message) { }
    }
}