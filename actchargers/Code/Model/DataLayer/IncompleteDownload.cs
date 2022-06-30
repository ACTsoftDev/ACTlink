namespace actchargers
{
    public class IncompleteDownload : DBModel
    {
        public string SerialNumber { get; set; }

        public byte Command { get; set; }

        public int LastSucceededIndex { get; set; }

        public override bool KeysEqual(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var i = (IncompleteDownload)obj;
            return Command == i.Command && SerialNumber.Equals(i.SerialNumber);
        }
    }
}
