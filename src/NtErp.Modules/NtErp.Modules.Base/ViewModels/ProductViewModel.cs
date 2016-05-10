using Autofac;
using Microsoft.Practices.Prism.Commands;
using NtErp.Modules.Base.Views;
using NtErp.Shared.Contracts.Repository;
using NtErp.Shared.Entities.MasterFileData;
using NtErp.Shared.Services.Events;
using NtErp.Shared.Services.ViewModels;
using Prism.Events;
using System.Windows.Input;

namespace NtErp.Modules.Base.ViewModels {
    public class ProductViewModel : ViewModelBase {
        private ICommand _createProductCommand;
        private ICommand _saveProductCommand;
        private ICommand _deleteProductCommand;
        private ICommand _openProductSearchCommand;
        private ICommand _addComponentCommand;
        private ICommand _removeComponentCommand;
        private ICommand _refreshProductCommand;

        private readonly ILifetimeScope _scope;
        private readonly IProductRepository _repository;
        private readonly IEventAggregator _eventAggregator;
        private ProductComponent _selectedProductComponent;
        private string _statusText;

        #region Commands

        public ICommand RefreshProductCommand {
            get { return _refreshProductCommand ?? (_refreshProductCommand = new DelegateCommand(RefreshProductCommand_OnExecute)); }
        }

        public ICommand CreateProductCommand {
            get { return _createProductCommand ?? (_createProductCommand = new DelegateCommand(CreateProductCommand_OnExecute)); }
        }

        public ICommand SaveProductCommand {
            get { return _saveProductCommand ?? (_saveProductCommand = new DelegateCommand(SaveProductCommand_OnExecute)); }
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

        public string StatusText {
            get { return _statusText; }
            set { _statusText = value; RaisePropertyChanged(); }
        }

        public ProductComponent SelectedComponent {
            get { return _selectedProductComponent; }
            set {
                _selectedProductComponent = value;

                RaisePropertyChanged();
                RaisePropertyChanged(nameof(HasComponentSelected));
                RaisePropertyChanged(nameof(CanRemoveComponents));
            }
        }

        public bool HasComponentSelected {
            get {
                return SelectedComponent != null;
            }
        }

        public bool CanAddComponents {
            get {
                return HasRootEntity;
            }
        }

        public bool CanRemoveComponents {
            get {
                return HasRootEntity && HasComponentSelected;
            }
        }

        #endregion

        public ProductViewModel(ILifetimeScope scope, IProductRepository repository, IEventAggregator eventAggregator) {
            _scope = scope;
            _repository = repository;
            _eventAggregator = eventAggregator;
        }

        private void RefreshProductCommand_OnExecute() {
            _repository.Refresh(RootEntity);
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
                RootEntity = _repository.Find(response.EntityId);
                //StatusText = "Selected Product: " + SelectedProduct.Number;
            }
        }

        private void CreateProductCommand_OnExecute() {
            RootEntity = _repository.New();
            //StatusText = "Create new Product";
        }

        private void SaveProductCommand_OnExecute() {
            _repository.Save(RootEntity);
            //StatusText = "Product saved";
        }

        private void DeleteProductCommand_OnExecute() {
            _repository.Delete(RootEntity);
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
                            .Unsubscribe(ComponentSearch_OnReply);

            if (!response.DialogResult.Equals(true))
                return;

            Product component = _repository.Find(response.EntityId);
            Product kit = RootEntity as Product;

            _repository.AddComponent(kit, component);

            //StatusText = "Selected Product: " + SelectedProduct.Number;
        }

        private void RemoveComponentCommand_OnExecute() {
            if (SelectedComponent == null)
                return;

            //string componentName = SelectedProductComponent.Name;

            _repository.RemoveComponent(SelectedComponent);
            //StatusText = $"Removed component {SelectedProduct.Name} from product {componentName}";
        }

        protected override void RefreshEnabledBindings() {
            RaisePropertyChanged(nameof(CanCreateNew));
            RaisePropertyChanged(nameof(CanDelete));
            RaisePropertyChanged(nameof(CanSave));
            RaisePropertyChanged(nameof(CanRefresh));

        }
    }
}
