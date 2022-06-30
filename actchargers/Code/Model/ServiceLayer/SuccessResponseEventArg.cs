using System;

namespace actchargers
{
    /// <summary>
    /// success Response event argument.
    /// </summary>
    public class SuccessResponseEventArg : EventArgs
    {
        public string Response { get; set; }

        public byte[] ResponseBytes { get; set; }
    }
}