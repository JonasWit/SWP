using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SWP.Domain.Models.LegalApp;
using SWP.Domain.Models.News;
using SWP.Domain.Models.Portal;
using SWP.Domain.Models.Portal.Communication;

namespace SWP.DataBase
{
    public class AppContext : IdentityDbContext
    {
        #region Legal SWP

        public DbSet<Client> Clients { get; set; }
        public DbSet<Case> Cases { get; set; }
        public DbSet<ClientJob> ClientJobs { get; set; }
        public DbSet<TimeRecord> TimeRecords { get; set; }
        public DbSet<Reminder> Reminders { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<CashMovement> CashMovements { get; set; }
        public DbSet<ClientContactPerson> ClientContactPeople { get; set; }
        public DbSet<CaseContactPerson> CaseContactPeople { get; set; }

        #endregion

        #region Portal

        public DbSet<NewsRecord> News { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<BillingDetail> BillingDetails { get; set; }
        public DbSet<UserLicense> UserLicenses { get; set; }
        public DbSet<ClientRequest> ClientRequests { get; set; }
        public DbSet<ClientRequestMessage> ClientRequestMessages { get; set; }

        #endregion

        public AppContext(DbContextOptions<AppContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Legal App

            modelBuilder.Entity<Client>()
                .HasMany(c => c.Cases)
                .WithOne(e => e.Client)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Client>()
                .HasMany(c => c.ContactPeople)
                .WithOne(e => e.Client)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Client>()
                .HasMany(c => c.Jobs)
                .WithOne(e => e.Client)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Client>()
                .HasMany(c => c.TimeRecords)
                .WithOne(e => e.Client)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Client>()
                .HasMany(c => c.CashMovements)
                .WithOne(e => e.Client)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Case>()
                .HasMany(c => c.Reminders)
                .WithOne(e => e.Case)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Case>()
                .HasMany(c => c.ContactPeople)
                .WithOne(e => e.Case)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Case>()
                .HasMany(c => c.Notes)
                .WithOne(e => e.Case)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion

            #region Portal

            modelBuilder.Entity<ClientRequest>()
                .HasMany(c => c.Messages)
                .WithOne(e => e.ClientRequest)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Log>().ToTable("Log");

            #endregion
        }
    }
}
