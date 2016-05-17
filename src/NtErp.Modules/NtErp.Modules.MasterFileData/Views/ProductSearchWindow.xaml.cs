using Autofac;
using NtErp.ViewModel.MasterFileData;
using System.Windows;

namespace NtErp.Modules.MasterFileData.Views {
    public partial class ProductSearchWindow : Window {
        readonly ILifetimeScope _scope;
        readonly ProductSearchViewModel _viewModel;

        public ProductSearchWindow(ILifetimeScope scope, ProductSearchViewModel viewModel) {
            InitializeComponent();

            DataContext = _viewModel = viewModel;
            _scope = scope;

            _viewModel.CloseDialogRequested += (sender, args) => Close();
        }
    }
}
