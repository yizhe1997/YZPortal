﻿namespace YZPortal.Core.Error
{
    public class SyncException : Exception
    {
        public SyncException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
