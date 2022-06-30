using System;

namespace actchargers
{
    /// <summary>
    /// Error Response event argument.
    /// </summary>
    public class ErrorResponseEventArg : EventArgs
    {
        public string ErrorData { get; set; }
    }
}