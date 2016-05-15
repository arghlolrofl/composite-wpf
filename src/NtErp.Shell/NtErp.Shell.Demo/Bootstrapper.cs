using Autofac;
using Microsoft.Practices.ServiceLocation;
using NtErp.Modules.Base;
using NtErp.Modules.Finances;
using NtErp.Shared.Contracts.Repository;
using NtErp.Shared.DataAccess;
using NtErp.Shared.Repositories;
using NtErp.Shell.Demo.Views;
using Prism.Autofac;
using Prism.Modularity;
using System;
using System.Windows;

namespace NtErp.Shell.Demo {
  public class Bootstrapper : AutofacBootstrapper {
    protected override void ConfigureContainerBuilder(ContainerBuilder builder) {
      base.ConfigureContainerBuilder(builder);

      builder.RegisterType<ShellView>();
      builder.RegisterType<NtErpContext>().SingleInstance();
      builder.RegisterType<ProductRepository>().As<IProductRepository>();
      builder.RegisterType<CashJournalRepository>().As<ICashJournalRepository>();
      builder.RegisterType<CashJournalEntryRepository>().As<ICashJournalEntryRepository>();
      builder.RegisterType<TaxRateRepository>().As<ITaxRateRepository>();

      // register autofac module
      builder.RegisterModule<BaseModuleConfiguration>();
      builder.RegisterModule<FinancesModuleConfiguration>();
    }

    protected override DependencyObject CreateShell() {
      return ServiceLocator.Current
                           .GetInstance<ShellView>();
    }

    protected override void InitializeShell() {
      Application.Current.MainWindow = (ShellView)Shell;
      Application.Current.MainWindow.Show();
    }

    protected override IModuleCatalog CreateModuleCatalog() {
      return new ConfigurationModuleCatalog();
    }

    protected override void ConfigureModuleCatalog() {
      base.ConfigureModuleCatalog();

      Type moduleType = typeof(Modules.Base.BaseModule);
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
  }
}
