using NtErp.Modules.MasterFileData.Views;
using NtErp.Shared.Services.Constants;
using NtErp.Shared.Services.Modules;
using Prism.Events;
using Prism.Logging;
using Prism.Regions;

namespace NtErp.Modules.MasterFileData {

    public class MasterFileDataModule : ModuleBase {
        public MasterFileDataModule(IRegionManager regionManager, IEventAggregator eventAggregator, ILoggerFacade logger)
            : base(logger, eventAggregator, regionManager) {

        }

        protected override void RegisterViews() {
            _regionManager.RegisterViewWithRegion(ShellRegions.Ribbon, typeof(MasterFileDataModuleRibbon));
            _regionManager.RegisterViewWithRegion(ShellRegions.Ribbon, typeof(ProductViewRibbon));
            _regionManager.RegisterViewWithRegion(ShellRegions.Ribbon, typeof(TaxRateViewRibbon));

            _regionManager.RegisterViewWithRegion(ShellRegions.MainContent, typeof(ProductView));
            _regionManager.RegisterViewWithRegion(ShellRegions.MainContent, typeof(TaxRateView));
        }
    }
}