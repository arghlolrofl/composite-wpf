using NtErp.Modules.MasterFileData.Views;
using NtErp.Shared.Services.Modules;
using NtErp.Shared.Services.Regions;
using Prism.Events;
using Prism.Logging;
using Prism.Regions;

namespace NtErp.Modules.MasterFileData {

    public class MasterFileDataModule : ModuleBase {
        public MasterFileDataModule(IRegionManager regionManager, IEventAggregator eventAggregator, ILoggerFacade logger)
            : base(logger, eventAggregator, regionManager) {

        }

        protected override void RegisterViews() {
            _regionManager.RegisterViewWithRegion(RegionNames.RibbonBar, typeof(RibbonView));

            _regionManager.RegisterViewWithRegion(RegionNames.MainContent, typeof(ProductView));
            _regionManager.RegisterViewWithRegion(RegionNames.MainContent, typeof(TaxRateView));
        }
    }
}