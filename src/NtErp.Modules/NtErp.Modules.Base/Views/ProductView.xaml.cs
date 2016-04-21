using NtErp.Modules.Base.ViewModels;
using System.Windows.Controls;

namespace NtErp.Modules.Base.Views {
    /// <summary>
    /// Interaction logic for ComponentView.xaml
    /// </summary>
    public partial class ProductView : UserControl {
        private ProductViewModel _viewModel;


        public ProductView(ProductViewModel viewModel) {
            InitializeComponent();

            DataContext = _viewModel = viewModel;
        }
    }
}
