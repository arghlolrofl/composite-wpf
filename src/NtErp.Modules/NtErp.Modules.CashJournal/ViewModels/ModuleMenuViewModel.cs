using Autofac;
using NtErp.Modules.CashJournal.Views;
using NtErp.Shared.Entities.Base;
using NtErp.Shared.Services.Regions;
using Prism.Commands;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Windows.Input;

namespace NtErp.Modules.CashJournal.ViewModels {
    public class ModuleMenuViewModel : ViewModelBase {
        private ICommand _goToJournalViewCommand;
        private ICommand _goToTaxRateViewCommand;
        private IRegionManager _regionManager;
        private ILifetimeScope _scope;
        private IModuleManager _moduleManager;

        public ICommand GoToJournalViewCommand {
            get { return _goToJournalViewCommand ?? (_goToJournalViewCommand = new DelegateCommand(GoToJournalViewCommand_OnExecute)); }
        }

        public ICommand GoToTaxRateViewCommand {
            get { return _goToTaxRateViewCommand ?? (_goToTaxRateViewCommand = new DelegateCommand(GoToTaxRateViewCommand_OnExecute)); }
        }

        public ModuleMenuViewModel(ILifetimeScope scope, IRegionManager regionManager, IModuleManager moduleManager) {
            _scope = scope;
            _regionManager = regionManager;
            _moduleManager = moduleManager;
        }

        private void GoToJournalViewCommand_OnExecute() {
            IRegion region = _regionManager.Regions[RegionNames.MainContent];
            Uri viewUri = new Uri(nameof(JournalBookView), UriKind.Relative);

            region.RequestNavigate(viewUri);
        }

        private void GoToTaxRateViewCommand_OnExecute() {
            IRegion region = _regionManager.Regions[RegionNames.MainContent];
            Uri viewUri = new Uri(nameof(TaxRateView), UriKind.Relative);

            region.RequestNavigate(viewUri);
        }
    }
}