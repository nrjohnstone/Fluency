using System;

namespace Fluency
{
    public class FluencyException : Exception
    {
        public FluencyException(string message)
            : base(message) { }


        /// <summary>
        /// Initializes a new instance of the <see cref="FluencyException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public FluencyException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}