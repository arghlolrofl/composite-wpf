using Autofac;
using Microsoft.Practices.ServiceLocation;
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
        }

        protected override DependencyObject CreateShell() {
            return ServiceLocator.Current.GetInstance<ShellView>();
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

            //moduleType = typeof(Modules.Navigation.NavigationModule);
            //ModuleCatalog.AddModule(
            //    new ModuleInfo() {
            //        ModuleName = moduleType.Name,
            //        ModuleType = moduleType.AssemblyQualifiedName,
            //        InitializationMode = InitializationMode.WhenAvailable
            //    }
            //);
        }
    }
}
