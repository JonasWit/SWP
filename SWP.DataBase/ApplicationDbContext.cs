using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SWP.Domain.Models.LegalApp;
using SWP.Domain.Models.Log;
using SWP.Domain.Models.News;
using SWP.Domain.Models.Portal;

namespace SWP.DataBase
{
    public class ApplicationDbContext : IdentityDbContext
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

        #endregion

        #region Portal

        public DbSet<LogRecord> LogRecords { get; set; }
        public DbSet<BillingDetail> BillingDetails { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<UserLicense> UserLicenses { get; set; }

        #endregion

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

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

            modelBuilder.Entity<BillingDetail>()
                .HasMany(c => c.UserLicenses)
                .WithOne(e => e.BillingDetail)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion
        }
    }
}
