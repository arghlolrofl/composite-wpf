using Autofac;
using NtErp.Modules.Finances.Views;
using NtErp.Shared.Services.Regions;
using NtErp.Shared.Services.ViewModels;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using System.Windows.Input;

namespace NtErp.Modules.Finances {
    public class FinancesRibbonViewModel : RibbonViewModel {
        private ICommand _goToCashJournalViewCommand;

        public ICommand GoToCashJournalViewCommand {
            get { return _goToCashJournalViewCommand ?? (_goToCashJournalViewCommand = new DelegateCommand(GoToCashJournalViewCommand_OnExecute)); }
        }

        public FinancesRibbonViewModel(ILifetimeScope scope, IRegionManager regionManager, IEventAggregator eventAggregator)
            : base(scope, eventAggregator, regionManager) {

        }

        private void GoToCashJournalViewCommand_OnExecute() {
            // switch view in main content region
            NavigateToView(nameof(CashJournalView), RegionNames.MainContent);
        }

    }
}