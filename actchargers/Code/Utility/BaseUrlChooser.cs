namespace actchargers
{
    public static class BaseUrlChooser
    {
        static readonly UrlSourceTypes urlType = UrlSourceTypes.AUTO;

        public static string ChooseCorrectBaseUrl()
        {
            string correctBaseUrl = ACConstants.PRODUCTION_BASE_URL;

            if (DevelopmentProfileHelper.IsDebuggingMode())
                correctBaseUrl = ACConstants.DEVELOPMENT_BASE_URL;

            switch (urlType)
            {
                case UrlSourceTypes.DEVELOPMENT:
                    correctBaseUrl = ACConstants.DEVELOPMENT_BASE_URL;

                    break;

                case UrlSourceTypes.PRODUCTION:
                    correctBaseUrl = ACConstants.PRODUCTION_BASE_URL;

                    break;
            }

            return correctBaseUrl;
        }

        public static bool IsDevelopmentBaseUrl()
        {
            string baseUrl = ChooseCorrectBaseUrl();

            return baseUrl.Equals(ACConstants.DEVELOPMENT_BASE_URL);
        }
    }

    enum UrlSourceTypes
    {
        AUTO,
        DEVELOPMENT,
        PRODUCTION
    }
}
