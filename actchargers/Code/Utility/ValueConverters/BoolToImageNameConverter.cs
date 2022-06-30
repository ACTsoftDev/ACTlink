namespace actchargers
{
    public static class BoolToImageNameConverter
    {
        const string CHECKED_IMAGE = "activeselect";
        const string UNCHECKED_IMAGE = "circle";

        public static string BoolToImageName(bool isChecked)
        {
            if (isChecked)
                return CHECKED_IMAGE;
            else
                return UNCHECKED_IMAGE;
        }
    }
}
