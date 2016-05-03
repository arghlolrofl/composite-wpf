using Autofac;
using NtErp.Shared.Entities.Base;
using NtErp.Shared.Entities.CashJournal;
using NtErp.Shared.Entities.MasterFileData;
using NtErp.Shared.Services.Contracts;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace NtErp.Modules.CashJournal.ViewModels {
    public class JournalEntryViewModel : ViewModelBase, INavigationAware {
        private IEventAggregator _eventAggregator;
        private IJournalEntryRepository _repository;
        private ITaxRateRepository _taxRateRepository;
        private ILifetimeScope _scope;
        private JournalEntry _selectedEntry;
        private JournalEntryPosition _selectedPosition;
        private ObservableCollection<TaxRate> _availableTaxRates;

        public JournalEntry SelectedEntry {
            get { return _selectedEntry; }
            set { _selectedEntry = value; RaisePropertyChanged(); }
        }

        public JournalEntryPosition SelectedPosition {
            get { return _selectedPosition; }
            set { _selectedPosition = value; RaisePropertyChanged(); }
        }

        public ObservableCollection<TaxRate> AvailableTaxRates {
            get { return _availableTaxRates; }
            set { _availableTaxRates = value; RaisePropertyChanged(); }
        }


        #region Commands

        private ICommand _refreshEntryCommand;
        private ICommand _saveEntryCommand;
        private ICommand _attachDocumentCommand;
        private ICommand _addPositionCommand;


        public ICommand RefreshEntryCommand {
            get { return _refreshEntryCommand ?? (_refreshEntryCommand = new DelegateCommand(RefreshEntryCommand_OnExecute)); }
        }

        public ICommand SaveEntryCommand {
            get { return _saveEntryCommand ?? (_saveEntryCommand = new DelegateCommand(SaveEntryCommand_OnExecute)); }
        }

        public ICommand AttachDocumentCommand {
            get { return _attachDocumentCommand ?? (_attachDocumentCommand = new DelegateCommand(AttachDocumentCommand_OnExecute)); }
        }

        public ICommand AddPositionCommand {
            get { return _addPositionCommand ?? (_addPositionCommand = new DelegateCommand(AddPositionCommand_OnExecute)); }
        }

        #endregion



        public JournalEntryViewModel(ILifetimeScope scope, IEventAggregator eventAggregator,
            IJournalEntryRepository repository, ITaxRateRepository taxRateRepository) {
            _scope = scope;
            _eventAggregator = eventAggregator;
            _repository = repository;
            _taxRateRepository = taxRateRepository;

            refreshAvailableTaxRates();
        }

        private void refreshAvailableTaxRates() {
            AvailableTaxRates = new ObservableCollection<TaxRate>(_taxRateRepository.GetAll());
        }

        private void RefreshEntryCommand_OnExecute() {
            throw new NotImplementedException();
        }

        private void SaveEntryCommand_OnExecute() {
            throw new NotImplementedException();
        }

        private void AttachDocumentCommand_OnExecute() {
            throw new NotImplementedException();
        }

        private void AddPositionCommand_OnExecute() {
            throw new NotImplementedException();
        }

        #region INavigationAware Members

        public void OnNavigatedTo(NavigationContext navigationContext) {
            string entryId = (string)navigationContext.Parameters["id"];

            if (!String.IsNullOrEmpty(entryId)) {
                long id = Int64.Parse(entryId);
                SelectedEntry = _repository.GetSingle(id);
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext) {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext) {

        }

        #endregion
    }
}
