namespace actchargers
{
    public class FirmwareDownloaded : DBModel
    {
        public string Name { get; set; }

        public string Version { get; set; }

        public string FirmwareFile { get; set; }

        public override bool KeysEqual(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var i = (FirmwareDownloaded)obj;
            return Name == i.Name
                            && Version == i.Version;
        }
    }
}
