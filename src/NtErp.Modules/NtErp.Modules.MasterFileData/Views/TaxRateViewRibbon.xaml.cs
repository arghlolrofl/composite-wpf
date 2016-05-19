using NtErp.Modules.MasterFileData.ViewModels;
using System.Windows.Controls;

namespace NtErp.Modules.MasterFileData.Views {
    public partial class TaxRateViewRibbon : UserControl {
        public TaxRateViewRibbon(TaxRateViewModel viewModel) {
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}
