using Autofac;
using NtErp.Modules.Finances.Views;
using NtErp.Shared.Services.Regions;
using Prism.Events;
using Prism.Modularity;
using Prism.Regions;

namespace NtErp.Modules.Finances {
    public class FinancesModule : IModule {
        private ILifetimeScope _scope;
        private IEventAggregator _eventAggregator;

        public IRegionManager RegionManager { get; set; }


        public FinancesModule(ILifetimeScope scope, IRegionManager regionManager, IEventAggregator eventAggregator) {
            _scope = scope;
            _eventAggregator = eventAggregator;
            RegionManager = regionManager;
        }

        public void Initialize() {
            RegionManager.RegisterViewWithRegion(RegionNames.ShellMenuBar, typeof(ModuleMenuView));
            RegionManager.RegisterViewWithRegion(RegionNames.MainContent, typeof(CashJournalView));
            RegionManager.RegisterViewWithRegion(RegionNames.MainContent, typeof(CashJournalEntryView));
        }

    }
}
