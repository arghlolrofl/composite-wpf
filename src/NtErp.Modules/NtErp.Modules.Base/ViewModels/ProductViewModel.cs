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
    public class ProductViewModel : EntityViewModel {
        private ICommand _addComponentCommand;
        private ICommand _removeComponentCommand;

        private readonly IProductRepository _productRepository;
        private ProductComponent _selectedComponent;


        #region Commands

        public ICommand AddComponentCommand {
            get { return _addComponentCommand ?? (_addComponentCommand = new DelegateCommand(AddComponentCommand_OnExecute)); }
        }

        public ICommand RemoveComponentCommand {
            get { return _removeComponentCommand ?? (_removeComponentCommand = new DelegateCommand(RemoveComponentCommand_OnExecute)); }
        }

        #endregion

        #region View Bindings

        public ProductComponent SelectedComponent {
            get { return _selectedComponent; }
            set {
                _selectedComponent = value;

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


        public ProductViewModel(ILifetimeScope scope, IProductRepository productRepository, IEventAggregator eventAggregator) : base(scope, eventAggregator) {
            _productRepository = productRepository;
        }

        protected override void RefreshCommand_OnExecute() {
            _productRepository.Refresh(RootEntity);
        }

        protected override void OpenSearchCommand_OnExecute() {
            _eventAggregator.GetEvent<PubSubEvent<EntitySearchResultEvent>>()
                            .Subscribe(ProductSearch_OnReply);

            var searchWindow = _scope.Resolve<ProductSearchWindow>();
            searchWindow.ShowDialog();
        }

        private void ProductSearch_OnReply(EntitySearchResultEvent response) {
            _eventAggregator.GetEvent<PubSubEvent<EntitySearchResultEvent>>()
                            .Unsubscribe(ProductSearch_OnReply);

            if (response.DialogResult.Equals(true))
                RootEntity = _productRepository.Find(response.EntityId);
        }

        protected override void CreateCommand_OnExecute() {
            RootEntity = _productRepository.New();
            //StatusText = "Create new Product";
        }

        protected override void SaveCommand_OnExecute() {
            _productRepository.Save(RootEntity);
        }

        protected override void DeleteCommand_OnExecute() {
            _productRepository.Delete(RootEntity);
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

            Product component = _productRepository.Find(response.EntityId);
            Product kit = RootEntity as Product;

            _productRepository.AddComponent(kit, component);
        }

        private void RemoveComponentCommand_OnExecute() {
            if (SelectedComponent == null)
                return;

            _productRepository.RemoveComponent(SelectedComponent);
        }

        protected override void RefreshEnabledBindings() {
            RaisePropertyChanged(nameof(CanAddComponents));
            RaisePropertyChanged(nameof(CanCreateNew));
            RaisePropertyChanged(nameof(CanDelete));
            RaisePropertyChanged(nameof(CanSave));
            RaisePropertyChanged(nameof(CanRefresh));

        }
    }
}
