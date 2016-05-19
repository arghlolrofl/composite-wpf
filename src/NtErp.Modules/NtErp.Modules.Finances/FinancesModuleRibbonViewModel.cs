﻿using Autofac;
using NtErp.Modules.Finances.Views;
using NtErp.Shared.Services.Constants;
using NtErp.Shared.Services.ViewModels;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using System.Windows.Input;

namespace NtErp.Modules.Finances {
    public class FinancesModuleRibbonViewModel : RibbonViewModel {
        #region Commands

        private ICommand _goToCashJournalViewCommand;

        public ICommand GoToCashJournalViewCommand {
            get { return _goToCashJournalViewCommand ?? (_goToCashJournalViewCommand = new DelegateCommand(GoToCashJournalViewCommand_OnExecute)); }
        }

        #endregion

        #region Properties

        private bool _isRibbonGroupVisibleCashJournal;
        public bool IsRibbonGroupVisibleCashJournal {
            get { return _isRibbonGroupVisibleCashJournal; }
            set { _isRibbonGroupVisibleCashJournal = value; RaisePropertyChanged(); }
        }

        #endregion

        public FinancesModuleRibbonViewModel(ILifetimeScope scope, IRegionManager regionManager, IEventAggregator eventAggregator)
            : base(scope, eventAggregator, regionManager) {

        }

        private void GoToCashJournalViewCommand_OnExecute() {
            NavigateToView(nameof(CashJournalView), ShellRegions.MainContent);
        }
    }
}