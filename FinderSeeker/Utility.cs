using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinderSeeker
{
    public class Utility
    {
        #region Constants.

        public const long KILOBYTE = 1024;
        public const long MEGABYTE = 1048576;
        public const long GIGABYTE = 1073741824;
        public const long TERABYTE = 1099511627776L;
        public const long PETABYTE = 1125899906842624L;
        public const long EXABYTE = 1152921504606847000L;

        public const long MEGAHERTZ = 1000000L;
        public const long GIGAHERTZ = 1000000000L;
        public const long TERAHERTZ = 1000000000000L;

        public const long KILOBIT = 1000L;
        public const long MEGABIT = 1000000L;
        public const long GIGIBIT = 1000000000L;
        public const long TERABIT = 1000000000000L;

        // Used for the charting to abbreviate axis label values
        public const long THOUSAND = 1000; // Thousand
        public const long MILLION = THOUSAND * 1000; // Million
        public const long BILLION = MILLION * 1000; // Billion
        public const long TRILLION = BILLION * 1000; // Trillion

        #endregion

        public static string FormatFileSize(ulong? fileSize, int decimalPlaces, bool singleCharacterSuffix = false)
        {
            if (fileSize == null)
            {
                return string.Empty;
            }
            double divideBy = 1;
            string suffix = "";
            bool negative = false;

            if (fileSize >= EXABYTE)
            {
                divideBy = EXABYTE;
                suffix = "EB";
            }
            else if (fileSize >= PETABYTE)
            {
                divideBy = PETABYTE;
                suffix = "PB";
            }
            else if (fileSize >= TERABYTE)
            {
                divideBy = TERABYTE;
                suffix = "TB";
            }
            else if (fileSize >= GIGABYTE)
            {
                divideBy = GIGABYTE;
                suffix = "GB";
            }
            else if (fileSize >= MEGABYTE)
            {
                divideBy = MEGABYTE;
                suffix = "MB";
            }
            else if (fileSize >= KILOBYTE)
            {
                divideBy = KILOBYTE;
                suffix = "KB";
            }
            else
            {
                divideBy = 1;
                suffix = "B";
            }

            if (singleCharacterSuffix)
            {
                suffix = suffix.Substring(0, 1);
            }

            double friendlyFileSize = ((double)fileSize) / ((double)divideBy);

            if (negative)
            {
                friendlyFileSize *= -1;
            }

            return friendlyFileSize.ToString("N" + decimalPlaces.ToString()) + " " + suffix;
        }
    }
}
