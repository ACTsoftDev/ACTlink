namespace actchargers
{
    public class CommandObject
    {
        public const int MAX_TRIALS = 3;

        public byte CommandBytes { get; set; }

        public byte[] Data { get; set; }

        public int ExpectedSize { get; set; }

        public bool VerifyExpectedSize { get; set; }

        public byte[] ResultArray { get; set; }

        public bool SaveLastSucceededIndex { get; set; }

        public int Trials { get; set; }

        public bool CanTry()
        {
            return Trials < MAX_TRIALS;
        }

        public void IncreaseTrials()
        {
            Trials++;
        }

        public void ResetTrials()
        {
            Trials = 0;
        }

        public CommandObject()
        {
            ResultArray = new byte[0];
        }
    }
}
