using NtErp.Shared.Services.Modules;
using Prism.Events;
using Prism.Logging;
using Prism.Regions;

namespace NtErp.Modules.HumanResources {
    public class HumanResourcesModule : ModuleBase {
        public HumanResourcesModule(IRegionManager regionManager, IEventAggregator eventAggregator, ILoggerFacade logger)
            : base(logger, eventAggregator, regionManager) {

        }

        protected override void RegisterViews() {
            //_regionManager.RegisterViewWithRegion(ShellRegions.Ribbon, typeof(HumanResourcesModuleRibbon));

            //_regionManager.RegisterViewWithRegion(ShellRegions.MainContent, typeof(ProductView));
            //_regionManager.RegisterViewWithRegion(ShellRegions.MainContent, typeof(TaxRateView));
        }
    }
}
