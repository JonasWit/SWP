using SWP.Domain.Models.SWPLegal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SWP.Domain.Infrastructure
{
    public interface ILegalSwpManager
    {
        #region Customers

        TResult GetCustomer<TResult>(int id, string claim, Func<Customer, TResult> selector);
        TResult GetCustomerWithoutCases<TResult>(int id, string claim, Func<Customer, TResult> selector);
        List<TResult> GetCustomers<TResult>(string claim, Func<Customer, TResult> selector);
        List<Customer> GetCustomersWithoutCases(string claim);

        Task<Customer> CreateCustomer(Customer customer);
        Task<Customer> UpdateCustomer(Customer customer);
        Task<int> DeleteCustomer(int id);
        Task<int> DeleteProfileCustomers(string profileClaim);

        #endregion

        #region Jobs

        Task<int> CreateCustomerJob(int customerId, string profile, CustomerJob job);
        Task<int> DeleteCustomerJob(int id);
        Task<int> UpdateCustomerJob(CustomerJob job);
        TResult GetCustomerJob<TResult>(int id, Func<CustomerJob, TResult> selector);

        #endregion

        #region Cases
        TResult GetCase<TResult>(int id, Func<Case, TResult> selector);
        TResult GetCaseWithoutData<TResult>(int id, Func<Case, TResult> selector);
        TResult GetCases<TResult>(string profile, Func<Case, TResult> selector);
        string GetCaseParentName(int id);
        string GetCaseName(int id);

        Task<Case> CreateCase(int customerId, string profile, Case c);
        Task<Case> UpdateCase(Case c);
        Task<int> DeleteCase(int id);
        int CountCases(int customerId);

        #endregion

        #region Reminders

        TResult GetReminder<TResult>(int id, Func<Reminder, TResult> selector);
        List<Reminder> GetReminders(string profile);
        List<Reminder> GetRemindersForCustomer(int customerId);

        Task<Reminder> CreateReminder(int caseId, Reminder reminder);
        Task<Reminder> UpdateReminder(Reminder reminder);
        Task<int> DeleteReminder(int id);

        Task<int> CountReminders();

        #endregion

        #region Notes

        Note GetNote(int id);
        List<Note> GetNotesForCase(int caseId);

        Task<int> CreateNote(int caseId, Note note);
        Task<int> UpdateNote(Note note);
        Task<int> DeleteNote(int id);

        #endregion

        #region Statistics

        int CountCustomers();
        int CountCasesPerCustomer(int customerId);
        int CountJobsPerCustomer(int customerId);
        IEnumerable<int> GetCustomerCasesIds(int customerId);

        int CountRemindersPerCase(int caseId);
        int CountDeadlineRemindersPerCase(int caseId);
        int CountNotesPerCase(int caseId);

        #endregion

    }
}
