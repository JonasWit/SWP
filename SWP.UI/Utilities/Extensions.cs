using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.Utilities
{
    public static class Extensions
    {
        public static string FormatPLN(this double input) => String.Format(CultureInfo.GetCultureInfo("pl"), "{0:C}", input);
    }
}
