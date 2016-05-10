using NtErp.Shared.Contracts.Repository;
using NtErp.Shared.Entities.CashJournal;
using NtErp.Shared.Services.Events;
using NtErp.Shared.Services.ViewModels;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace NtErp.Modules.Finances.ViewModels {
    public class CashJournalSearchViewModel : ViewModelBase {
        #region Commands

        private ICommand _selectCommand;
        private ICommand _searchCommand;
        private ICommand _resetCommand;
        private ICommand _cancelCommand;


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


        private bool? _dialogResult;
        private IJournalBookRepository _repository;
        private IEventAggregator _eventAggregator;
        private CashJournal _selectedJournal;
        private ObservableCollection<CashJournal> _journals = new ObservableCollection<CashJournal>();


        #region Properties

        public bool? DialogResult {
            get { return _dialogResult; }
            set { _dialogResult = value; RaisePropertyChanged(); }
        }

        public ObservableCollection<CashJournal> Journals {
            get { return _journals; }
            set { _journals = value; RaisePropertyChanged(); }
        }

        public CashJournal SelectedJournal {
            get { return _selectedJournal; }
            set { _selectedJournal = value; RaisePropertyChanged(); }
        }

        #endregion


        #region Initialization

        public CashJournalSearchViewModel(IJournalBookRepository repository, IEventAggregator eventAggregator) {
            _repository = repository;
            _eventAggregator = eventAggregator;

            RefreshJournals();
        }

        #endregion


        private void SelectCommand_OnExecute() {
            if (SelectedJournal != null) {
                DialogResult = true;
                SendResponseAndRequestCloseDialog();
            }
        }

        private void SearchCommand_OnExecute() {
            RefreshJournals();
        }

        private void ResetCommand_OnExecute() {
            throw new NotImplementedException();
        }

        private void CancelCommand_OnExecute() {
            DialogResult = false;
            SendResponseAndRequestCloseDialog();
        }

        private void RefreshJournals() {
            Journals.Clear();
            Journals.AddRange(_repository.Fetch());
        }

        private void SendResponseAndRequestCloseDialog() {
            _eventAggregator.GetEvent<PubSubEvent<EntitySearchResultEvent>>()
                            .Publish(new EntitySearchResultEvent(SelectedJournal.Id, DialogResult));

            RaiseCloseDialogRequested();
        }

        protected override void RefreshEnabledBindings() {

        }
    }
}