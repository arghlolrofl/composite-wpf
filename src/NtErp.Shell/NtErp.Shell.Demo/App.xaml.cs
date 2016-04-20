using NtErp.Shared.DataAccess;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;

namespace NtErp.Shell.Demo {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);

            PrepareDataLayer();

            Bootstrapper bootstrap = new Bootstrapper();
            bootstrap.Run();
        }



        /// <summary>
        /// Uses context once while startup to initialize the database
        /// </summary>
        /// <returns>True, if no errors occured</returns>
        private bool PrepareDataLayer() {
            try {
                using (var context = new NtErpContext()) {
                    //context.Database.CreateIfNotExists();

                    //if (!context.Database.CompatibleWithModel(true))
                    //    Debug.WriteLine("Database model is not compatible with class structure!");

                    Debug.WriteLine("DATABASE");
                    //Debug.WriteLine("    [Entity] Products: " + context.Products.Count());
                    //Debug.WriteLine("    [Entity] LineItem: " + context.LineItems.Count());
                    Debug.WriteLine("    [Entity] Products: " + context.Products.Count());
                    //Debug.WriteLine("    [Entity] BillsOfMaterials: " + context.BillsOfMaterials.Count());
                }
            } catch (Exception ex) {
                Debug.WriteLine(ex.GetType().Name + ": " + ex.Message);
                throw;
            }

            return true;
        }
    }
}
