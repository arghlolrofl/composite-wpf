using Autofac;
using Microsoft.Win32;
using NtErp.Shared.Contracts.Repository;
using NtErp.Shared.Entities.Finances;
using NtErp.Shared.Entities.MasterFileData;
using NtErp.Shared.Services.Constants;
using NtErp.Shared.Services.Contracts;
using NtErp.Shared.Services.ViewModels;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace NtErp.Modules.Finances.ViewModels {
    public class CashJournalEntryViewModel : EntityViewModel, INavigationAware {
        #region INavigationAware Members

        public void OnNavigatedTo(NavigationContext navigationContext) {
            IsActive = true;

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
            }

            // Set a SelectedPosition for direct editing
            var entry = RootEntity as CashJournalEntry;
            if (!entry.Positions.Any())
                SelectedPosition = _cashJournalEntryRepository.NewPosition(entry);
            else
                SelectedPosition = entry.Positions.Last();

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
            IsActive = false;
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

        public bool IsAnyActive {
            get {
                return IsActive || Parent.IsActive;
            }
        }


        public CashJournalEntryPosition SelectedPosition {
            get { return _selectedPosition; }
            set { _selectedPosition = value; RaisePropertyChanged(); }
        }

        public ObservableCollection<TaxRate> AvailableTaxRates {
            get { return _availableTaxRates; }
            set { _availableTaxRates = value; RaisePropertyChanged(); }
        }

        public bool CanRefreshEntry {
            get {
                return IsActive && HasRootEntity && RootEntity.Exists;
            }
        }

        public bool CanSaveEntry {
            get {
                return IsActive && HasRootEntity && ((RootEntity.Exists && RootEntity.HasChanges) || (!RootEntity.Exists));
            }
        }

        public bool CanAttachDocument {
            get { return IsActive && HasRootEntity; }
        }

        public bool CanCreatePosition {
            get { return IsActive && (RootEntity.Exists && !RootEntity.HasChanges) && (HasRootEntity && (SelectedPosition != null && SelectedPosition.Exists)); }
        }

        public bool CanAddPosition {
            get { return IsActive && (RootEntity.Exists && !RootEntity.HasChanges) && HasRootEntity && (SelectedPosition != null && !SelectedPosition.Exists); }
        }

        public bool CanDeletePosition {
            get {
                return IsActive && (RootEntity.Exists && !RootEntity.HasChanges) && (SelectedPosition != null && SelectedPosition.Exists);
            }
        }

        public CashJournalViewModel Parent { get; private set; }

        #endregion

        #region Commands

        private ICommand _attachDocumentCommand;
        private ICommand _createPositionCommand;
        private ICommand _addPositionCommand;
        private ICommand _goBackCommand;
        private ICommand _deletePositionCommand;


        public ICommand AttachDocumentCommand {
            get { return _attachDocumentCommand ?? (_attachDocumentCommand = new DelegateCommand(AttachDocumentCommand_OnExecute)); }
        }

        public ICommand CreatePositionCommand {
            get { return _createPositionCommand ?? (_createPositionCommand = new DelegateCommand(CreatePositionCommand_OnExecute)); }
        }

        public ICommand AddPositionCommand {
            get { return _addPositionCommand ?? (_addPositionCommand = new DelegateCommand(AddPositionCommand_OnExecute)); }
        }

        public ICommand DeletePositionCommand {
            get { return _deletePositionCommand ?? (_deletePositionCommand = new DelegateCommand(DeletePositionCommand_OnExecute)); }
        }

        public ICommand GoBackCommand {
            get { return _goBackCommand ?? (_goBackCommand = new DelegateCommand(GoBackCommand_OnExecute)); }
        }

        #endregion

        #region Initialization

        public CashJournalEntryViewModel(
            ILifetimeScope scope,
            IRegionManager regionManager,
            IEventAggregator eventAggregator,
            ICashJournalRepository cashJournalRepository,
            ICashJournalEntryRepository repository,
            ITaxRateRepository taxRateRepository,
            CashJournalViewModel parent)
            : base(scope, eventAggregator, regionManager) {
            _cashJournalRepository = cashJournalRepository;
            _cashJournalEntryRepository = repository;
            _taxRateRepository = taxRateRepository;
            Parent = parent;

            Parent.PropertyChanged += (sender, args) => {
                if (args.PropertyName == nameof(Parent.IsActive))
                    RaisePropertyChanged(nameof(IsAnyActive));
            };

            PropertyChanged += (sender, args) => {
                if (args.PropertyName == nameof(IsActive))
                    RaisePropertyChanged(nameof(IsAnyActive));
            };

            RefreshAvailableTaxRates();
            RefreshEnabledBindings();
        }

        #endregion

        /// <summary>
        /// Refreshes the list of available <see cref="TaxRate"/>s.
        /// </summary>
        private void RefreshAvailableTaxRates() {
            AvailableTaxRates = new ObservableCollection<TaxRate>(_taxRateRepository.Fetch());
        }

        /// <inheritdoc />
        protected override void RefreshCommand_OnExecute() {
            _cashJournalEntryRepository.Refresh(RootEntity);
            RefreshEnabledBindings();
        }

        /// <inheritdoc />
        protected override void SaveCommand_OnExecute() {
            _cashJournalEntryRepository.Save(RootEntity);
            RefreshEnabledBindings();
        }

        /// <inheritdoc />
        protected override void RefreshEnabledBindings() {
            RaisePropertyChanged(nameof(CanAttachDocument));
            RaisePropertyChanged(nameof(CanCreatePosition));
            RaisePropertyChanged(nameof(CanAddPosition));
            RaisePropertyChanged(nameof(CanDeletePosition));
            RaisePropertyChanged(nameof(IsActive));
            RaisePropertyChanged(nameof(IsAnyActive));
            RaisePropertyChanged(nameof(SelectedPosition));
        }

        /// <inheritdoc />
        protected override void CreateCommand_OnExecute() {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        protected override void DeleteCommand_OnExecute() {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        protected override void OpenSearchCommand_OnExecute() {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Attaches document information to an entry.
        /// </summary>
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

        /// <summary>
        /// Creates a new position for the current entry that can be edited and added later.
        /// </summary>
        private void CreatePositionCommand_OnExecute() {
            var entry = RootEntity as CashJournalEntry;

            SelectedPosition = _cashJournalEntryRepository.NewPosition(entry);

            RefreshEnabledBindings();
        }

        /// <summary>
        /// Adds a position to the current entry or updates an existing position.
        /// </summary>
        private void AddPositionCommand_OnExecute() {
            if (!RootEntity.Exists || (RootEntity.Exists && RootEntity.HasChanges))
                _cashJournalEntryRepository.Save(RootEntity);

            CashJournalEntry entry = RootEntity as CashJournalEntry;
            if (entry == null)
                throw new ArgumentNullException("ERROR: Selected Entity is 'null' => JournalEntryViewModel.AddPositionCommand_OnExecute()");

            if (!SelectedPosition.Exists)
                _cashJournalEntryRepository.AddPosition(entry, SelectedPosition);
            else
                _cashJournalEntryRepository.UpdatePosition(SelectedPosition);

            entry.RefreshCashBalance();
            RefreshEnabledBindings();
            //RaisePropertyChanged(String.Empty);
        }

        /// <summary>
        /// Deletes the selected position from the entry.
        /// </summary>
        private void DeletePositionCommand_OnExecute() {
            _cashJournalEntryRepository.RemovePosition(SelectedPosition);
        }

        /// <summary>
        /// Navigates back to the last view.
        /// </summary>
        private void GoBackCommand_OnExecute() {
            //@TODO: Check for changes

            NavigateToView(_nextView, ShellRegions.MainContent);
        }
    }
}
