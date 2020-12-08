using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.Utilities
{
    public static class Extensions
    {
        public static string FormatPLN(this double input) => $"{input.ToString("C", CultureInfo.CreateSpecificCulture("pl"))}";


    }
}
