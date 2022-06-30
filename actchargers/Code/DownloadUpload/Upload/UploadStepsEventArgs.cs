using System;

namespace actchargers
{
    public class UploadStepsEventArgs : EventArgs
    {
        public UploadSteps CurrentStep { get; set; }

        public float Progress { get; set; }
    }
}
