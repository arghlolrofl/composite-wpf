using Autofac;
using NtErp.Shared.Services.Regions;
using NtErp.Shell.Demo.Views;
using Prism.Events;
using Prism.Regions;

namespace NtErp.Shell.Demo.ViewModels {
    public class ShellViewModel {
        private ILifetimeScope _scope;
        private IEventAggregator _eventAggregator;
        private IRegionManager _regionManager;

        public ShellViewModel(ILifetimeScope scope, IEventAggregator eventAggregator, IRegionManager regionManager) {
            _scope = scope;
            _eventAggregator = eventAggregator;
            _regionManager = regionManager;

            regionManager.RegisterViewWithRegion(RegionNames.MainContent, typeof(StartView));
        }

    }
}
