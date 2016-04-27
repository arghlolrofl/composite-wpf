using Autofac;
using Microsoft.Practices.Prism.Commands;
using NtErp.Modules.Base.Views;
using NtErp.Shared.Entities.Base;
using NtErp.Shared.Entities.MasterFileData;
using NtErp.Shared.Services.Contracts;
using NtErp.Shared.Services.Events;
using Prism.Events;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;

namespace NtErp.Modules.Base.ViewModels {
    public class ProductViewModel : ViewModelBase {
        private ICommand _createProductCommand;
        private ICommand _updateProductCommand;
        private ICommand _deleteProductCommand;
        private ICommand _openProductSearchCommand;
        private ICommand _addComponentCommand;
        private ICommand _removeComponentCommand;
        private ICommand _refreshProductCommand;

        private readonly ILifetimeScope _scope;
        private readonly IProductRepository _repository;
        private readonly IEventAggregator _eventAggregator;
        private Product _selectedProduct;
        private ProductComponent _selectedProductComponent;
        private string _statusText;
        private bool _hasChanges;

        #region Commands

        public ICommand RefreshProductCommand {
            get { return _refreshProductCommand ?? (_refreshProductCommand = new DelegateCommand(RefreshProductCommand_OnExecute)); }
        }

        public ICommand CreateProductCommand {
            get { return _createProductCommand ?? (_createProductCommand = new DelegateCommand(CreateProductCommand_OnExecute)); }
        }

        public ICommand UpdateProductCommand {
            get { return _updateProductCommand ?? (_updateProductCommand = new DelegateCommand(UpdateProductCommand_OnExecute)); }
        }

        public ICommand DeleteProductCommand {
            get { return _deleteProductCommand ?? (_deleteProductCommand = new DelegateCommand(DeleteProductCommand_OnExecute)); }
        }

        public ICommand OpenProductSearchCommand {
            get { return _openProductSearchCommand ?? (_openProductSearchCommand = new DelegateCommand(OpenProductSearchCommand_OnExecute)); }
        }

        public ICommand AddComponentCommand {
            get { return _addComponentCommand ?? (_addComponentCommand = new DelegateCommand(AddComponentCommand_OnExecute)); }
        }

        public ICommand RemoveComponentCommand {
            get { return _removeComponentCommand ?? (_removeComponentCommand = new DelegateCommand(RemoveComponentCommand_OnExecute)); }
        }

        #endregion

        #region View Bindings

        public Product SelectedProduct {
            get { return _selectedProduct; }
            set {
                if (_selectedProduct != null)
                    _selectedProduct.PropertyChanged -= SelectedProduct_OnPropertyChanged;

                _selectedProduct = value;
                HasChanges = false;

                RaisePropertyChanged();
                RaisePropertyChanged(nameof(HasRootEntity));
                RaisePropertyChanged(nameof(CanDeleteProduct));
                RaisePropertyChanged(nameof(CanUpdateProduct));
                RaisePropertyChanged(nameof(CanRefreshProduct));
                RaisePropertyChanged(nameof(CanAddComponents));
                RaisePropertyChanged(nameof(CanRemoveComponents));

                _selectedProduct.PropertyChanged += SelectedProduct_OnPropertyChanged;
            }
        }

        public ProductComponent SelectedProductComponent {
            get { return _selectedProductComponent; }
            set {
                _selectedProductComponent = value;

                RaisePropertyChanged();
                RaisePropertyChanged(nameof(HasComponentSelected));
                RaisePropertyChanged(nameof(CanRemoveComponents));
            }
        }

        public string StatusText {
            get { return _statusText; }
            set { _statusText = value; RaisePropertyChanged(); }
        }

        public bool HasRootEntity {
            get { return SelectedProduct != null; }
        }

        public bool CanRefreshProduct {
            get {
                return HasRootEntity;
            }

        }

        public bool CanUpdateProduct {
            get { return HasRootEntity && HasChanges; }
        }

        public bool CanDeleteProduct {
            get { return HasRootEntity; }
        }

        public bool HasComponentSelected {
            get {
                return SelectedProductComponent != null;
            }
        }

        public bool CanAddComponents {
            get {
                return HasRootEntity;
            }
        }

        public bool CanRemoveComponents {
            get {
                return HasRootEntity && HasComponentSelected && SelectedProduct.Components.Any();
            }
        }

        public bool HasChanges {
            get { return _hasChanges; }
            set {
                _hasChanges = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(CanUpdateProduct));
            }
        }


        #endregion

        public ProductViewModel(ILifetimeScope scope, IProductRepository repository, IEventAggregator eventAggregator) {
            _scope = scope;
            _repository = repository;
            _eventAggregator = eventAggregator;
        }

        private void SelectedProduct_OnPropertyChanged(object sender, PropertyChangedEventArgs e) {
            Debug.WriteLine($"SelectedProduct_OnPropertyChanged => {e.PropertyName}");

            switch (e.PropertyName) {
                case "Id":
                    break;
                default:
                    HasChanges = true;
                    break;
            }
        }

        private void RefreshProductCommand_OnExecute() {
            SelectedProduct = _repository.GetSingle(SelectedProduct.Id);
        }

        private void OpenProductSearchCommand_OnExecute() {
            _eventAggregator.GetEvent<PubSubEvent<EntitySearchResultEvent>>()
                            .Subscribe(ProductSearch_OnReply);

            var searchWindow = _scope.Resolve<ProductSearchWindow>();
            searchWindow.ShowDialog();
        }

        private void ProductSearch_OnReply(EntitySearchResultEvent response) {
            _eventAggregator.GetEvent<PubSubEvent<EntitySearchResultEvent>>()
                            .Unsubscribe(ProductSearch_OnReply);

            if (response.DialogResult.Equals(true)) {
                SelectedProduct = _repository.GetSingle(response.EntityId);
                //StatusText = "Selected Product: " + SelectedProduct.Number;
            }
        }

        private void CreateProductCommand_OnExecute() {
            SelectedProduct = _repository.New();
            //StatusText = "Create new Product";
        }

        private void UpdateProductCommand_OnExecute() {
            _repository.Save(SelectedProduct);
            //StatusText = "Product saved";
            HasChanges = false;
            RaisePropertyChanged(nameof(SelectedProduct));
        }

        private void DeleteProductCommand_OnExecute() {
            _repository.Delete(SelectedProduct);
            //StatusText = "Product deleted";
        }

        private void AddComponentCommand_OnExecute() {
            _eventAggregator.GetEvent<PubSubEvent<EntitySearchResultEvent>>()
                            .Subscribe(ComponentSearch_OnReply);

            var searchWindow = _scope.Resolve<ProductSearchWindow>();
            searchWindow.ShowDialog();
        }

        private void ComponentSearch_OnReply(EntitySearchResultEvent response) {
            _eventAggregator.GetEvent<PubSubEvent<EntitySearchResultEvent>>()
                            .Unsubscribe(ProductSearch_OnReply);

            if (!response.DialogResult.Equals(true))
                return;

            Product component = _repository.GetSingle(response.EntityId);
            _repository.AddComponent(SelectedProduct, component);

            //StatusText = "Selected Product: " + SelectedProduct.Number;
        }

        private void RemoveComponentCommand_OnExecute() {
            if (SelectedProductComponent == null)
                return;

            //string componentName = SelectedProductComponent.Name;

            _repository.RemoveComponent(SelectedProductComponent);
            //StatusText = $"Removed component {SelectedProduct.Name} from product {componentName}";
        }
    }
}
