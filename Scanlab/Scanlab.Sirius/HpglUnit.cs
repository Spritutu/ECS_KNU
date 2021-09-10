namespace Scanlab.Sirius
{
    internal static class HpglUnit
    {
        public static double PluToMm(double value) => value / 40.0;

        public static string PluToReadableString(double value)
        {
            double mm = HpglUnit.PluToMm(value);
            double num = 1.0;
            string str = "mm";
            if (mm > 1000000.0)
            {
                num = 1000000.0;
                str = "km";
            }
            else if (mm > 1000.0)
            {
                num = 1000.0;
                str = "m";
            }
            else if (mm > 100.0)
            {
                num = 10.0;
                str = "cm";
            }
            return string.Format("{0:0.##} {1}", (object)(mm / num), (object)str);
        }
    }
}