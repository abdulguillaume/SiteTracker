using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nigeria_Reg.Helpers
{
    public class GPSData
    {
        public static string ConvertDoubletoDMS(double coord)
        {
            int sec = (int)Math.Round(coord * 3600);
            int deg = sec / 3600;
            sec = Math.Abs(sec % 3600);
            int min = sec / 60;
            sec %= 60;

            return deg + "d" + min + "m" + sec + "s";

        }

        public static double ConvertDMSToDouble(string dms)
        {
            int i_d = dms.IndexOf('d');
            int i_m = dms.IndexOf('m');
            int i_s = dms.IndexOf('s');


            double degrees = double.Parse(string.IsNullOrEmpty(dms.Substring(0, i_d)) ? "0" : dms.Substring(0, i_d));
            double minutes = double.Parse(i_m == -1 ? "0" : (string.IsNullOrEmpty(dms.Substring(i_d + 1, i_m - i_d - 1)) ? "0" : dms.Substring(i_d + 1, i_m - i_d - 1)));
            double seconds = double.Parse(i_s == -1 ? "0" : (string.IsNullOrEmpty(dms.Substring(i_m + 1, i_s - i_m - 1)) ? "0" : dms.Substring(i_m + 1, i_s - i_m - 1)));


            //if (degrees == 0 && minutes == 0 && seconds == 0) return null;

            return degrees + (minutes / 60) + (seconds / 3600);
        }
    }
}