using Autofac;
using Prism.Events;
using Prism.Regions;

namespace NtErp.Shared.Services.ViewModels {
    public class RibbonViewModel : CommonViewModel {
        #region Properties

        private EntityViewModel _activeViewModel;

        public EntityViewModel ActiveViewModel {
            get { return _activeViewModel; }
            set { _activeViewModel = value; RaisePropertyChanged(); }
        }

        #endregion


        public RibbonViewModel(ILifetimeScope scope, IEventAggregator eventAggregator, IRegionManager regionManager)
            : base(scope, eventAggregator, regionManager) {

        }
    }
}
