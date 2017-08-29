using System;

namespace Casting.Environment.Exceptions
{
    class WallAssignmentException : Exception
    {
        public WallAssignmentException()
        {
        }

        public WallAssignmentException(string message) : base(message)
        {
        }

        public WallAssignmentException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
