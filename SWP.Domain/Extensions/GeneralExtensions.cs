using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP.Domain.Extensions
{
    public static class GeneralExtensions
    {
        public static bool ContainsAny(this string inputString, params string[] lookupStrings) => lookupStrings.Any(inputString.Contains);
    }
}
