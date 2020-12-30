using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.Utilities
{
    public static class LogTags
    {
        public const string PortalIdentityLogPrefix = "[From Identity] ";
        public const string LegalAppLogPrefix = "[From Legal App] ";
        public const string LegalAppErrorLogPrefix = "[Exception From Legal App] ";
        public const string PortalAppLogPrefix = "[From Portal App] ";
        public const string PortalAppErrorLogPrefix = "[Exception From Portal App] ";
        public const string AdminAppLogPrefix = "[From Admin App] ";
        public const string AdminAppErrorLogPrefix = "[Exception From Admin App] ";
        public const string AutomationLogPrefix = "[Log From Automation] ";
    }
}
