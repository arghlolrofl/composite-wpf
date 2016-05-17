using NtErp.Modules.Finances.Views;
using NtErp.Shared.Services.Modules;
using NtErp.Shared.Services.Regions;
using Prism.Events;
using Prism.Logging;
using Prism.Regions;

namespace NtErp.Modules.Finances {
    public class FinancesModule : ModuleBase {
        public FinancesModule(IRegionManager regionManager, IEventAggregator eventAggregator, ILoggerFacade logger)
            : base(logger, eventAggregator, regionManager) {

        }


        protected override void RegisterViews() {
            _regionManager.RegisterViewWithRegion(RegionNames.RibbonBar, typeof(RibbonView));

            _regionManager.RegisterViewWithRegion(RegionNames.MainContent, typeof(CashJournalView));
            _regionManager.RegisterViewWithRegion(RegionNames.MainContent, typeof(CashJournalEntryView));
        }
    }
}
