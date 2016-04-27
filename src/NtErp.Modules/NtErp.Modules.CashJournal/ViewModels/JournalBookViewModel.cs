using Autofac;
using NtErp.Modules.Base.Views;
using NtErp.Shared.Entities.Base;
using NtErp.Shared.Entities.CashJournal;
using NtErp.Shared.Services.Contracts;
using NtErp.Shared.Services.Events;
using Prism.Commands;
using Prism.Events;
using System.Windows.Input;

namespace NtErp.Modules.CashJournal.ViewModels {
    public class JournalBookViewModel : ViewModelBase {
        private ICommand _createJournalCommand;
        private ICommand _updateJournalCommand;
        private ICommand _deleteJournalCommand;
        private ICommand _refreshJournalCommand;
        private ICommand _openJournalSearchCommand;
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

        #endregion

        #region View Bindings

        public JournalBook SelectedJournal {
            get { return _selectedJournal; }
            set {
                _selectedJournal = value;
                RaisePropertyChanged();

                SelectedJournal.HasChanges = false;

                RaisePropertyChanged(nameof(HasRootEntity));
                RaisePropertyChanged(nameof(CanUpdateJournal));
                RaisePropertyChanged(nameof(CanDeleteJournal));
                RaisePropertyChanged(nameof(CanRefreshJournal));

                SelectedJournal.PropertyChanged += (sender, args) => {
                    if (args.PropertyName == nameof(SelectedJournal.HasChanges)) {
                        RaisePropertyChanged(nameof(CanUpdateJournal));
                    }
                };
            }
        }

        public JournalEntry SelectedJournalEntry {
            get { return _selectedJournalEntry; }
            set { _selectedJournalEntry = value; RaisePropertyChanged(); }
        }

        public bool HasRootEntity {
            get { return SelectedJournal != null; }
        }

        public bool CanUpdateJournal {
            get { return HasRootEntity && SelectedJournal.HasChanges; }
        }

        public bool CanDeleteJournal {
            get { return HasRootEntity; }
        }

        public bool CanRefreshJournal {
            get { return HasRootEntity; }
        }
        #endregion


        public JournalBookViewModel(ILifetimeScope scope, IEventAggregator eventAggregator, IJournalBookRepository repository) {
            _scope = scope;
            _eventAggregator = eventAggregator;
            _repository = repository;
        }


        private void RefreshJournalCommand_OnExecute() {
            SelectedJournal = _repository.GetSingle(SelectedJournal.Id);
        }

        private void CreateJournalCommand_OnExecute() {
            SelectedJournal = _repository.New();
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

        private void JournalSearch_OnReply(EntitySearchResultEvent response) {
            _eventAggregator.GetEvent<PubSubEvent<EntitySearchResultEvent>>()
                            .Unsubscribe(JournalSearch_OnReply);

            if (response.DialogResult.Equals(true)) {
                SelectedJournal = _repository.GetSingle(response.EntityId);
                //StatusText = "Selected Product: " + SelectedProduct.Number;
            }
        }
    }
}