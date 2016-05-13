using Autofac;
using Microsoft.Practices.Prism.Commands;
using NtErp.Shared.Services.Base;
using Prism.Events;
using Prism.Regions;
using System;
using System.Windows.Input;

namespace NtErp.Shared.Services.ViewModels {
    public abstract class SearchViewModel : CommonViewModel {
        #region CloseDialog Event Members

        public event EventHandler CloseDialogRequested;
        protected void RaiseCloseDialogRequested() => CloseDialogRequested?.Invoke(this, EventArgs.Empty);

        #endregion


        protected bool? _dialogResult;
        private EntityBase _selectedEntity;



        #region Commands

        private ICommand _selectCommand;
        private ICommand _searchCommand;
        private ICommand _resetCommand;
        private ICommand _cancelCommand;
        private ICommand _mouseDoubleClickCommand;

        public ICommand MouseDoubleClickCommand {
            get { return _mouseDoubleClickCommand ?? (_mouseDoubleClickCommand = new DelegateCommand(MouseDoubleClickCommand_OnExecute)); }
        }

        public ICommand SelectCommand {
            get { return _selectCommand ?? (_selectCommand = new DelegateCommand(SelectCommand_OnExecute)); }
        }

        public ICommand SearchCommand {
            get { return _searchCommand ?? (_searchCommand = new DelegateCommand(SearchCommand_OnExecute)); }
        }

        public ICommand ResetCommand {
            get { return _resetCommand ?? (_resetCommand = new DelegateCommand(ResetCommand_OnExecute)); }
        }

        public ICommand CancelCommand {
            get { return _cancelCommand ?? (_cancelCommand = new DelegateCommand(CancelCommand_OnExecute)); }
        }

        #endregion


        public EntityBase SelectedEntity {
            get { return _selectedEntity; }
            set { _selectedEntity = value; RaisePropertyChanged(); }
        }

        public bool? DialogResult {
            get { return _dialogResult; }
            set { _dialogResult = value; RaisePropertyChanged(); }
        }


        public SearchViewModel(ILifetimeScope scope, IEventAggregator eventAggregator, IRegionManager regionManager)
            : base(scope, eventAggregator, regionManager) {

        }


        protected abstract void MouseDoubleClickCommand_OnExecute();

        protected abstract void SelectCommand_OnExecute();

        protected abstract void SearchCommand_OnExecute();

        protected abstract void ResetCommand_OnExecute();

        protected abstract void CancelCommand_OnExecute();

    }
}
