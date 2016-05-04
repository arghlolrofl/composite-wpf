using Autofac;
using NtErp.Modules.Base.Views;
using NtErp.Modules.CashJournal.Views;
using NtErp.Shared.Contracts.Repository;
using NtErp.Shared.Entities.CashJournal;
using NtErp.Shared.Services.Contracts;
using NtErp.Shared.Services.Events;
using NtErp.Shared.Services.Regions;
using NtErp.Shared.Services.ViewModels;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using System.Windows.Input;

namespace NtErp.Modules.CashJournal.ViewModels {
    public class JournalBookViewModel : ViewModelBase, INavigationAware {
        private ICommand _createJournalCommand;
        private ICommand _saveJournalCommand;
        private ICommand _deleteJournalCommand;
        private ICommand _refreshJournalCommand;
        private ICommand _openJournalSearchCommand;
        private ICommand _createEntryCommand;
        private ICommand _editEntryCommand;
        private ICommand _deleteEntryCommand;
        private ILifetimeScope _scope;
        private IEventAggregator _eventAggregator;
        private JournalBook _selectedJournal;
        private JournalEntry _selectedJournalEntry;
        private IJournalBookRepository _repository;
        private IJournalEntryRepository _entryRepository;


        #region Commands

        public ICommand OpenJournalSearchCommand {
            get { return _openJournalSearchCommand ?? (_openJournalSearchCommand = new DelegateCommand(OpenJournalSearchCommand_OnExecute)); }
        }

        public ICommand CreateJournalCommand {
            get { return _createJournalCommand ?? (_createJournalCommand = new DelegateCommand(CreateJournalCommand_OnExecute)); }
        }

        public ICommand RefreshJournalCommand {
            get { return _refreshJournalCommand ?? (_refreshJournalCommand = new DelegateCommand(RefreshJournalCommand_OnExecute)); }
        }

        public ICommand SaveJournalCommand {
            get { return _saveJournalCommand ?? (_saveJournalCommand = new DelegateCommand(SaveJournalCommand_OnExecute)); }
        }

        public ICommand DeleteJournalCommand {
            get { return _deleteJournalCommand ?? (_deleteJournalCommand = new DelegateCommand(DeleteJournalCommand_OnExecute)); }
        }

        public ICommand CreateEntryCommand {
            get { return _createEntryCommand ?? (_createEntryCommand = new DelegateCommand(CreateEntryCommand_OnExecute)); }
        }

        public ICommand EditEntryCommand {
            get { return _editEntryCommand ?? (_editEntryCommand = new DelegateCommand(EditEntryCommand_OnExecute)); }
        }

        public ICommand DeleteEntryCommand {
            get { return _deleteEntryCommand ?? (_deleteEntryCommand = new DelegateCommand(DeleteEntryCommand_OnExecute)); }
        }

        #endregion

        #region View Bindings

        public JournalBook SelectedJournal {
            get { return _selectedJournal; }
            set {
                _selectedJournal = value;

                raiseIsEnabledPropertyChanged();

                if (SelectedJournal != null)
                    SelectedJournal.PropertyChanged += (sender, args) => {
                        if (args.PropertyName == nameof(SelectedJournal.HasChanges))
                            RaisePropertyChanged(nameof(CanSaveJournal));
                    };
            }
        }

        public JournalEntry SelectedJournalEntry {
            get { return _selectedJournalEntry; }
            set {
                _selectedJournalEntry = value;
                RaisePropertyChanged();

                raiseIsEnabledPropertyChanged();
            }
        }

        public bool HasRootEntity {
            get { return SelectedJournal != null; }
        }

        public bool CanCreateJournal {
            get { return !HasRootEntity || (HasRootEntity && !SelectedJournal.HasChanges); }
        }

        public bool CanRefreshJournal {
            get { return HasRootEntity && SelectedJournal.Id > 0; }
        }

        public bool CanSaveJournal {
            get { return HasRootEntity && SelectedJournal.HasChanges; }
        }

        public bool CanDeleteJournal {
            get { return HasRootEntity && SelectedJournal.Id > 0; }
        }

        public bool CanCreateEntry {
            get { return HasRootEntity && SelectedJournal.Id > 0 && !SelectedJournal.HasChanges; }
        }

        public bool CanEditEntry {
            get { return HasRootEntity && SelectedJournalEntry != null; }
        }

        public bool CanDeleteEntry {
            get { return HasRootEntity && SelectedJournalEntry != null; }
        }

        #endregion

        #region Initialization

        public JournalBookViewModel(
            ILifetimeScope scope, IEventAggregator eventAggregator, IRegionManager regionManager,
            IJournalBookRepository repository, IJournalEntryRepository entryRepository, ITaxRateRepository taxRateRepository) {
            _scope = scope;
            _eventAggregator = eventAggregator;
            _regionManager = regionManager;
            _repository = repository;
            _entryRepository = entryRepository;
        }

        #endregion


        private void OpenJournalSearchCommand_OnExecute() {
            _eventAggregator.GetEvent<PubSubEvent<EntitySearchResultEvent>>()
                            .Subscribe(JournalSearch_OnReply);

            var searchWindow = _scope.Resolve<JournalBookSearchWindow>();
            searchWindow.ShowDialog();
        }

        private void RefreshJournalCommand_OnExecute() {
            _repository.Refresh(SelectedJournal);
        }

        private void CreateJournalCommand_OnExecute() {
            SelectedJournal = _repository.New();
        }

        private void SaveJournalCommand_OnExecute() {
            _repository.Save(SelectedJournal);
        }

        private void DeleteJournalCommand_OnExecute() {
            _repository.Delete(SelectedJournal);
            SelectedJournal = null;
        }

        private void CreateEntryCommand_OnExecute() {
            var param = new NavigationParameters() {
                { ParameterNames.NextView, nameof(JournalBookView) }
            };

            NavigateToView(nameof(JournalEntryView), RegionNames.MainContent, param);
        }

        private void EditEntryCommand_OnExecute() {
            var param = new NavigationParameters() {
                { ParameterNames.Id,        SelectedJournalEntry.Id },
                { ParameterNames.NextView,  nameof(JournalBookView) }
            };

            NavigateToView(nameof(JournalEntryView), RegionNames.MainContent, param);
        }

        private void DeleteEntryCommand_OnExecute() {
            _entryRepository.Delete(SelectedJournalEntry);
        }



        private void JournalSearch_OnReply(EntitySearchResultEvent response) {
            _eventAggregator.GetEvent<PubSubEvent<EntitySearchResultEvent>>()
                            .Unsubscribe(JournalSearch_OnReply);

            if (response.DialogResult.Equals(true)) {
                SelectedJournal = _repository.GetSingle(response.EntityId);
                //StatusText = "Selected Product: " + SelectedProduct.Number;
            }
        }

        private void raiseIsEnabledPropertyChanged() {
            RaisePropertyChanged(nameof(SelectedJournal));
            RaisePropertyChanged(nameof(HasRootEntity));
            RaisePropertyChanged(nameof(CanSaveJournal));
            RaisePropertyChanged(nameof(CanDeleteJournal));
            RaisePropertyChanged(nameof(CanRefreshJournal));
            RaisePropertyChanged(nameof(CanCreateEntry));
            RaisePropertyChanged(nameof(CanDeleteEntry));
            RaisePropertyChanged(nameof(CanEditEntry));
        }


        #region INavigationAware Members

        public void OnNavigatedTo(NavigationContext navigationContext) {

        }

        public bool IsNavigationTarget(NavigationContext navigationContext) {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext) {

        }

        #endregion
    }
}