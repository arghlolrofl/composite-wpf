using Autofac;
using Microsoft.Practices.ServiceLocation;
using NtErp.Shared.Contracts.Repository;
using NtErp.Shared.DataAccess;
using NtErp.Shared.Repositories;
using NtErp.Shared.Services.Events;
using NtErp.Shell.Demo.ViewModels;
using NtErp.Shell.Demo.Views;
using Prism.Autofac;
using Prism.Events;
using Prism.Modularity;
using Prism.Regions;
using Prism.RibbonRegionAdapter;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls.Ribbon;

namespace NtErp.Shell.Demo {
    public class Bootstrapper : AutofacBootstrapper {
        protected override IModuleCatalog CreateModuleCatalog() {
            return new ConfigurationModuleCatalog();
        }

        /// <summary>
        /// Configures the module catalog for the application
        /// </summary>
        protected override void ConfigureModuleCatalog() {
            base.ConfigureModuleCatalog();

            // Modules that should be loaded ...
            IEnumerable<Type> modules = new Type[] {
                typeof(Modules.MasterFileData.MasterFileDataModule),
                typeof(Modules.Finances.FinancesModule),
                //typeof(Modules.HumanResources.HumanResourcesModule)
            };

            // ... are added to the catalog here
            foreach (Type module in modules) {
                ModuleCatalog.AddModule(new ModuleInfo() {
                    ModuleName = module.Name,
                    ModuleType = module.AssemblyQualifiedName,
                    InitializationMode = InitializationMode.WhenAvailable
                });
            }
        }

        protected override void ConfigureContainerBuilder(ContainerBuilder builder) {
            base.ConfigureContainerBuilder(builder);

            builder.RegisterType<RibbonRegionAdapter>();
            builder.RegisterType<ShellView>();
            builder.RegisterType<ShellViewModel>().SingleInstance();
            builder.RegisterType<NtErpContext>().SingleInstance();
            builder.RegisterType<ProductRepository>().As<IProductRepository>();
            builder.RegisterType<CashJournalRepository>().As<ICashJournalRepository>();
            builder.RegisterType<CashJournalEntryRepository>().As<ICashJournalEntryRepository>();
            builder.RegisterType<TaxRateRepository>().As<ITaxRateRepository>();
        }

        protected override RegionAdapterMappings ConfigureRegionAdapterMappings() {
            var mappings = base.ConfigureRegionAdapterMappings();

            IRegionAdapter adapter = ServiceLocator.Current.GetInstance<RibbonRegionAdapter>();
            mappings.RegisterMapping(typeof(Ribbon), adapter);

            return mappings;
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
