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
    public class CashJournalEntryViewModel : ViewModelBase, INavigationAware {
        #region INavigationAware Members

        public void OnNavigatedTo(NavigationContext navigationContext) {
            string entryId = (string)navigationContext.Parameters[ParameterNames.Id];
            if (!String.IsNullOrEmpty(entryId)) {
                long id = Int64.Parse(entryId);
                RootEntity = _repository.Find(id);
            } else
                RootEntity = _repository.New();

            string nextView = (string)navigationContext.Parameters[ParameterNames.NextView];
            if (!String.IsNullOrEmpty(nextView))
                _nextView = nextView;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext) {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext) {

        }

        #endregion

        #region Fields

        private IEventAggregator _eventAggregator;
        private IJournalEntryRepository _repository;
        private ITaxRateRepository _taxRateRepository;
        private ILifetimeScope _scope;
        private CashJournalEntryPosition _selectedPosition;
        private ObservableCollection<TaxRate> _availableTaxRates;

        #endregion


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
            get { return HasRootEntity; }
        }

        public bool CanAddPosition {
            get { return HasRootEntity && SelectedPosition != null; }
        }


        #region Commands

        private ICommand _refreshEntryCommand;
        private ICommand _saveEntryCommand;
        private ICommand _attachDocumentCommand;
        private ICommand _createPositionCommand;
        private ICommand _addPositionCommand;
        private ICommand _applyCommand;
        private ICommand _cancelCommand;


        public ICommand RefreshEntryCommand {
            get { return _refreshEntryCommand ?? (_refreshEntryCommand = new DelegateCommand(RefreshEntryCommand_OnExecute)); }
        }

        public ICommand SaveEntryCommand {
            get { return _saveEntryCommand ?? (_saveEntryCommand = new DelegateCommand(SaveEntryCommand_OnExecute)); }
        }

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

        public CashJournalEntryViewModel(ILifetimeScope scope, IRegionManager regionManager, IEventAggregator eventAggregator,
            IJournalEntryRepository repository, ITaxRateRepository taxRateRepository) {
            _scope = scope;
            _eventAggregator = eventAggregator;
            _repository = repository;
            _regionManager = regionManager;
            _taxRateRepository = taxRateRepository;

            RefreshAvailableTaxRates();
        }

        #endregion


        private void RefreshAvailableTaxRates() {
            AvailableTaxRates = new ObservableCollection<TaxRate>(_taxRateRepository.Fetch());
        }

        private void RefreshEntryCommand_OnExecute() {
            _repository.Refresh(RootEntity);
        }

        private void SaveEntryCommand_OnExecute() {
            _repository.Save(RootEntity);
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
                throw new ArgumentNullException("ERROR: Selected Entity is 'null' => JournalEntryViewModel.AttachDocumentCommand_OnExecute()");

            entry.DocumentName = file.Name;
            entry.DocumentFolderPath = file.DirectoryName;
        }

        private void CreatePositionCommand_OnExecute() {
            SelectedPosition = _repository.NewPosition();
        }

        private void AddPositionCommand_OnExecute() {
            CashJournalEntry entry = RootEntity as CashJournalEntry;
            if (entry == null)
                throw new ArgumentNullException("ERROR: Selected Entity is 'null' => JournalEntryViewModel.AddPositionCommand_OnExecute()");

            _repository.AddPosition(entry, SelectedPosition);
        }

        private void ApplyCommand_OnExecute() {
            //@TODO: Check for changes

            NavigateToView(_nextView, RegionNames.MainContent);
        }

        private void CancelCommand_OnExecute() {
            NavigateToView(_nextView, RegionNames.MainContent);
        }

        protected override void RefreshEnabledBindings() {
            throw new NotImplementedException();
        }
    }
}
