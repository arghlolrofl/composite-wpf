using Autofac;
using Microsoft.Practices.Prism.Commands;
using NtErp.Shared.Services.Regions;
using NtErp.Shell.Demo.Views;
using Prism.Events;
using Prism.Regions;
using System.Windows.Input;

namespace NtErp.Shell.Demo.ViewModels {
    public class ShellViewModel {
        private ILifetimeScope _scope;
        private IEventAggregator _eventAggregator;
        private IRegionManager _regionManager;

        private ICommand _shutdownCommand;
        public ICommand ShutdownCommand {
            get {
                return _shutdownCommand ?? (_shutdownCommand = new DelegateCommand(ShutdownApplication));
            }
        }

        public ShellViewModel(ILifetimeScope scope, IEventAggregator eventAggregator, IRegionManager regionManager) {
            _scope = scope;
            _eventAggregator = eventAggregator;
            _regionManager = regionManager;

            regionManager.RegisterViewWithRegion(RegionNames.MainContent, typeof(StartView));
        }

        private void ShutdownApplication() {
            App.Current.Shutdown();
        }

    }
}
