using Autofac;
using Prism.Events;
using Prism.Regions;

namespace NtErp.Shared.Services.ViewModels {
    public class RibbonViewModel : CommonViewModel {
        public RibbonViewModel(ILifetimeScope scope, IEventAggregator eventAggregator, IRegionManager regionManager)
            : base(scope, eventAggregator, regionManager) {

        }
    }
}
