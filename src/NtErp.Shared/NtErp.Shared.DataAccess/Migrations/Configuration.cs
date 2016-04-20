using NtErp.Shared.Entities.MasterFileData;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.Linq;

namespace NtErp.Shared.DataAccess.Migrations {
    internal sealed class Configuration : DbMigrationsConfiguration<NtErpContext> {
        const string AllowDataLossOnMigrationConfigKeyName = "AllowDataLossOnMigration";

        public Configuration() {
            AutomaticMigrationsEnabled = true;

            string allowDataLossOnMigration = ConfigurationManager.AppSettings[AllowDataLossOnMigrationConfigKeyName] as string;

            if (allowDataLossOnMigration.ToLower() == "true")
                AutomaticMigrationDataLossAllowed = true;
            else
                AutomaticMigrationDataLossAllowed = false;
        }

        protected override void Seed(NtErpContext context) {
            Debug.WriteLine(" > Seeding data into database ...");
            SeedProducts(context);
            SeedKits(context);
        }

        private void SeedProducts(NtErpContext context) {
            IList<Product> productsToSeed = new List<Product>() {
                new Product { Name = "Screw (small)", Description = "Kleine Schraube", Number = "S-K-123", Version = 1 },
                new Product { Name = "Screw (medium)", Description = "Mittelgroße Schraube", Number = "S-M-123", Version = 1 },
                new Product { Name = "Screw (large)", Description = "Große Schraube", Number = "S-G-123", Version = 1 },
                new Product { Name = "Fan", Description = "Propeller", Number = "E-F-123", Version = 1 },
                new Product { Name = "Compressor", Description = "Verdichter", Number = "E-C-123", Version = 1 },
                new Product { Name = "Burning Chamber", Description = "Brennkammer", Number = "E-B-123", Version = 1 },
                new Product { Name = "Turbine", Description = "Turbine", Number = "E-T-123", Version = 1 },
                new Product { Name = "Jet Nozzle", Description = "Schubdüse", Number = "E-N-123", Version = 1 }
            };

            foreach (var component in productsToSeed) {
                try {
                    context.Products.AddOrUpdate(c => c.Name, component);
                } catch (Exception ex) {
                    Debug.WriteLine("SEED ERROR:");
                    Debug.WriteLine("    " + ex.GetType().Name.ToUpper() + ": " + ex.Message);
                    continue;
                }
            }
        }

        private void SeedKits(NtErpContext context) {
            Product component = context.Products.Local.First();
            Kit testKit1 = new Kit() { Name = "TestKit", Description = "Test-Baugruppe", Number = "KIT-123-456", Version = 1 };

            testKit1.Components.Add(new ProductComponent() { Kit = testKit1, Component = component, Amount = 1 });

            IList<Kit> kitsToSeed = new List<Kit>() {
                testKit1
            };

            foreach (var kit in kitsToSeed) {
                try {
                    context.Kits.AddOrUpdate(c => c.Name, kit);
                } catch (Exception ex) {
                    Debug.WriteLine("SEED ERROR:");
                    Debug.WriteLine("    " + ex.GetType().Name.ToUpper() + ": " + ex.Message);
                    continue;
                }
            }
        }
    }
}
