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

namespace NtErp.Modules.CashJournal.ViewModels {
    public class JournalEntryViewModel : ViewModelBase, INavigationAware {
        #region INavigationAware Members

        public void OnNavigatedTo(NavigationContext navigationContext) {
            string entryId = (string)navigationContext.Parameters[ParameterNames.Id];
            if (!String.IsNullOrEmpty(entryId)) {
                long id = Int64.Parse(entryId);
                SelectedEntity = _repository.GetSingle(id);
            } else
                SelectedEntity = _repository.New();

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
        private JournalEntryPosition _selectedPosition;
        private ObservableCollection<TaxRate> _availableTaxRates;

        #endregion


        public JournalEntryPosition SelectedPosition {
            get { return _selectedPosition; }
            set { _selectedPosition = value; RaisePropertyChanged(); }
        }

        public ObservableCollection<TaxRate> AvailableTaxRates {
            get { return _availableTaxRates; }
            set { _availableTaxRates = value; RaisePropertyChanged(); }
        }

        public bool CanAttachDocument {
            get { return HasEntitySelected; }
        }

        public bool CanRefreshEntry {
            get { return HasEntitySelected && SelectedEntity.Id > 0; }
        }

        public bool CanSaveEntry {
            get { return HasEntitySelected && SelectedEntity.HasChanges; }
        }

        public bool CanCreatePosition {
            get { return HasEntitySelected; }
        }

        public bool CanAddPosition {
            get { return HasEntitySelected && SelectedPosition != null; }
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

        public JournalEntryViewModel(ILifetimeScope scope, IRegionManager regionManager, IEventAggregator eventAggregator,
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
            AvailableTaxRates = new ObservableCollection<TaxRate>(_taxRateRepository.GetAll());
        }

        private void RefreshEntryCommand_OnExecute() {
            _repository.Refresh(SelectedEntity);
        }

        private void SaveEntryCommand_OnExecute() {
            _repository.Save(SelectedEntity);
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

            JournalEntry entry = SelectedEntity as JournalEntry;
            if (entry == null)
                throw new ArgumentNullException("ERROR: Selected Entity is 'null' => JournalEntryViewModel.AttachDocumentCommand_OnExecute()");

            entry.DocumentName = file.Name;
            entry.DocumentFolderPath = file.DirectoryName;
        }

        private void CreatePositionCommand_OnExecute() {
            SelectedPosition = _repository.NewPosition();
        }

        private void AddPositionCommand_OnExecute() {
            JournalEntry entry = SelectedEntity as JournalEntry;
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
    }
}
