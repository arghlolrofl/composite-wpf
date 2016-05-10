using Autofac;
using NtErp.Modules.Finances.Views;
using NtErp.Shared.Services.Regions;
using NtErp.Shared.Services.ViewModels;
using Prism.Commands;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Windows.Input;

namespace NtErp.Modules.Finances.ViewModels {
    public class ModuleMenuViewModel : EntityViewModelBase {
        private ICommand _goToCashJournalViewCommand;
        private ILifetimeScope _scope;
        private IModuleManager _moduleManager;

        public ICommand GoToCashJournalViewCommand {
            get { return _goToCashJournalViewCommand ?? (_goToCashJournalViewCommand = new DelegateCommand(GoToCashJournalViewCommand_OnExecute)); }
        }

        public ModuleMenuViewModel(ILifetimeScope scope, IRegionManager regionManager, IModuleManager moduleManager) {
            _scope = scope;
            _regionManager = regionManager;
            _moduleManager = moduleManager;
        }

        private void GoToCashJournalViewCommand_OnExecute() {
            IRegion region = _regionManager.Regions[RegionNames.MainContent];
            Uri viewUri = new Uri(nameof(CashJournalView), UriKind.Relative);

            region.RequestNavigate(viewUri);
        }

        protected override void RefreshEnabledBindings() {

        }
    }
}