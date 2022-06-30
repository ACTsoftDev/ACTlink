namespace actchargers
{
    public static class DevelopmentProfileHelper
    {
        const bool IS_DEBUGGING_TESTING = true;
        const bool IS_EMULATOR = false;

        public static bool IsDebuggingMode()
        {
            return IS_DEBUGGING_TESTING && IsDevelopmentProfile();
        }

        public static bool IsEmulator()
        {
            return IS_EMULATOR;
        }

        public static bool IsDevelopmentProfile()
        {
            bool isDevelopmentProfile = false;

#if DEBUG
            isDevelopmentProfile = true;
#endif

            return isDevelopmentProfile;
        }
    }
}
