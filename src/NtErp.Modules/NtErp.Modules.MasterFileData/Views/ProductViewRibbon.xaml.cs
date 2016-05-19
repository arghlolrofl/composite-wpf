using NtErp.Modules.MasterFileData.ViewModels;
using System.Windows.Controls;

namespace NtErp.Modules.MasterFileData.Views {
    public partial class ProductViewRibbon : UserControl {
        public ProductViewRibbon(ProductViewModel viewModel) {
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}
