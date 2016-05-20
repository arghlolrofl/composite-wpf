using Autofac;
using NtErp.Modules.Finances.Views;
using NtErp.Shared.Contracts.Repository;
using NtErp.Shared.Entities.MasterFileData;
using NtErp.Shared.Services.Constants;
using NtErp.Shared.Services.ViewModels;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace NtErp.Modules.Finances.ViewModels {
    /// <summary>
    /// ViewModel for <see cref="TaxRateView"/> and <see cref="TaxRateViewRibbon"/>
    /// </summary>
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

        #region Commands

        private ICommand _GoToTaxRateViewCommand;

        /// <summary>
        /// Navigates to the <see cref="TaxRateView"/>
        /// </summary>
        public ICommand GoToTaxRateViewCommand {
            get { return _GoToTaxRateViewCommand ?? (_GoToTaxRateViewCommand = new DelegateCommand(GoToTaxRateViewCommand_OnExecute)); }
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Creates a new instance of the TaxRateViewModel
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="eventAggregator"></param>
        /// <param name="repository"></param>
        /// <param name="regionManager"></param>
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

        private void GoToTaxRateViewCommand_OnExecute() {
            NavigateToView(nameof(TaxRateView), ShellRegions.MainContent);
        }
    }
}