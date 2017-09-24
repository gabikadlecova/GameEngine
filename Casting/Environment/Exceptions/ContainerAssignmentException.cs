using System;

namespace Input.Exceptions
{
    /// <summary>
    /// The exception is thrown when there are multiple elements assigned to an index in the container
    /// </summary>
    class ContainerAssignmentException : Exception
    {
        public ContainerAssignmentException()
        {
        }

        public ContainerAssignmentException(string message) : base(message)
        {
        }

        public ContainerAssignmentException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
