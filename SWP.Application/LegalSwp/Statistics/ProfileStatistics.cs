using SWP.Domain.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace SWP.Application.LegalSwp.Statistics
{
    [TransientService]
    public class ProfileStatistics
    {
        private readonly ILegalSwpManager legalSwpManager;
        public ProfileStatistics(ILegalSwpManager legalSwpManager) => this.legalSwpManager = legalSwpManager;

        public Response CountCasesPerCustomer(string claim)
        {
            var customersWithData = legalSwpManager.GetCustomers(claim, x => x);





            return new Response();
        }

        public class Response
        {
            public string ProfileName { get; set; }

            public List<StatisticRecord> CustomerCases { get; set; }
            public Dictionary<string, StatisticRecord> CustomerJobs { get; set; }

            public class StatisticRecord
            { 
                public string ItemName { get; set; }
                public int Count { get; set; }                       
            }




        }



    }
}
