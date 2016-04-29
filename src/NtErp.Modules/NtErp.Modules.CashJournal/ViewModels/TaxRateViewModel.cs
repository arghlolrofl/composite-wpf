using Autofac;
using NtErp.Shared.Entities.Base;
using NtErp.Shared.Entities.MasterFileData;
using NtErp.Shared.Services.Contracts;
using Prism.Events;
using System.Collections.ObjectModel;

namespace NtErp.Modules.CashJournal.ViewModels {
    public class TaxRateViewModel : ViewModelBase {
        private IEventAggregator _eventAggregator;
        private ITaxRateRepository _repository;
        private ILifetimeScope _scope;
        private TaxRate _selectedTaxRate;
        private ObservableCollection<TaxRate> _taxRates;

        public TaxRate SelectedTaxRate {
            get { return _selectedTaxRate; }
            set { _selectedTaxRate = value; RaisePropertyChanged(); }
        }

        public ObservableCollection<TaxRate> TaxRates {
            get { return _taxRates; }
            set { _taxRates = value; RaisePropertyChanged(); }
        }


        public TaxRateViewModel(ILifetimeScope scope, IEventAggregator eventAggregator, ITaxRateRepository repository) {
            _scope = scope;
            _eventAggregator = eventAggregator;
            _repository = repository;
            _taxRates = new ObservableCollection<TaxRate>();

            refreshTaxRates();
        }

        private void refreshTaxRates() {
            _taxRates = new ObservableCollection<TaxRate>(_repository.GetAll());
        }
    }
}