using Microsoft.EntityFrameworkCore;
using SWP.Domain.Infrastructure;
using SWP.Domain.Models.SWPLegal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.DataBase.Managers
{
    public class LegalSwpManager : ILegalSwpManager
    {
        private readonly ApplicationDbContext _context;
        public LegalSwpManager(ApplicationDbContext context) => _context = context;

        #region Customer

        public TResult GetCustomer<TResult>(int id, string claim, Func<Customer, TResult> selector) =>
            _context.Customers
                .Include(x => x.Cases)
                    .ThenInclude(y => y.Reminders)
                .Include(x => x.Cases)
                    .ThenInclude(y => y.Notes)
                .Include(x => x.Jobs)
                .Where(x => x.Id == id && x.ProfileClaim == claim)
                .Select(selector)
                .FirstOrDefault();

        public TResult GetCustomerWithoutCases<TResult>(int id, string claim, Func<Customer, TResult> selector) =>
            _context.Customers
                .Include(x => x.Jobs)
                .Where(x => x.Id == id && x.ProfileClaim == claim)
                .Select(selector)
                .FirstOrDefault();

        public List<TResult> GetCustomers<TResult>(string claim, Func<Customer, TResult> selector) =>
            _context.Customers
                .Include(x => x.Cases)
                    .ThenInclude(y => y.Reminders)
                .Include(x => x.Cases)
                    .ThenInclude(y => y.Notes)
                .Include(x => x.Jobs)
                .Where(x => x.ProfileClaim == claim)
                .Select(selector)
                .ToList();

        public List<TResult> GetCustomersWithoutCases<TResult>(string claim, Func<Customer, TResult> selector) =>
            _context.Customers
                .Include(x => x.Jobs)
                .Where(x => x.ProfileClaim == claim)
                .Select(selector)
                .ToList();

        public Task<int> CreateCustomer(Customer customer)
        {
            _context.Customers.Add(customer);
            return _context.SaveChangesAsync();
        }

        public Task<int> UpdateCustomer(Customer customer)
        {
            _context.Customers.Update(customer);
            return _context.SaveChangesAsync();
        }

        public Task<int> DeleteCustomer(int id)
        {
            _context.Customers.Remove(_context.Customers.FirstOrDefault(x => x.Id == id));
            return _context.SaveChangesAsync();
        }

        public Task<int> DeleteProfileCustomers(string profileClaim)
        {
            var customers = GetCustomers(profileClaim, x => x.Id);
            _context.Customers.RemoveRange(_context.Customers.Where(x => customers.Contains(x.Id)));
            return _context.SaveChangesAsync();
        }

        public int CountCustomers() => _context.Customers.Count();

        #endregion

        #region Case

        public TResult GetCases<TResult>(string profile, Func<Case, TResult> selector)
        {
            throw new NotImplementedException();
        }

        public TResult GetCase<TResult>(int id, Func<Case, TResult> selector) =>
            _context.Cases
                .Include(x => x.Notes)
                .Include(x => x.Reminders)
                .Where(x => x.Id == id)
                .Select(selector)
                .FirstOrDefault();

        public TResult GetCaseWithoutData<TResult>(int id, Func<Case, TResult> selector) =>
            _context.Cases
                .Where(x => x.Id == id)
                .Select(selector)
                .FirstOrDefault();

        public Task<int> DeleteCase(int id)
        {
            var caseEntity = _context.Cases.FirstOrDefault(x => x.Id == id);
            _context.Cases.Remove(caseEntity);
            return _context.SaveChangesAsync();
        }

        public Task<int> CreateCase(int customerId, string profile, Case c)
        {
            var customerEntity = GetCustomer(customerId, profile, x => x);
            customerEntity.Cases.Add(c);
            return _context.SaveChangesAsync();
        }

        public Task<int> UpdateCase(Case c)
        {
            _context.Cases.Update(c);
            return _context.SaveChangesAsync();
        }

        public int CountCases(int customerId) => _context.Cases.Count(x => x.CustomerId == customerId);

        #endregion

        #region Reminder

        public TResult GetReminder<TResult>(int id, Func<Reminder, TResult> selector) =>
            _context.Reminders
                .Where(x => x.Id == id)
                .Select(selector)
                .FirstOrDefault();

        public List<Reminder> GetReminders(string profile) =>
            _context.Customers
                .Where(x => x.ProfileClaim == profile)
                .Include(x => x.Cases)
                    .ThenInclude(y => y.Reminders)
                .SelectMany(x => x.Cases.SelectMany(x => x.Reminders))
                .ToList();

        public List<Reminder> GetRemindersForCustomer(int customerId) =>
            _context.Customers
                .Where(x => x.Id == customerId)
                .Include(x => x.Cases)
                    .ThenInclude(y => y.Reminders)
                .SelectMany(x => x.Cases.SelectMany(x => x.Reminders))
                .ToList();

        public Task<int> CreateReminder(int caseId, Reminder reminder)
        {
            var caseEntity = GetCase(caseId, x => x);
            caseEntity.Reminders.Add(reminder);
            return _context.SaveChangesAsync();
        }

        public Task<int> UpdateReminder(Reminder reminder)
        {
            _context.Reminders.Update(reminder);
            return _context.SaveChangesAsync();
        }

        public Task<int> DeleteReminder(int id)
        {
            var reminder = _context.Reminders.FirstOrDefault(x => x.Id == id);
            _context.Reminders.Remove(reminder);
            return _context.SaveChangesAsync();
        }

        public Task<int> CountReminders()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Note

        public Note GetNote(int id) => _context.Notes.FirstOrDefault(x => x.Id == id);

        public List<Note> GetNotesForCase(int caseId) =>
            _context.Notes
                .Where(x => x.CaseId == caseId)
                .ToList();

        public Task<int> CreateNote(int caseId, Note note)
        {
            var caseEntity = GetCase(caseId, x => x);
            caseEntity.Notes.Add(note);
            return _context.SaveChangesAsync();
        }

        public Task<int> UpdateNote(Note note)
        {
            _context.Notes.Update(note);
            return _context.SaveChangesAsync();
        }

        public Task<int> DeleteNote(int id)
        {
            var note = _context.Notes.FirstOrDefault(x => x.Id == id);
            _context.Notes.Remove(note);
            return _context.SaveChangesAsync();
        }

        #endregion

        #region Job

        public Task<int> CreateCustomerJob(int customerId, string profile, CustomerJob job)
        {
            var cs = GetCustomer(customerId, profile, x => x);
            cs.Jobs.Add(job);
            return _context.SaveChangesAsync();
        }

        public Task<int> DeleteCustomerJob(int id)
        {
            var job = GetCustomerJob(id, x => x);
            _context.CustomerJobs.Remove(job);
            return _context.SaveChangesAsync();
        }

        public TResult GetCustomerJob<TResult>(int id, Func<CustomerJob, TResult> selector) =>
            _context.CustomerJobs
                .Where(x => x.Id == id)
                .Select(selector)
                .FirstOrDefault();

        public Task<int> UpdateCustomerJob(CustomerJob job)
        {
            _context.CustomerJobs.Update(job);
            return _context.SaveChangesAsync();
        }


        #endregion



    }

}
