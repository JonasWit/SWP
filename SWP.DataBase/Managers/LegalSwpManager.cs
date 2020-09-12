using Microsoft.EntityFrameworkCore;
using SWP.Domain.Infrastructure;
using SWP.Domain.Models.SWPLegal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace SWP.DataBase.Managers
{
    public class LegalSwpManager : ILegalSwpManager
    {
        private readonly ApplicationDbContext context;
        public LegalSwpManager(ApplicationDbContext context) => this.context = context;

        #region Customer

        public TResult GetCustomer<TResult>(int id, string claim, Func<Customer, TResult> selector) =>
            context.Customers
                .Include(x => x.Cases)
                    .ThenInclude(y => y.Reminders)
                .Include(x => x.Cases)
                    .ThenInclude(y => y.Notes)
                .Include(x => x.Jobs)
                .Where(x => x.Id == id && x.ProfileClaim == claim)
                .Select(selector)
                .FirstOrDefault();

        public TResult GetCustomerWithoutCases<TResult>(int id, string claim, Func<Customer, TResult> selector) =>
            context.Customers
                .Include(x => x.Jobs)
                .Where(x => x.Id == id && x.ProfileClaim == claim)
                .Select(selector)
                .FirstOrDefault();

        public List<TResult> GetCustomers<TResult>(string claim, Func<Customer, TResult> selector) =>
            context.Customers
                .Include(x => x.Cases)
                    .ThenInclude(y => y.Reminders)
                .Include(x => x.Cases)
                    .ThenInclude(y => y.Notes)
                .Include(x => x.Jobs)
                .Where(x => x.ProfileClaim == claim)
                .Select(selector)
                .ToList();

        public List<Customer> GetCustomersWithoutCases(string claim) =>
            context.Customers
                .Include(x => x.Jobs)
                .Where(x => x.ProfileClaim == claim)
                .AsNoTracking()
                .ToList();

        public async Task<Customer> CreateCustomer(Customer customer)
        {
            context.Customers.Add(customer);
            await context.SaveChangesAsync();
            return customer;
        }

        public async Task<Customer> UpdateCustomer(Customer customer)
        {
            context.Customers.Update(customer);
            await context.SaveChangesAsync();
            return customer;
        }

        public Task<int> DeleteCustomer(int id)
        {
            context.Customers.Remove(context.Customers.FirstOrDefault(x => x.Id == id));
            return context.SaveChangesAsync();
        }

        public Task<int> DeleteProfileCustomers(string profileClaim)
        {
            var customers = GetCustomers(profileClaim, x => x.Id);
            context.Customers.RemoveRange(context.Customers.Where(x => customers.Contains(x.Id)));
            return context.SaveChangesAsync();
        }

        public int CountCustomers() => context.Customers.Count();

        #endregion

        #region Case

        public TResult GetCases<TResult>(string profile, Func<Case, TResult> selector)
        {
            throw new NotImplementedException();
        }

        public TResult GetCase<TResult>(int id, Func<Case, TResult> selector) =>
            context.Cases
                .Include(x => x.Notes)
                .Include(x => x.Reminders)
                .Where(x => x.Id == id)
                .Select(selector)
                .FirstOrDefault();

        public TResult GetCaseWithoutData<TResult>(int id, Func<Case, TResult> selector) =>
            context.Cases
                .Where(x => x.Id == id)
                .Select(selector)
                .FirstOrDefault();

        public Task<int> DeleteCase(int id)
        {
            var caseEntity = context.Cases.FirstOrDefault(x => x.Id == id);
            context.Cases.Remove(caseEntity);
            return context.SaveChangesAsync();
        }

        public async Task<Case> CreateCase(int customerId, string profile, Case c)
        {
            var customerEntity = GetCustomer(customerId, profile, x => x);
            customerEntity.Cases.Add(c);
            await context.SaveChangesAsync();
            return c;
        }

        public async Task<Case> UpdateCase(Case c)
        {
            context.Cases.Update(c);
            await context.SaveChangesAsync();
            return c;
        }

        public int CountCases(int customerId) => context.Cases.Count(x => x.CustomerId == customerId);

        public string GetCaseParentName(int id) => context.Customers.Where(x => x.Cases.Any(y => y.Id == id)).Select(x => x.Name).FirstOrDefault();

        public string GetCaseName(int id) => context.Cases.Where(x => x.Id == id).Select(y => y.Name).FirstOrDefault();

        #endregion

        #region Reminder

        public TResult GetReminder<TResult>(int id, Func<Reminder, TResult> selector) =>
            context.Reminders
                .Where(x => x.Id == id)
                .Select(selector)
                .FirstOrDefault();

        public List<Reminder> GetReminders(string profile) =>
            context.Customers
                .Where(x => x.ProfileClaim == profile)
                .Include(x => x.Cases)
                    .ThenInclude(y => y.Reminders)
                .SelectMany(x => x.Cases.SelectMany(x => x.Reminders))
                .ToList();

        public List<Reminder> GetRemindersForCustomer(int customerId) =>
            context.Customers
                .Where(x => x.Id == customerId)
                .Include(x => x.Cases)
                    .ThenInclude(y => y.Reminders)
                .SelectMany(x => x.Cases.SelectMany(x => x.Reminders))
                .ToList();

        public async Task<Reminder> CreateReminder(int caseId, Reminder reminder)
        {
            var caseEntity = GetCase(caseId, x => x);
            caseEntity.Reminders.Add(reminder);
            await context.SaveChangesAsync();
            return reminder;
        }

        public async Task<Reminder> UpdateReminder(Reminder reminder)
        {
            context.Reminders.Update(reminder);
            await context.SaveChangesAsync();
            return reminder;
        }

        public Task<int> DeleteReminder(int id)
        {
            var reminder = context.Reminders.FirstOrDefault(x => x.Id == id);
            context.Reminders.Remove(reminder);
            return context.SaveChangesAsync();
        }

        public Task<int> CountReminders()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Note

        public Note GetNote(int id) => context.Notes.FirstOrDefault(x => x.Id == id);

        public List<Note> GetNotesForCase(int caseId) =>
            context.Notes
                .Where(x => x.CaseId == caseId)
                .ToList();

        public async Task<Note> CreateNote(int caseId, Note note)
        {
            var caseEntity = GetCase(caseId, x => x);
            caseEntity.Notes.Add(note);
            await context.SaveChangesAsync();
            return note;
        }

        public async Task<Note> UpdateNote(Note note)
        {
            context.Notes.Update(note);
            await context.SaveChangesAsync();
            return note;
        }

        public Task<int> DeleteNote(int id)
        {
            var note = context.Notes.FirstOrDefault(x => x.Id == id);
            context.Notes.Remove(note);
            return context.SaveChangesAsync();
        }

        #endregion

        #region Job

        public Task<int> CreateCustomerJob(int customerId, string profile, CustomerJob job)
        {
            var cs = GetCustomer(customerId, profile, x => x);
            cs.Jobs.Add(job);
            return context.SaveChangesAsync();
        }

        public Task<int> DeleteCustomerJob(int id)
        {
            var job = GetCustomerJob(id, x => x);
            context.CustomerJobs.Remove(job);
            return context.SaveChangesAsync();
        }

        public TResult GetCustomerJob<TResult>(int id, Func<CustomerJob, TResult> selector) =>
            context.CustomerJobs
                .Where(x => x.Id == id)
                .Select(selector)
                .FirstOrDefault();

        public async Task<CustomerJob> UpdateCustomerJob(CustomerJob job)
        {
            context.CustomerJobs.Update(job);
            await context.SaveChangesAsync();
            return job;
        }

        #endregion

        #region Statistics

        public int CountCasesPerCustomer(int customerId) => 
            context.Cases.AsNoTracking().Count(x => x.CustomerId == customerId);

        public int CountJobsPerCustomer(int customerId) => 
            context.CustomerJobs.AsNoTracking().Count(x => x.CustomerId == customerId);

        public int CountRemindersPerCase(int caseId) => 
            context.Reminders.AsNoTracking()
                .Count(y => y.CaseId == caseId && y.End >= DateTime.Now);

        public int CountDeadlineRemindersPerCase(int caseId) => 
            context.Reminders.AsNoTracking()
                .Count(y => y.CaseId == caseId && y.End >= DateTime.Now && y.IsDeadline);

        public int CountNotesPerCase(int caseId) => 
            context.Notes.AsNoTracking().Count(x => x.CaseId == caseId);

        public IEnumerable<int> GetCustomerCasesIds(int customerId) =>
            context.Customers
                .AsNoTracking()
                .Include(x => x.Cases)
                .FirstOrDefault(x => x.Id == customerId).Cases
                .Select(y => y.Id)
                .ToList();

        #endregion
    }

}
