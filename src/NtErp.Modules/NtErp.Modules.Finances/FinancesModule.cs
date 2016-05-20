using NtErp.Modules.Finances.Views;
using NtErp.Shared.Services.Constants;
using NtErp.Shared.Services.Modules;
using Prism.Events;
using Prism.Logging;
using Prism.Regions;

namespace NtErp.Modules.Finances {
    /// <summary>
    /// Prism module for Finances.
    /// </summary>
    public class FinancesModule : ModuleBase {
        /// <summary>
        /// Creates a new instance of the FinancesModule.
        /// </summary>
        /// <param name="regionManager">Prism's RegionManager</param>
        /// <param name="eventAggregator">Prism's EventAggregator</param>
        /// <param name="logger">Prism's default logger</param>
        public FinancesModule(IRegionManager regionManager, IEventAggregator eventAggregator, ILoggerFacade logger)
            : base(logger, eventAggregator, regionManager) {

        }

        ///<inheritdoc />
        protected override void RegisterViews() {
            _regionManager.RegisterViewWithRegion(ShellRegions.Ribbon, typeof(FinancesModuleRibbon));
            _regionManager.RegisterViewWithRegion(ShellRegions.Ribbon, typeof(TaxRateViewRibbon));
            _regionManager.RegisterViewWithRegion(ShellRegions.Ribbon, typeof(CashJournalViewRibbon));
            _regionManager.RegisterViewWithRegion(ShellRegions.Ribbon, typeof(CashJournalEntryViewRibbon));

            _regionManager.RegisterViewWithRegion(ShellRegions.MainContent, typeof(TaxRateView));
            _regionManager.RegisterViewWithRegion(ShellRegions.MainContent, typeof(CashJournalView));
            _regionManager.RegisterViewWithRegion(ShellRegions.MainContent, typeof(CashJournalEntryView));
        }
    }
}
