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
using System.Windows;
using System.Windows.Input;

namespace NtErp.Modules.Finances.ViewModels {
    public class CashJournalViewModel : EntityViewModel, INavigationAware {
        #region INavigationAware Members

        public void OnNavigatedTo(NavigationContext navigationContext) {
            IsViewActive = Visibility.Visible;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext) {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext) {
            IsViewActive = Visibility.Collapsed;
        }

        #endregion

        #region Fields

        private CashJournalEntry _selectedEntry;
        private ICashJournalRepository _cashJournalRepository;
        private ICashJournalEntryRepository _cashJournalEntryRepository;
        private RibbonViewModel _Ribbon;

        #endregion

        #region Commands

        private ICommand _createEntryCommand;
        private ICommand _editEntryCommand;
        private ICommand _deleteEntryCommand;

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

        #region Properties

        private Visibility _isViewActive = Visibility.Collapsed;
        public Visibility IsViewActive {
            get { return _isViewActive; }
            set {
                _isViewActive = value;
                RaisePropertyChanged();
            }
        }

        public CashJournalEntry SelectedEntry {
            get { return _selectedEntry; }
            set {
                _selectedEntry = value;
                RaisePropertyChanged();

                RefreshEnabledBindings();
            }
        }

        public bool CanCreateEntry {
            get { return HasRootEntity && RootEntity.Exists && !RootEntity.HasChanges; }
        }

        public bool CanEditEntry {
            get { return HasRootEntity && SelectedEntry != null; }
        }

        public bool CanDeleteEntry {
            get { return HasRootEntity && SelectedEntry != null; }
        }

        public RibbonViewModel Ribbon {
            get { return _Ribbon; }
            set { _Ribbon = value; RaisePropertyChanged(); }
        }


        #endregion

        #region Initialization

        public CashJournalViewModel(
            ILifetimeScope scope, IEventAggregator eventAggregator, IRegionManager regionManager,
            ICashJournalRepository cashJournalRepository,
            ICashJournalEntryRepository cashJournalEntryRepository,
            ITaxRateRepository taxRateRepository)
            : base(scope, eventAggregator, regionManager) {
            _cashJournalRepository = cashJournalRepository;
            _cashJournalEntryRepository = cashJournalEntryRepository;

            Ribbon = _scope.Resolve<FinancesRibbonViewModel>();
        }

        #endregion


        protected override void OpenSearchCommand_OnExecute() {
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

        protected override void RefreshCommand_OnExecute() {
            _cashJournalRepository.Refresh(RootEntity);
        }

        protected override void CreateCommand_OnExecute() {
            RootEntity = _cashJournalRepository.New();
        }

        protected override void SaveCommand_OnExecute() {
            _cashJournalRepository.Save(RootEntity);
        }

        protected override void DeleteCommand_OnExecute() {
            _cashJournalRepository.Delete(RootEntity);
        }

        protected override void RefreshEnabledBindings() {
            RaisePropertyChanged(nameof(HasRootEntity));
            RaisePropertyChanged(nameof(CanCreateEntry));
            RaisePropertyChanged(nameof(CanDeleteEntry));
            RaisePropertyChanged(nameof(CanEditEntry));
        }

        private void CreateEntryCommand_OnExecute() {
            var param = new NavigationParameters() {
                { ParameterNames.NextView, nameof(CashJournalView) },
                { ParameterNames.ParentId, RootEntity.Id }
            };

            NavigateToView(nameof(CashJournalEntryView), RegionNames.MainContent, param);
        }

        private void EditEntryCommand_OnExecute() {
            var param = new NavigationParameters() {
                { ParameterNames.Id,        SelectedEntry.Id },
                { ParameterNames.NextView,  nameof(CashJournalView) },
                { ParameterNames.ParentId, RootEntity.Id }
            };

            NavigateToView(nameof(CashJournalEntryView), RegionNames.MainContent, param);
        }

        private void DeleteEntryCommand_OnExecute() {
            _cashJournalEntryRepository.Delete(SelectedEntry);
        }

    }
}