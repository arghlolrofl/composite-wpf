using Autofac;
using NtErp.Modules.Base.Views;
using NtErp.Shared.Services.Regions;
using Prism.Events;
using Prism.Modularity;
using Prism.Regions;

namespace NtErp.Modules.Base {

    public class BaseModule : IModule {
        private ILifetimeScope _scope;
        private IEventAggregator _eventAggregator;

        public IRegionManager RegionManager { get; set; }

        public BaseModule(ILifetimeScope scope, IRegionManager regionManager, IEventAggregator eventAggregator) {
            _scope = scope;
            _eventAggregator = eventAggregator;
            RegionManager = regionManager;
        }

        public void Initialize() {
            RegionManager.RegisterViewWithRegion(RegionNames.ShellMenuBar, typeof(ModuleMenuView));
            RegionManager.RegisterViewWithRegion(RegionNames.MainContent, typeof(ProductView));
            RegionManager.RegisterViewWithRegion(RegionNames.MainContent, typeof(TaxRateView));
        }
    }
}