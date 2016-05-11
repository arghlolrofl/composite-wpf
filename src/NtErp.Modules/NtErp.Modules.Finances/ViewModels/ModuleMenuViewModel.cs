using Autofac;
using NtErp.Modules.Finances.Views;
using NtErp.Shared.Services.Regions;
using NtErp.Shared.Services.ViewModels;
using Prism.Commands;
using Prism.Events;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Windows.Input;

namespace NtErp.Modules.Finances.ViewModels {
    public class ModuleMenuViewModel : MenuItemViewModel {
        private ICommand _goToCashJournalViewCommand;
        private IModuleManager _moduleManager;

        public ICommand GoToCashJournalViewCommand {
            get { return _goToCashJournalViewCommand ?? (_goToCashJournalViewCommand = new DelegateCommand(GoToCashJournalViewCommand_OnExecute)); }
        }

        public ModuleMenuViewModel(ILifetimeScope scope, IRegionManager regionManager, IModuleManager moduleManager, IEventAggregator eventAggregator)
            : base(scope, eventAggregator) {
            _regionManager = regionManager;
            _moduleManager = moduleManager;
        }

        private void GoToCashJournalViewCommand_OnExecute() {
            IRegion region = _regionManager.Regions[RegionNames.MainContent];
            Uri viewUri = new Uri(nameof(CashJournalView), UriKind.Relative);

            region.RequestNavigate(viewUri);
        }

    }
}