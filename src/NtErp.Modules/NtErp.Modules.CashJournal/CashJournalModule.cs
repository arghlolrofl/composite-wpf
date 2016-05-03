using Autofac;
using NtErp.Modules.CashJournal.Views;
using NtErp.Shared.Services.Regions;
using Prism.Events;
using Prism.Modularity;
using Prism.Regions;

namespace NtErp.Modules.CashJournal {
    public class CashJournalModule : IModule {
        private ILifetimeScope _scope;
        private IEventAggregator _eventAggregator;

        public IRegionManager RegionManager { get; set; }


        public CashJournalModule(ILifetimeScope scope, IRegionManager regionManager, IEventAggregator eventAggregator) {
            _scope = scope;
            _eventAggregator = eventAggregator;
            RegionManager = regionManager;
        }

        public void Initialize() {
            RegionManager.RegisterViewWithRegion(RegionNames.ShellMenuBar, typeof(ModuleMenuView));
            RegionManager.RegisterViewWithRegion(RegionNames.MainContent, typeof(JournalBookView));
            RegionManager.RegisterViewWithRegion(RegionNames.MainContent, typeof(JournalEntryView));
            RegionManager.RegisterViewWithRegion(RegionNames.MainContent, typeof(TaxRateView));
        }
    }
}
