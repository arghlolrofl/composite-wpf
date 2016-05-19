using Autofac;
using NtErp.Shared.Contracts.Repository;
using NtErp.Shared.Entities.MasterFileData;
using NtErp.Shared.Services.ViewModels;
using Prism.Events;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;

namespace NtErp.Modules.MasterFileData.ViewModels {
    public class TaxRateViewModel : EntityViewModel, INavigationAware {
        #region INavigationAware Members

        public void OnNavigatedTo(NavigationContext navigationContext) {
            IsActive = true;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext) {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext) {
            IsActive = false;
        }

        #endregion


        private ITaxRateRepository _taxRateRepository;
        private ObservableCollection<TaxRate> _taxRates;

        #region Properties

        public ObservableCollection<TaxRate> TaxRates {
            get { return _taxRates; }
            set { _taxRates = value; RaisePropertyChanged(); }
        }

        #endregion

        #region Initialization

        public TaxRateViewModel(ILifetimeScope scope, IEventAggregator eventAggregator, ITaxRateRepository repository, IRegionManager regionManager)
            : base(scope, eventAggregator, regionManager) {
            _taxRateRepository = repository;
            _taxRates = new ObservableCollection<TaxRate>();

            RefreshTaxRates();
        }

        #endregion


        protected override void CreateCommand_OnExecute() {
            RootEntity = _taxRateRepository.New();
        }

        protected override void RefreshCommand_OnExecute() {
            _taxRateRepository.Refresh(RootEntity);
        }

        protected override void SaveCommand_OnExecute() {
            _taxRateRepository.Save(RootEntity);

            RefreshEnabledBindings();
            RefreshTaxRates();
        }

        protected override void DeleteCommand_OnExecute() {
            _taxRateRepository.Delete(RootEntity);
            RootEntity = null;

            RefreshEnabledBindings();
            RefreshTaxRates();
        }



        private void RefreshTaxRates() {
            TaxRates = new ObservableCollection<TaxRate>(_taxRateRepository.Fetch());
        }

        protected override void RefreshEnabledBindings() {
            RaisePropertyChanged(nameof(HasRootEntity));
            RaisePropertyChanged(nameof(CanCreateNew));
            RaisePropertyChanged(nameof(CanRefresh));
            RaisePropertyChanged(nameof(CanDelete));
            RaisePropertyChanged(nameof(CanSave));
        }

        protected override void OpenSearchCommand_OnExecute() {
            throw new NotImplementedException();
        }
    }
}