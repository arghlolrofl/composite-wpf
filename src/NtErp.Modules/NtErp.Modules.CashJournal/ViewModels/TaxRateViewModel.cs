using Autofac;
using NtErp.Shared.Entities.Base;
using NtErp.Shared.Entities.MasterFileData;
using NtErp.Shared.Services.Contracts;
using Prism.Commands;
using Prism.Events;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace NtErp.Modules.CashJournal.ViewModels {
    public class TaxRateViewModel : ViewModelBase {
        #region Commands

        private ICommand _CreateTaxRateCommand;
        private ICommand _RefreshTaxRateCommand;
        private ICommand _UpdateTaxRateCommand;
        private ICommand _DeleteTaxRateCommand;


        public ICommand CreateTaxRateCommand {
            get { return _CreateTaxRateCommand ?? (_CreateTaxRateCommand = new DelegateCommand(CreateTaxRateCommand_OnExecute)); }
        }

        public ICommand RefreshTaxRateCommand {
            get { return _RefreshTaxRateCommand ?? (_RefreshTaxRateCommand = new DelegateCommand(RefreshTaxRateCommand_OnExecute)); }
        }

        public ICommand UpdateTaxRateCommand {
            get { return _UpdateTaxRateCommand ?? (_UpdateTaxRateCommand = new DelegateCommand(UpdateTaxRateCommand_OnExecute)); }
        }

        public ICommand DeleteTaxRateCommand {
            get { return _DeleteTaxRateCommand ?? (_DeleteTaxRateCommand = new DelegateCommand(DeleteTaxRateCommand_OnExecute)); }
        }

        #endregion

        private IEventAggregator _eventAggregator;
        private ITaxRateRepository _repository;
        private ILifetimeScope _scope;
        private TaxRate _selectedTaxRate;
        private ObservableCollection<TaxRate> _taxRates;

        #region Properties

        public TaxRate SelectedTaxRate {
            get { return _selectedTaxRate; }
            set {
                _selectedTaxRate = value;

                raiseIsEnabledPropertyChanged();

                if (SelectedTaxRate != null)
                    SelectedTaxRate.PropertyChanged += (sender, args) => {
                        if (args.PropertyName == nameof(SelectedTaxRate.HasChanges))
                            RaisePropertyChanged(nameof(CanUpdateTaxRate));
                    };
            }
        }

        public ObservableCollection<TaxRate> TaxRates {
            get { return _taxRates; }
            set { _taxRates = value; RaisePropertyChanged(); }
        }

        public bool HasRootEntity {
            get { return SelectedTaxRate != null; }
        }

        public bool CanCreateNewTaxRate {
            get { return !HasRootEntity || (HasRootEntity && SelectedTaxRate.Id > 0); }
        }

        public bool CanRefreshTaxRate {
            get { return HasRootEntity && SelectedTaxRate.Id > 0; }
        }

        public bool CanUpdateTaxRate {
            get { return HasRootEntity && SelectedTaxRate.HasChanges; }
        }

        public bool CanDeleteTaxRate {
            get { return HasRootEntity && SelectedTaxRate.Id > 0; }
        }

        #endregion

        #region Initialization

        public TaxRateViewModel(ILifetimeScope scope, IEventAggregator eventAggregator, ITaxRateRepository repository) {
            _scope = scope;
            _eventAggregator = eventAggregator;
            _repository = repository;
            _taxRates = new ObservableCollection<TaxRate>();

            refreshTaxRates();
        }

        #endregion

        private void refreshTaxRates() {
            TaxRates = new ObservableCollection<TaxRate>(_repository.GetAll());
        }

        private void CreateTaxRateCommand_OnExecute() {
            SelectedTaxRate = _repository.New();
        }

        private void RefreshTaxRateCommand_OnExecute() {
            _repository.Refresh(SelectedTaxRate);
        }

        private void UpdateTaxRateCommand_OnExecute() {
            _repository.Save(SelectedTaxRate);

            raiseIsEnabledPropertyChanged();

            refreshTaxRates();
        }

        private void DeleteTaxRateCommand_OnExecute() {
            _repository.Delete(SelectedTaxRate);
            SelectedTaxRate = null;

            raiseIsEnabledPropertyChanged();

            refreshTaxRates();
        }




        private void raiseIsEnabledPropertyChanged() {
            RaisePropertyChanged(nameof(SelectedTaxRate));
            RaisePropertyChanged(nameof(HasRootEntity));
            RaisePropertyChanged(nameof(CanCreateNewTaxRate));
            RaisePropertyChanged(nameof(CanRefreshTaxRate));
            RaisePropertyChanged(nameof(CanDeleteTaxRate));
            RaisePropertyChanged(nameof(CanUpdateTaxRate));
        }
    }
}