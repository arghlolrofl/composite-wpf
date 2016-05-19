using NtErp.Modules.Finances.Views;
using NtErp.Shared.Services.Constants;
using NtErp.Shared.Services.Modules;
using Prism.Events;
using Prism.Logging;
using Prism.Regions;

namespace NtErp.Modules.Finances {
  public class FinancesModule : ModuleBase {
    public FinancesModule(IRegionManager regionManager, IEventAggregator eventAggregator, ILoggerFacade logger)
        : base(logger, eventAggregator, regionManager) {

    }


    protected override void RegisterViews() {
      _regionManager.RegisterViewWithRegion(ShellRegions.Ribbon, typeof(FinancesModuleRibbon));
      _regionManager.RegisterViewWithRegion(ShellRegions.Ribbon, typeof(CashJournalViewRibbon));
      _regionManager.RegisterViewWithRegion(ShellRegions.Ribbon, typeof(CashJournalEntryViewRibbon));

      _regionManager.RegisterViewWithRegion(ShellRegions.MainContent, typeof(CashJournalView));
      _regionManager.RegisterViewWithRegion(ShellRegions.MainContent, typeof(CashJournalEntryView));
    }
  }
}
