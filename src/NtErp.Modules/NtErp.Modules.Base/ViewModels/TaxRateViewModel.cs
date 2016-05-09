using Autofac;
using NtErp.Shared.Contracts.Repository;
using NtErp.Shared.Entities.MasterFileData;
using NtErp.Shared.Services.ViewModels;
using Prism.Commands;
using Prism.Events;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace NtErp.Modules.Base.ViewModels {
    public class TaxRateViewModel : ViewModelBase {
        #region Commands

        private ICommand _createTaxRateCommand;
        private ICommand _refreshTaxRateCommand;
        private ICommand _saveTaxRateCommand;
        private ICommand _deleteTaxRateCommand;


        public ICommand CreateTaxRateCommand {
            get { return _createTaxRateCommand ?? (_createTaxRateCommand = new DelegateCommand(CreateTaxRateCommand_OnExecute)); }
        }

        public ICommand RefreshTaxRateCommand {
            get { return _refreshTaxRateCommand ?? (_refreshTaxRateCommand = new DelegateCommand(RefreshTaxRateCommand_OnExecute)); }
        }

        public ICommand SaveTaxRateCommand {
            get { return _saveTaxRateCommand ?? (_saveTaxRateCommand = new DelegateCommand(SaveTaxRateCommand_OnExecute)); }
        }

        public ICommand DeleteTaxRateCommand {
            get { return _deleteTaxRateCommand ?? (_deleteTaxRateCommand = new DelegateCommand(DeleteTaxRateCommand_OnExecute)); }
        }

        #endregion

        private IEventAggregator _eventAggregator;
        private ITaxRateRepository _repository;
        private ILifetimeScope _scope;
        private ObservableCollection<TaxRate> _taxRates;

        #region Properties

        public ObservableCollection<TaxRate> TaxRates {
            get { return _taxRates; }
            set { _taxRates = value; RaisePropertyChanged(); }
        }

        #endregion

        #region Initialization

        public TaxRateViewModel(ILifetimeScope scope, IEventAggregator eventAggregator, ITaxRateRepository repository) {
            _scope = scope;
            _eventAggregator = eventAggregator;
            _repository = repository;
            _taxRates = new ObservableCollection<TaxRate>();

            RefreshTaxRates();
        }

        #endregion


        private void CreateTaxRateCommand_OnExecute() {
            SelectedEntity = _repository.New();
        }

        private void RefreshTaxRateCommand_OnExecute() {
            _repository.Refresh(SelectedEntity);
        }

        private void SaveTaxRateCommand_OnExecute() {
            _repository.Save(SelectedEntity);

            RefreshEnabledBindings();
            RefreshTaxRates();
        }

        private void DeleteTaxRateCommand_OnExecute() {
            _repository.Delete(SelectedEntity);
            SelectedEntity = null;

            RefreshEnabledBindings();
            RefreshTaxRates();
        }



        private void RefreshTaxRates() {
            TaxRates = new ObservableCollection<TaxRate>(_repository.GetAll());
        }

        protected override void RefreshEnabledBindings() {
            RaisePropertyChanged(nameof(HasRootEntity));
            RaisePropertyChanged(nameof(CanCreateNew));
            RaisePropertyChanged(nameof(CanRefresh));
            RaisePropertyChanged(nameof(CanDelete));
            RaisePropertyChanged(nameof(CanSave));
        }

    }
}