using Autofac;
using Microsoft.Win32;
using NtErp.Shared.Contracts.Repository;
using NtErp.Shared.Entities.CashJournal;
using NtErp.Shared.Entities.MasterFileData;
using NtErp.Shared.Services.Contracts;
using NtErp.Shared.Services.Regions;
using NtErp.Shared.Services.ViewModels;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;

namespace NtErp.Modules.Finances.ViewModels {
    public class CashJournalEntryViewModel : EntityViewModel, INavigationAware {
        #region INavigationAware Members

        public void OnNavigatedTo(NavigationContext navigationContext) {
            // we do expect at least 2 parameters here:
            //  - Parent Id (Id of the CashJournal)
            //  - Next View (The view to call, after closing this one)
            //  - Id (Id of the entry to be edited, optional)

            // First, we fetch the entry id ...
            string entryId = (string)navigationContext.Parameters[ParameterNames.Id];
            if (!String.IsNullOrEmpty(entryId)) {
                // ... and load the desired entity.
                long id = Int64.Parse(entryId);
                RootEntity = _cashJournalEntryRepository.Find(id);
            } else {
                // If it will be a new entry, we have to fetch the (parent) journal object first ...
                long parentId = Int64.Parse((string)navigationContext.Parameters[ParameterNames.ParentId]);
                var parent = _cashJournalRepository.Find(parentId);

                // ... and then we can create the new entry for the journal.
                RootEntity = _cashJournalEntryRepository.New(parent);

                // For a new entry, we initialize a position too.
                var entry = RootEntity as CashJournalEntry;
                SelectedPosition = _cashJournalEntryRepository.NewPosition(entry);
            }

            // Store the name of the next view for later use
            string nextView = (string)navigationContext.Parameters[ParameterNames.NextView];
            if (!String.IsNullOrEmpty(nextView))
                _nextView = nextView;

            RefreshEnabledBindings();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext) {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext) {

        }

        #endregion

        #region Fields

        private ICashJournalEntryRepository _cashJournalEntryRepository;
        private ITaxRateRepository _taxRateRepository;
        private CashJournalEntryPosition _selectedPosition;
        private ObservableCollection<TaxRate> _availableTaxRates;
        private ICashJournalRepository _cashJournalRepository;

        #endregion

        #region Properties

        public CashJournalEntryPosition SelectedPosition {
            get { return _selectedPosition; }
            set { _selectedPosition = value; RaisePropertyChanged(); }
        }

        public ObservableCollection<TaxRate> AvailableTaxRates {
            get { return _availableTaxRates; }
            set { _availableTaxRates = value; RaisePropertyChanged(); }
        }

        public bool CanAttachDocument {
            get { return HasRootEntity; }
        }

        public bool CanCreatePosition {
            get { return HasRootEntity && SelectedPosition != null && SelectedPosition.Exists; }
        }

        public bool CanAddPosition {
            get { return HasRootEntity && SelectedPosition != null; }
        }

        #endregion

        #region Commands

        private ICommand _attachDocumentCommand;
        private ICommand _createPositionCommand;
        private ICommand _addPositionCommand;
        private ICommand _applyCommand;
        private ICommand _cancelCommand;

        public ICommand AttachDocumentCommand {
            get { return _attachDocumentCommand ?? (_attachDocumentCommand = new DelegateCommand(AttachDocumentCommand_OnExecute)); }
        }

        public ICommand CreatePositionCommand {
            get { return _createPositionCommand ?? (_createPositionCommand = new DelegateCommand(CreatePositionCommand_OnExecute)); }
        }

        public ICommand AddPositionCommand {
            get { return _addPositionCommand ?? (_addPositionCommand = new DelegateCommand(AddPositionCommand_OnExecute)); }
        }

        public ICommand ApplyCommand {
            get { return _applyCommand ?? (_applyCommand = new DelegateCommand(ApplyCommand_OnExecute)); }
        }

        public ICommand CancelCommand {
            get { return _cancelCommand ?? (_cancelCommand = new DelegateCommand(CancelCommand_OnExecute)); }
        }

        #endregion

        #region Initialization

        public CashJournalEntryViewModel(
            ILifetimeScope scope,
            IRegionManager regionManager,
            IEventAggregator eventAggregator,
            ICashJournalRepository cashJournalRepository,
            ICashJournalEntryRepository repository,
            ITaxRateRepository taxRateRepository)
            : base(scope, eventAggregator) {
            _cashJournalRepository = cashJournalRepository;
            _cashJournalEntryRepository = repository;
            _regionManager = regionManager;
            _taxRateRepository = taxRateRepository;

            RefreshAvailableTaxRates();
        }

        #endregion


        private void RefreshAvailableTaxRates() {
            AvailableTaxRates = new ObservableCollection<TaxRate>(_taxRateRepository.Fetch());
        }

        protected override void RefreshCommand_OnExecute() {
            _cashJournalEntryRepository.Refresh(RootEntity);
        }

        protected override void SaveCommand_OnExecute() {
            _cashJournalEntryRepository.Save(RootEntity);
        }

        protected override void RefreshEnabledBindings() {
            RaisePropertyChanged(nameof(CanAttachDocument));
            RaisePropertyChanged(nameof(CanCreatePosition));
            RaisePropertyChanged(nameof(CanAddPosition));
        }

        protected override void CreateCommand_OnExecute() {
            throw new NotImplementedException();
        }

        protected override void DeleteCommand_OnExecute() {
            throw new NotImplementedException();
        }

        protected override void OpenSearchCommand_OnExecute() {
            throw new NotImplementedException();
        }

        private void AttachDocumentCommand_OnExecute() {
            OpenFileDialog ofd = new OpenFileDialog() {
                CheckFileExists = true,
                CheckPathExists = true,
                Multiselect = false,
                Title = "Select document"
            };

            ofd.ShowDialog();

            string fileName = ofd.FileName;
            if (!File.Exists(fileName))
                return;

            FileInfo file = new FileInfo(fileName);

            CashJournalEntry entry = RootEntity as CashJournalEntry;
            if (entry == null)
                throw new ArgumentNullException("ERROR: Selected Entity is 'null' => CashJournalEntryViewModel.AttachDocumentCommand_OnExecute()");

            entry.DocumentName = file.Name;
            entry.DocumentFolderPath = file.DirectoryName;
        }

        private void CreatePositionCommand_OnExecute() {
            var entry = RootEntity as CashJournalEntry;

            SelectedPosition = _cashJournalEntryRepository.NewPosition(entry);
        }

        private void AddPositionCommand_OnExecute() {
            if (!RootEntity.Exists || (RootEntity.Exists && RootEntity.HasChanges))
                _cashJournalEntryRepository.Save(RootEntity);

            CashJournalEntry entry = RootEntity as CashJournalEntry;
            if (entry == null)
                throw new ArgumentNullException("ERROR: Selected Entity is 'null' => JournalEntryViewModel.AddPositionCommand_OnExecute()");

            _cashJournalEntryRepository.AddPosition(entry, SelectedPosition);
        }

        private void ApplyCommand_OnExecute() {
            //@TODO: Check for changes

            NavigateToView(_nextView, RegionNames.MainContent);
        }

        private void CancelCommand_OnExecute() {
            NavigateToView(_nextView, RegionNames.MainContent);
        }
    }
}
