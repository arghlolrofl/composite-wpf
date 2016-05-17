using Autofac;
using Microsoft.Practices.Prism.Commands;
using NtErp.Modules.MasterFileData.ViewModels;
using NtErp.Modules.MasterFileData.Views;
using NtErp.Shared.Services.Regions;
using Prism.Events;
using Prism.Regions;
using System.Windows.Input;

namespace NtErp.Modules.MasterFileData {
    public class RibbonViewModel : NtErp.Shared.Services.ViewModels.RibbonViewModel {
        #region Commands

        private ICommand _GoToProductViewCommand;
        private ICommand _GoToTaxRateViewCommand;


        public ICommand GoToProductViewCommand {
            get { return _GoToProductViewCommand ?? (_GoToProductViewCommand = new DelegateCommand(GoToProductViewCommand_OnExecute)); }
        }

        public ICommand GoToTaxRateViewCommand {
            get { return _GoToTaxRateViewCommand ?? (_GoToTaxRateViewCommand = new DelegateCommand(GoToTaxRateViewCommand_OnExecute)); }
        }

        #endregion

        #region Properties

        private bool _isRibbonGroupVisibleProduct;
        private bool _isRibbonGroupVisibleTaxRate;


        public bool IsRibbonGroupVisibleProduct {
            get { return _isRibbonGroupVisibleProduct; }
            set {
                _isRibbonGroupVisibleProduct = value;
                RaisePropertyChanged();
            }
        }

        public bool IsRibbonGroupVisibleTaxRate {
            get { return _isRibbonGroupVisibleTaxRate; }
            set {
                _isRibbonGroupVisibleTaxRate = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        public RibbonViewModel(ILifetimeScope scope, IEventAggregator eventAggregator, IRegionManager regionManager)
            : base(scope, eventAggregator, regionManager) {

        }


        private void GoToProductViewCommand_OnExecute() {
            NavigateToView(nameof(ProductView), RegionNames.MainContent);
            ActiveViewModel = _scope.Resolve<ProductViewModel>();

            IsRibbonGroupVisibleProduct = true;
            IsRibbonGroupVisibleTaxRate = false;
        }

        private void GoToTaxRateViewCommand_OnExecute() {
            NavigateToView(nameof(TaxRateView), RegionNames.MainContent);
            ActiveViewModel = _scope.Resolve<TaxRateViewModel>();

            IsRibbonGroupVisibleProduct = false;
            IsRibbonGroupVisibleTaxRate = true;
        }
    }
}
