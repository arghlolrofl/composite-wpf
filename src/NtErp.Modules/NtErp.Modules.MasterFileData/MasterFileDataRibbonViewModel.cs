using Autofac;
using Microsoft.Practices.Prism.Commands;
using NtErp.Modules.MasterFileData.Views;
using NtErp.Shared.Services.Constants;
using Prism.Events;
using Prism.Regions;
using System.Windows.Input;

namespace NtErp.Modules.MasterFileData {
    public class MasterFileDataRibbonViewModel : NtErp.Shared.Services.ViewModels.RibbonViewModel {
        #region Commands

        private ICommand _GoToProductViewCommand;


        public ICommand GoToProductViewCommand {
            get { return _GoToProductViewCommand ?? (_GoToProductViewCommand = new DelegateCommand(GoToProductViewCommand_OnExecute)); }
        }

        #endregion

        public MasterFileDataRibbonViewModel(ILifetimeScope scope, IEventAggregator eventAggregator, IRegionManager regionManager)
            : base(scope, eventAggregator, regionManager) {

        }


        private void GoToProductViewCommand_OnExecute() {
            NavigateToView(nameof(ProductView), ShellRegions.MainContent);
        }
    }
}
