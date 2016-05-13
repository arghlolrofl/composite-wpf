using Autofac;
using Microsoft.Practices.Prism.Commands;
using NtErp.Shared.Services.Regions;
using NtErp.Shared.Services.ViewModels;
using NtErp.Shell.Demo.Views;
using Prism.Events;
using Prism.Regions;
using System.Windows.Input;

namespace NtErp.Shell.Demo.ViewModels {
    public class ShellViewModel : CommonViewModel {
        private ICommand _shutdownCommand;
        public ICommand ShutdownCommand {
            get {
                return _shutdownCommand ?? (_shutdownCommand = new DelegateCommand(ShutdownApplication));
            }
        }



        public ShellViewModel(ILifetimeScope scope, IEventAggregator eventAggregator, IRegionManager regionManager) : base(scope, eventAggregator, regionManager) {
            _regionManager.RegisterViewWithRegion(RegionNames.MainContent, typeof(StartView));
        }

        private void ShutdownApplication() {
            App.Current.Shutdown();
        }

    }
}
