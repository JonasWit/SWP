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

        #region Client

        public TResult GetClient<TResult>(int id, string profile, Func<Client, TResult> selector) =>
            context.Clients
                .Include(x => x.Cases)
                    .ThenInclude(y => y.Reminders)
                .Include(x => x.Cases)
                    .ThenInclude(y => y.Notes)
                .Include(x => x.Jobs)
                .Include(x => x.CashMovements)
                .Where(x => x.Id == id && x.ProfileClaim == profile)
                .Select(selector)
                .FirstOrDefault();

        public TResult GetClientWithoutCases<TResult>(int id, string profile, Func<Client, TResult> selector) =>
            context.Clients
                .Include(x => x.Jobs)
                .Where(x => x.Id == id && x.ProfileClaim == profile)
                .Select(selector)
                .FirstOrDefault();

        public List<TResult> GetClients<TResult>(string profile, Func<Client, TResult> selector) =>
            context.Clients
                .Include(x => x.Cases)
                    .ThenInclude(y => y.Reminders)
                .Include(x => x.Cases)
                    .ThenInclude(y => y.Notes)
                .Include(x => x.Jobs)
                .Where(x => x.ProfileClaim == profile)
                .Select(selector)
                .ToList();

        public List<Client> GetClientsWithoutCases(string profile) =>
            context.Clients
                .Include(x => x.Jobs)
                .Where(x => x.ProfileClaim == profile)
                .AsNoTracking()
                .ToList();

        public async Task<Client> CreateClient(Client Client)
        {
            context.Clients.Add(Client);
            await context.SaveChangesAsync();
            return Client;
        }

        public async Task<Client> UpdateClient(Client Client)
        {
            context.Clients.Update(Client);
            await context.SaveChangesAsync();
            return Client;
        }

        public Task<int> DeleteClient(int id)
        {
            context.Clients.Remove(context.Clients.FirstOrDefault(x => x.Id == id));
            return context.SaveChangesAsync();
        }

        public Task<int> DeleteProfileClients(string profile)
        {
            var Clients = GetClients(profile, x => x.Id);
            context.Clients.RemoveRange(context.Clients.Where(x => Clients.Contains(x.Id)));
            return context.SaveChangesAsync();
        }

        public int CountClients() => context.Clients.Count();

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

        public async Task<Case> CreateCase(int ClientId, string profile, Case c)
        {
            var ClientEntity = GetClient(ClientId, profile, x => x);
            ClientEntity.Cases.Add(c);
            await context.SaveChangesAsync();
            return c;
        }

        public async Task<Case> UpdateCase(Case c)
        {
            context.Cases.Update(c);
            await context.SaveChangesAsync();
            return c;
        }

        public int CountCases(int ClientId) => context.Cases.Count(x => x.ClientId == ClientId);

        public string GetCaseParentName(int id) => context.Clients.Where(x => x.Cases.Any(y => y.Id == id)).Select(x => x.Name).FirstOrDefault();

        public string GetCaseName(int id) => context.Cases.Where(x => x.Id == id).Select(y => y.Name).FirstOrDefault();

        #endregion

        #region Reminder

        public TResult GetReminder<TResult>(int id, Func<Reminder, TResult> selector) =>
            context.Reminders
                .Where(x => x.Id == id)
                .Select(selector)
                .FirstOrDefault();

        public List<Reminder> GetReminders(string profile) =>
            context.Clients
                .Where(x => x.ProfileClaim == profile)
                .Include(x => x.Cases)
                    .ThenInclude(y => y.Reminders)
                .SelectMany(x => x.Cases.SelectMany(x => x.Reminders))
                .ToList();

        public List<Reminder> GetRemindersForClient(int ClientId) =>
            context.Clients
                .Where(x => x.Id == ClientId)
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

        public async Task<ClientJob> CreateClientJob(int ClientId, string profile, ClientJob job)
        {
            var cs = GetClient(ClientId, profile, x => x);
            cs.Jobs.Add(job);
            await context.SaveChangesAsync();
            return job;
        }

        public Task<int> DeleteClientJob(int id)
        {
            var job = GetClientJob(id, x => x);
            context.ClientJobs.Remove(job);
            return context.SaveChangesAsync();
        }

        public TResult GetClientJob<TResult>(int id, Func<ClientJob, TResult> selector) =>
            context.ClientJobs
                .Where(x => x.Id == id)
                .Select(selector)
                .FirstOrDefault();

        public async Task<ClientJob> UpdateClientJob(ClientJob job)
        {
            context.ClientJobs.Update(job);
            await context.SaveChangesAsync();
            return job;
        }

        #endregion

        #region Cash Movements

        public CashMovement GetCashMovement(int id) => context.CashMovements.FirstOrDefault(x => x.Id == id);

        public List<CashMovement> GetCashMovementsForClient(int ClientId) =>
            context.CashMovements
                .Where(x => x.ClientId == ClientId)
                .ToList();

        public async Task<CashMovement> CreateCashMovement(int ClientId, CashMovement cashMovement)
        {
            var cs = context.Clients.FirstOrDefault(x =>x.Id == ClientId);
            cs.CashMovements.Add(cashMovement);
            await context.SaveChangesAsync();
            return cashMovement;
        }

        public async Task<CashMovement> UpdateCashMovement(CashMovement cashMovement)
        {
            context.CashMovements.Update(cashMovement);
            await context.SaveChangesAsync();
            return cashMovement;
        }

        public Task<int> DeleteCashMovement(int id)
        {
            var cm = context.CashMovements.FirstOrDefault(x => x.Id == id);
            context.CashMovements.Remove(cm);
            return context.SaveChangesAsync();
        }

        #endregion

        #region Statistics

        public int CountCasesPerClient(int ClientId) => 
            context.Cases.AsNoTracking().Count(x => x.ClientId == ClientId);

        public int CountJobsPerClient(int ClientId) => 
            context.ClientJobs.AsNoTracking().Count(x => x.ClientId == ClientId);

        public int CountRemindersPerCase(int caseId) => 
            context.Reminders.AsNoTracking()
                .Count(y => y.CaseId == caseId && y.End >= DateTime.Now);

        public int CountDeadlineRemindersPerCase(int caseId) => 
            context.Reminders.AsNoTracking()
                .Count(y => y.CaseId == caseId && y.End >= DateTime.Now && y.IsDeadline);

        public int CountNotesPerCase(int caseId) => 
            context.Notes.AsNoTracking().Count(x => x.CaseId == caseId);

        public IEnumerable<int> GetClientCasesIds(int ClientId) =>
            context.Clients
                .AsNoTracking()
                .Include(x => x.Cases)
                .FirstOrDefault(x => x.Id == ClientId).Cases
                .Select(y => y.Id)
                .ToList();

        #endregion
    }

}
