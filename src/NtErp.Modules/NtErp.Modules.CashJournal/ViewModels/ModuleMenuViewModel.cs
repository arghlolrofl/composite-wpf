using Autofac;
using NtErp.Modules.CashJournal.Views;
using NtErp.Shared.Services.Regions;
using NtErp.Shared.Services.ViewModels;
using Prism.Commands;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Windows.Input;

namespace NtErp.Modules.CashJournal.ViewModels {
    public class ModuleMenuViewModel : ViewModelBase {
        private ICommand _goToJournalViewCommand;
        private ILifetimeScope _scope;
        private IModuleManager _moduleManager;

        public ICommand GoToJournalViewCommand {
            get { return _goToJournalViewCommand ?? (_goToJournalViewCommand = new DelegateCommand(GoToJournalViewCommand_OnExecute)); }
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

        protected override void RefreshEnabledBindings() {

        }
    }
}