using NtErp.Data.Migrations;
using NtErp.Shared.Entities.Finances;
using NtErp.Shared.Entities.MasterFileData;
using System;
using System.Configuration;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;

namespace NtErp.Shared.DataAccess {
    public class NtErpContext : DbContext {
        const string NtErpConnectionStringName = "NtErpConnectionString";
        const string DataDirectoryKey = "DataDirectory";

        static string DatabasePath;

        #region Context Entities

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductComponent> Components { get; set; }
        public DbSet<CashJournal> CashJournals { get; set; }
        public DbSet<CashJournalEntry> CashJournalEntries { get; set; }
        public DbSet<CashJournalEntryPosition> CashJournalEntryPositions { get; set; }
        public DbSet<TaxRate> TaxRates { get; set; }

        #endregion

        /// <summary>
        /// Reads the database directory from the config file.
        /// </summary>
        static NtErpContext() {
            Debug.WriteLine("Reading database path from config file");
            DatabasePath = ConfigurationManager.AppSettings[DataDirectoryKey];
        }

        /// <summary>
        /// Initializes a new instance of the database context class.
        /// </summary>
        public NtErpContext() : base(NtErpConnectionStringName) {
            this.Configuration.AutoDetectChangesEnabled = false;

            string path = AppDomain.CurrentDomain.GetData(DataDirectoryKey) as String;
            if (String.IsNullOrEmpty(path)) {
                if (DatabasePath.StartsWith("%")) {
                    DatabasePath = Environment.ExpandEnvironmentVariables(DatabasePath);

                    if (DatabasePath.StartsWith("%"))
                        throw new ArgumentException("Invalid database path detected: " + DatabasePath);

                    DirectoryInfo dir = new DirectoryInfo(DatabasePath);
                    if (!dir.Exists)
                        dir.Create();
                }

                // Set |DataDirectory| value
                AppDomain.CurrentDomain.SetData(DataDirectoryKey, DatabasePath);
                Debug.WriteLine("DataDirectory set to: " + DatabasePath);
            }

            Database.SetInitializer(new MigrateDatabaseToLatestVersion<NtErpContext, Migrations.Configuration>(NtErpConnectionStringName));
        }

        /// <summary>
        /// Configures entities when building the databse model from POCO's.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            modelBuilder.Configurations.Add(new ProductComponentConfiguration());

            modelBuilder.Configurations.Add(new ProductConfiguration());

            base.OnModelCreating(modelBuilder);

        }
    }
}