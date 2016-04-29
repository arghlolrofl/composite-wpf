using Autofac;
using Microsoft.Win32;
using NtErp.Modules.Base.Views;
using NtErp.Shared.Entities.Base;
using NtErp.Shared.Entities.CashJournal;
using NtErp.Shared.Services.Contracts;
using NtErp.Shared.Services.Events;
using Prism.Commands;
using Prism.Events;
using System.IO;
using System.Windows.Input;

namespace NtErp.Modules.CashJournal.ViewModels {
    public class JournalBookViewModel : ViewModelBase {
        private ICommand _createJournalCommand;
        private ICommand _updateJournalCommand;
        private ICommand _deleteJournalCommand;
        private ICommand _refreshJournalCommand;
        private ICommand _openJournalSearchCommand;
        private ICommand _createEntryCommand;
        private ICommand _updateEntryCommand;
        private ICommand _deleteEntryCommand;
        private ICommand _selectAttachmentCommand;
        private JournalBook _selectedJournal;
        private JournalEntry _selectedJournalEntry;
        private ILifetimeScope _scope;
        private IEventAggregator _eventAggregator;
        private IJournalBookRepository _repository;


        #region Commands

        public ICommand RefreshJournalCommand {
            get { return _refreshJournalCommand ?? (_refreshJournalCommand = new DelegateCommand(RefreshJournalCommand_OnExecute)); }
        }

        public ICommand CreateJournalCommand {
            get { return _createJournalCommand ?? (_createJournalCommand = new DelegateCommand(CreateJournalCommand_OnExecute)); }
        }

        public ICommand UpdateJournalCommand {
            get { return _updateJournalCommand ?? (_updateJournalCommand = new DelegateCommand(UpdateJournalCommand_OnExecute)); }
        }

        public ICommand DeleteJournalCommand {
            get { return _deleteJournalCommand ?? (_deleteJournalCommand = new DelegateCommand(DeleteJournalCommand_OnExecute)); }
        }

        public ICommand OpenJournalSearchCommand {
            get { return _openJournalSearchCommand ?? (_openJournalSearchCommand = new DelegateCommand(OpenJournalSearchCommand_OnExecute)); }
        }

        public ICommand CreateEntryCommand {
            get { return _createEntryCommand ?? (_createEntryCommand = new DelegateCommand(CreateEntryCommand_OnExecute)); }
        }

        public ICommand UpdateEntryCommand {
            get { return _updateEntryCommand ?? (_updateEntryCommand = new DelegateCommand(UpdateEntryCommand_OnExecute)); }
        }

        public ICommand DeleteEntryCommand {
            get { return _deleteEntryCommand ?? (_deleteEntryCommand = new DelegateCommand(DeleteEntryCommand_OnExecute)); }
        }

        public ICommand SelectAttachmentCommand {
            get { return _selectAttachmentCommand ?? (_selectAttachmentCommand = new DelegateCommand(SelectAttachmentCommand_OnExecute)); }
        }

        #endregion

        #region View Bindings

        public JournalBook SelectedJournal {
            get { return _selectedJournal; }
            set {
                _selectedJournal = value;
                RaisePropertyChanged();

                evaluateEnabledBindings();
            }
        }

        public JournalEntry SelectedJournalEntry {
            get { return _selectedJournalEntry; }
            set {
                _selectedJournalEntry = value;
                RaisePropertyChanged();

                evaluateEnabledBindings();
            }
        }

        public bool HasRootEntity {
            get { return SelectedJournal != null; }
        }

        public bool CanUpdateJournal {
            get { return HasRootEntity; }
        }

        public bool CanDeleteJournal {
            get { return HasRootEntity; }
        }

        public bool CanRefreshJournal {
            get { return HasRootEntity; }
        }

        public bool CanCreateEntry {
            get { return HasRootEntity; }
        }

        public bool CanUpdateEntry {
            get { return HasRootEntity; }
        }

        public bool CanDeleteEntry {
            get { return HasRootEntity && SelectedJournalEntry != null; }
        }

        public bool CanAttachDocument {
            get { return HasRootEntity && SelectedJournalEntry != null; }
        }

        #endregion

        #region Initialization

        public JournalBookViewModel(ILifetimeScope scope, IEventAggregator eventAggregator, IJournalBookRepository repository) {
            _scope = scope;
            _eventAggregator = eventAggregator;
            _repository = repository;
        }

        #endregion

        #region Command Actions

        private void RefreshJournalCommand_OnExecute() {
            SelectedJournal = _repository.GetSingle(SelectedJournal.Id);
        }

        private void CreateJournalCommand_OnExecute() {
            SelectedJournal = _repository.NewJournal();
        }

        private void UpdateJournalCommand_OnExecute() {
            _repository.Save(SelectedJournal);
        }

        private void DeleteJournalCommand_OnExecute() {
            _repository.Delete(SelectedJournal);
            SelectedJournal = null;
        }

        private void OpenJournalSearchCommand_OnExecute() {
            _eventAggregator.GetEvent<PubSubEvent<EntitySearchResultEvent>>()
                            .Subscribe(JournalSearch_OnReply);

            var searchWindow = _scope.Resolve<JournalBookSearchWindow>();
            searchWindow.ShowDialog();
        }

        private void CreateEntryCommand_OnExecute() {
            SelectedJournalEntry = _repository.NewEntry(SelectedJournal);
        }

        private void UpdateEntryCommand_OnExecute() {
            _repository.UpdateEntry(SelectedJournalEntry);
        }

        private void DeleteEntryCommand_OnExecute() {
            _repository.DeleteEntry(SelectedJournalEntry);
        }

        private void SelectAttachmentCommand_OnExecute() {
            OpenFileDialog ofd = new OpenFileDialog() {
                CheckFileExists = true,
                CheckPathExists = true,
                Multiselect = false,
                Title = "Select attachment"
            };

            ofd.ShowDialog();

            if (string.IsNullOrEmpty(ofd.FileName))
                return;

            FileInfo f = new FileInfo(ofd.FileName);
            if (!f.Exists)
                throw new FileNotFoundException("File not found: " + f.FullName);

            SelectedJournalEntry.DocumentFolderPath = f.DirectoryName;
            SelectedJournalEntry.DocumentName = f.Name;
        }

        #endregion


        private void JournalSearch_OnReply(EntitySearchResultEvent response) {
            _eventAggregator.GetEvent<PubSubEvent<EntitySearchResultEvent>>()
                            .Unsubscribe(JournalSearch_OnReply);

            if (response.DialogResult.Equals(true)) {
                SelectedJournal = _repository.GetSingle(response.EntityId);
                //StatusText = "Selected Product: " + SelectedProduct.Number;
            }
        }

        private void evaluateEnabledBindings() {
            RaisePropertyChanged(nameof(HasRootEntity));
            RaisePropertyChanged(nameof(CanUpdateJournal));
            RaisePropertyChanged(nameof(CanDeleteJournal));
            RaisePropertyChanged(nameof(CanRefreshJournal));
            RaisePropertyChanged(nameof(CanCreateEntry));
            RaisePropertyChanged(nameof(CanUpdateEntry));
            RaisePropertyChanged(nameof(CanDeleteEntry));
            RaisePropertyChanged(nameof(CanAttachDocument));
        }
    }
}