using Autofac;
using NtErp.Modules.Finances.Views;
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

namespace NtErp.Modules.Finances.ViewModels {
    public class CashJournalViewModel : EntityViewModelBase, INavigationAware {
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
        private CashJournalEntry _selectedEntry;
        private IJournalBookRepository _cashJournalRepository;
        private IJournalEntryRepository _cashJournalEntryRepository;


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

        public CashJournalEntry SelectedEntry {
            get { return _selectedEntry; }
            set {
                _selectedEntry = value;
                RaisePropertyChanged();

                RefreshEnabledBindings();
            }
        }

        public bool CanCreateEntry {
            get { return HasRootEntity && RootEntity.Id > 0 && !RootEntity.HasChanges; }
        }

        public bool CanEditEntry {
            get { return HasRootEntity && SelectedEntry != null; }
        }

        public bool CanDeleteEntry {
            get { return HasRootEntity && SelectedEntry != null; }
        }

        #endregion

        #region Initialization

        public CashJournalViewModel(
            ILifetimeScope scope, IEventAggregator eventAggregator, IRegionManager regionManager,
            IJournalBookRepository cashJournalRepository,
            IJournalEntryRepository cashJournalEntryRepository,
            ITaxRateRepository taxRateRepository) {
            _scope = scope;
            _eventAggregator = eventAggregator;
            _regionManager = regionManager;
            _cashJournalRepository = cashJournalRepository;
            _cashJournalEntryRepository = cashJournalEntryRepository;
        }

        #endregion


        private void OpenJournalSearchCommand_OnExecute() {
            _eventAggregator.GetEvent<PubSubEvent<EntitySearchResultEvent>>()
                            .Subscribe(JournalSearch_OnReply);

            var searchWindow = _scope.Resolve<CashJournalSearchWindow>();
            searchWindow.ShowDialog();
        }

        private void JournalSearch_OnReply(EntitySearchResultEvent response) {
            _eventAggregator.GetEvent<PubSubEvent<EntitySearchResultEvent>>()
                            .Unsubscribe(JournalSearch_OnReply);

            if (response.DialogResult.Equals(true))
                RootEntity = _cashJournalRepository.Find(response.EntityId);
        }

        private void RefreshJournalCommand_OnExecute() {
            _cashJournalRepository.Refresh(RootEntity);
        }

        private void CreateJournalCommand_OnExecute() {
            RootEntity = _cashJournalRepository.New();
        }

        private void SaveJournalCommand_OnExecute() {
            _cashJournalRepository.Save(RootEntity);
        }

        private void DeleteJournalCommand_OnExecute() {
            _cashJournalRepository.Delete(RootEntity);
        }

        private void CreateEntryCommand_OnExecute() {
            var param = new NavigationParameters() {
                { ParameterNames.NextView, nameof(CashJournalView) }
            };

            NavigateToView(nameof(CashJournalEntryView), RegionNames.MainContent, param);
        }

        private void EditEntryCommand_OnExecute() {
            var param = new NavigationParameters() {
                { ParameterNames.Id,        SelectedEntry.Id },
                { ParameterNames.NextView,  nameof(CashJournalView) }
            };

            NavigateToView(nameof(CashJournalEntryView), RegionNames.MainContent, param);
        }

        private void DeleteEntryCommand_OnExecute() {
            _cashJournalEntryRepository.Delete(SelectedEntry);
        }


        protected override void RefreshEnabledBindings() {
            RaisePropertyChanged(nameof(HasRootEntity));
            RaisePropertyChanged(nameof(CanSave));
            RaisePropertyChanged(nameof(CanDelete));
            RaisePropertyChanged(nameof(CanRefresh));
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