using Microsoft.Practices.Prism.Commands;
using NtErp.Shared.Contracts.Repository;
using NtErp.Shared.Entities.MasterFileData;
using NtErp.Shared.Services.Events;
using NtErp.Shared.Services.ViewModels;
using Prism.Events;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace NtErp.ViewModel.MasterFileData {
    public class ProductSearchViewModel : ViewModelBase {
        private bool? _dialogResult;
        private IProductRepository _repository;
        private IEventAggregator _eventAggregator;
        private Product _selectedProduct;
        private ObservableCollection<Product> _products = new ObservableCollection<Product>();

        #region Commands

        private ICommand _selectCommand;
        private ICommand _searchCommand;
        private ICommand _resetCommand;
        private ICommand _cancelCommand;
        private ICommand _mouseDoubleClickCommand;

        public ICommand MouseDoubleClickCommand {
            get { return _mouseDoubleClickCommand ?? (_mouseDoubleClickCommand = new DelegateCommand(MouseDoubleClickCommand_OnExecute)); }
        }

        public ICommand SelectCommand {
            get { return _selectCommand ?? (_selectCommand = new DelegateCommand(SelectCommand_OnExecute)); }
        }

        public ICommand SearchCommand {
            get { return _searchCommand ?? (_searchCommand = new DelegateCommand(SearchCommand_OnExecute)); }
        }

        public ICommand ResetCommand {
            get { return _resetCommand ?? (_resetCommand = new DelegateCommand(ResetCommand_OnExecute)); }
        }

        public ICommand CancelCommand {
            get { return _cancelCommand ?? (_cancelCommand = new DelegateCommand(CancelCommand_OnExecute)); }
        }

        #endregion

        #region Properties

        public bool? DialogResult {
            get { return _dialogResult; }
            set { _dialogResult = value; RaisePropertyChanged(); }
        }

        public ObservableCollection<Product> Products {
            get { return _products; }
            set { _products = value; RaisePropertyChanged(); }
        }

        public Product SelectedProduct {
            get { return _selectedProduct; }
            set { _selectedProduct = value; RaisePropertyChanged(); }
        }

        #endregion

        #region Initialization

        public ProductSearchViewModel(IProductRepository repository, IEventAggregator eventAggregator) {
            _repository = repository;
            _eventAggregator = eventAggregator;

            RefreshComponents();
        }

        #endregion


        private void SelectCommand_OnExecute() {
            if (SelectedProduct != null) {
                DialogResult = true;
                SendResponseAndRequestCloseDialog();
            }
        }

        private void MouseDoubleClickCommand_OnExecute() {
            if (SelectedProduct != null) {
                DialogResult = true;
                SendResponseAndRequestCloseDialog();
            }
        }

        private void SearchCommand_OnExecute() {
            RefreshComponents();
        }

        private void ResetCommand_OnExecute() {
            throw new NotImplementedException();
        }

        private void CancelCommand_OnExecute() {
            DialogResult = false;
            SendResponseAndRequestCloseDialog();
        }

        private void RefreshComponents() {
            Products.Clear();
            Products.AddRange(_repository.GetAll());
        }

        private void SendResponseAndRequestCloseDialog() {
            _eventAggregator.GetEvent<PubSubEvent<EntitySearchResultEvent>>()
                            .Publish(new EntitySearchResultEvent(SelectedProduct.Id, DialogResult));

            RaiseCloseDialogRequested();
        }
    }
}
