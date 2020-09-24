using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SWP.Domain.Models.General;
using SWP.Domain.Models.Log;
using SWP.Domain.Models.SWPLegal;
using SWP.Domain.Models.SWPMedical;

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

        public DbSet<UserData> LicensedUsers { get; set; }
        public DbSet<LogRecord> LogRecords { get; set; }

        #endregion

        #region Medical SWP

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        #endregion

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Licenses

            modelBuilder.Entity<UserData>()
                .HasMany(c => c.Licenses)
                .WithOne(e => e.UserData)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserData>()
                .HasMany(c => c.LogRecords)
                .WithOne(e => e.UserData)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion

            #region Legal SWP

            modelBuilder.Entity<Client>()
                .HasMany(c => c.Cases)
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
                .HasMany(c => c.Notes)
                .WithOne(e => e.Case)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion

            #region Medical SWP

            modelBuilder.Entity<Patient>()
                .HasMany(c => c.Appointments)
                .WithOne(e => e.Patient)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion
        }
    }
}
