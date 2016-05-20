using NtErp.Modules.Finances.ViewModels;
using System.Windows.Controls;

namespace NtErp.Modules.Finances.Views {
    public partial class TaxRateViewRibbon : UserControl {
        public TaxRateViewRibbon(TaxRateViewModel viewModel) {
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}
