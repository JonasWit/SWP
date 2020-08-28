//using SWP.Application.LegalSwp.Cases;
//using SWP.Application.LegalSwp.Customers;
//using System.Collections.Generic;

//namespace SWP.Application.LegalSwp.Statistics
//{
//    [TransientService]
//    public class ProfileStatistics
//    {
//        private readonly GetCustomers getCustomers;
//        private readonly GetCases getCases;

//        public ProfileStatistics(GetCustomers getCustomers, GetCases getCases)
//        {
//            this.getCustomers = getCustomers;
//            this.getCases = getCases;
//        }

//        public Response Get([FromService] GetCustomers string profile)
//        {
//            var customers = getCustomers.GetCustomersWithoutData(profile);
//            var response = new Response();

//            foreach (var customer in customers)
//            {
//                var customerStatistic = new Response.CustomerStatistic();
//                var customerCases = getCustomers.GetCustomerCasesIds(customer.Id) ?? new List<int>();

//                customerStatistic.Jobs = getCustomers.CountJobsPerCustomer(customer.Id) ?? 0;

//                if (customerCases != null)
//                {
//                    foreach (var cs in customerCases)
//                    {
//                        customerStatistic.Cases.Add(new Response.CaseStatistic
//                        {
//                            Id = cs,
//                            Deadlines = getCases.CountDeadlineRemindersPerCase(cs) ?? 0,
//                            Reminders = getCases.CountRemindersPerCase(cs) ?? 0,
//                        });
//                    } 
//                }

//                response.Customers.Add(customerStatistic);
//            }

//            return response;
//        }

//        public class Response
//        {
//            public string ProfileName { get; set; }

//            public List<CustomerStatistic> Customers { get; set; } = new List<CustomerStatistic>();

//            public class CustomerStatistic
//            {
//                public int Id { get; set; }
//                public int Jobs { get; set; }
//                public List<CaseStatistic> Cases { get; set; } = new List<CaseStatistic>();
//            }

//            public class CaseStatistic
//            {
//                public int Id { get; set; }
//                public int Reminders { get; set; }
//                public int Deadlines { get; set; }
//            }
//        }
//    }
//}
