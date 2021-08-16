namespace Kfa.SubSystems.Cheques.Datalayer
{
    using Microsoft.EntityFrameworkCore;
    using MySqlConnector;
    using System;
    using System.Linq;

    namespace Src.Models
    {
        public class ChequesContext : DbContext
        {
            public ChequesContext()
            {
            }

            public virtual DbSet<ChequeRequisitionBatch> ChequeRequisitionBatches { get; set; }
            public virtual DbSet<CostCentre> CostCentres { get; set; }
            public virtual DbSet<GeneralLedgersDetail> GeneralLedgerDetails { get; set; }
            public virtual DbSet<LeasedPropertiesAccount> LeasedPropertiesAccounts { get; set; }
            public virtual DbSet<LedgerAccount> LedgerAccounts { get; set; }
            public virtual DbSet<PaidCheque> PaidCheques { get; set; }
            public virtual DbSet<Supplier> Suppliers { get; set; }
            public virtual DbSet<UserRole> UserRoles { get; set; }

            public ChequesContext(DbContextOptions<ChequesContext> options) : base(options)
            {
            }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                static MySqlConnection GetConnection()
                {
                    return new MySqlConnection(@"server=localhost;port=3306;database=kfa_db;user=root;password=;ConvertZeroDateTime=True");
                }

                var con = GetConnection();
                if (!optionsBuilder.IsConfigured)
                    optionsBuilder.UseMySql(con, ServerVersion.AutoDetect(con)).EnableSensitiveDataLogging();
            }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);
                modelBuilder.Model.GetEntityTypes()
                    .SelectMany(t => t.GetProperties())
                    .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?))
                    .Select(p => new Action(() => p.SetColumnType("decimal(18,6)")))
                    .ToList().ForEach(c => c());

                modelBuilder.Entity<CostCentre>()
                .HasMany(e => e.ChequeRequisitionBatches)
                .WithOne(e => e.CostCentre)
                .HasForeignKey(e => e.CostCentreCode);

                modelBuilder.Entity<CostCentre>()
                .HasMany(e => e.GeneralLedgersDetails)
                .WithOne(e => e.CostCentre)
                .HasForeignKey(e => e.CostCentreCode);

                modelBuilder.Entity<LedgerAccount>()
                .HasMany(e => e.GeneralLedgerDetails)
                .WithOne(e => e.LedgerAccount)
                .HasForeignKey(e => e.LedgerAccountId);

                modelBuilder.Entity<CostCentre>()
                .HasMany(e => e.Suppliers)
                .WithOne(e => e.CostCentre)
                .HasForeignKey(e => e.CostCentreCode);

                modelBuilder.Entity<GeneralLedgersDetail>()
                .HasMany(e => e.CreditPaidCheques)
                .WithOne(e => e.CreditGl)
                .HasForeignKey(e => e.PaidChequeCreditGlId);

                modelBuilder.Entity<GeneralLedgersDetail>()
                .HasMany(e => e.DebitPaidCheques)
                .WithOne(e => e.DebitGl)
                .HasForeignKey(e => e.PaidChequeDebitGlId);
            }
        }
    }
}