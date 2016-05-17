using Autofac;
using NtErp.Shared.Contracts.Repository;
using NtErp.Shared.Entities.MasterFileData;
using NtErp.Shared.Services.Events;
using NtErp.Shared.Services.ViewModels;
using Prism.Events;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;

namespace NtErp.ViewModel.MasterFileData {
    public class ProductSearchViewModel : SearchViewModel {
        private IProductRepository _productRepository;
        private ObservableCollection<Product> _products = new ObservableCollection<Product>();


        #region Properties

        public ObservableCollection<Product> Products {
            get { return _products; }
            set { _products = value; RaisePropertyChanged(); }
        }

        #endregion

        #region Initialization

        public ProductSearchViewModel(IProductRepository productRepository, IEventAggregator eventAggregator, ILifetimeScope scope, IRegionManager regionManager)
            : base(scope, eventAggregator, regionManager) {
            _productRepository = productRepository;

            RefreshComponents();
        }

        #endregion


        protected override void SelectCommand_OnExecute() {
            if (SelectedEntity != null) {
                DialogResult = true;
                SendResponseAndRequestCloseDialog();
            }
        }

        protected override void MouseDoubleClickCommand_OnExecute() {
            if (SelectedEntity != null) {
                DialogResult = true;
                SendResponseAndRequestCloseDialog();
            }
        }

        protected override void SearchCommand_OnExecute() {
            RefreshComponents();
        }

        protected override void ResetCommand_OnExecute() {
            throw new NotImplementedException();
        }

        protected override void CancelCommand_OnExecute() {
            DialogResult = false;
            SendResponseAndRequestCloseDialog();
        }

        private void RefreshComponents() {
            Products.Clear();
            Products.AddRange(_productRepository.Fetch());
        }

        private void SendResponseAndRequestCloseDialog() {
            long id = SelectedEntity == null ? 0 : SelectedEntity.Id;

            _eventAggregator.GetEvent<PubSubEvent<EntitySearchResultEvent>>()
                            .Publish(new EntitySearchResultEvent(id, DialogResult));

            RaiseCloseDialogRequested();
        }
    }
}
