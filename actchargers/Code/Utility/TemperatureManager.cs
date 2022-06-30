namespace actchargers
{
    public static class TemperatureManager
    {
        public static string GetFormattedCorrectTemperature(double sourceTemperature, bool isFahrenheit)
        {
            double correctTemperature = GetCorrectTemperature(sourceTemperature, isFahrenheit);
            string mark = GetCorrectMark(isFahrenheit);

            return string.Format("{0:N0}, {1}", correctTemperature, mark);
        }

        public static double GetCorrectTemperature(double sourceTemperature, bool isFahrenheit)
        {
            return isFahrenheit ? CelsiusToFahrenheit(sourceTemperature) : sourceTemperature;
        }

        public static string GetCorrectMark(bool isFahrenheit)
        {
            return isFahrenheit ? AppResources.f_chart : AppResources.c_chart;
        }

        public static double CelsiusToFahrenheit(double celsius)
        {
            double fahrenheit = (9.0 / 5.0) * celsius + 32;

            return fahrenheit;
        }

        public static double FahrenheitToCelsius(double fahrenheit)
        {
            double celsius = (5.0 / 9.0) * (fahrenheit - 32);

            return celsius;
        }
    }
}
