using Autofac;
using Microsoft.Practices.ServiceLocation;
using NtErp.Shared.Contracts.Repository;
using NtErp.Shared.DataAccess;
using NtErp.Shared.Repositories;
using NtErp.Shared.Services.Events;
using NtErp.Shell.Demo.Views;
using Prism.Autofac;
using Prism.Events;
using Prism.Modularity;
using System;
using System.Windows;

namespace NtErp.Shell.Demo {
    public class Bootstrapper : AutofacBootstrapper {
        protected override IModuleCatalog CreateModuleCatalog() {
            return new ConfigurationModuleCatalog();
        }

        protected override void ConfigureModuleCatalog() {
            base.ConfigureModuleCatalog();

            Type moduleType = typeof(Modules.MasterFileData.MasterFileDataModule);
            ModuleCatalog.AddModule(
                new ModuleInfo() {
                    ModuleName = moduleType.Name,
                    ModuleType = moduleType.AssemblyQualifiedName,
                    InitializationMode = InitializationMode.WhenAvailable
                }
            );

            moduleType = typeof(Modules.Finances.FinancesModule);
            ModuleCatalog.AddModule(
                new ModuleInfo() {
                    ModuleName = moduleType.Name,
                    ModuleType = moduleType.AssemblyQualifiedName,
                    InitializationMode = InitializationMode.WhenAvailable
                }
            );



        }

        protected override void ConfigureContainerBuilder(ContainerBuilder builder) {
            base.ConfigureContainerBuilder(builder);

            builder.RegisterType<ShellView>();
            builder.RegisterType<NtErpContext>().SingleInstance();
            builder.RegisterType<ProductRepository>().As<IProductRepository>();
            builder.RegisterType<CashJournalRepository>().As<ICashJournalRepository>();
            builder.RegisterType<CashJournalEntryRepository>().As<ICashJournalEntryRepository>();
            builder.RegisterType<TaxRateRepository>().As<ITaxRateRepository>();
        }

        protected override DependencyObject CreateShell() {
            ServiceLocator.Current
                          .GetInstance<IEventAggregator>()
                          .GetEvent<PubSubEvent<ModuleLoadEvent>>()
                          .Subscribe(PrismModule_OnLoad);

            return ServiceLocator.Current
                                 .GetInstance<ShellView>();
        }

        protected override void InitializeShell() {
            Application.Current.MainWindow = (ShellView)Shell;
            Application.Current.MainWindow.Show();
        }

        /// <summary>
        /// When loading a prism module (IModule.Initialize), we need to update the IoC container too.
        /// The Autofac.Module will be passed inside the <see cref="ModuleLoadEvent"/> class.
        /// </summary>
        /// <param name="e">Class containing the module configuration to be updated.</param>
        private void PrismModule_OnLoad(ModuleLoadEvent e) {
            var updater = new ContainerBuilder();
            updater.RegisterModule(e.Module);
            updater.Update(Container);
        }
    }
}
