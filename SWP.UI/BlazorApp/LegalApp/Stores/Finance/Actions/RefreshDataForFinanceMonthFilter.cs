using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.Finance.Actions
{
    public class RefreshDataForFinanceMonthFilter : IAction
    {
        public const string RefreshDataForFinanceMonth = "REFRESH_DATA_FOR_FINANCE_MONTH_FILTER";
        public string Name => RefreshDataForFinanceMonth;
    }
}
