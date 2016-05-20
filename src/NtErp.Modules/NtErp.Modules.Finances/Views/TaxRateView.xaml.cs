using NtErp.Modules.Finances.ViewModels;
using System.Windows.Controls;

namespace NtErp.Modules.Finances.Views {
    public partial class TaxRateView : UserControl {
        private TaxRateViewModel _viewModel;

        public TaxRateView(TaxRateViewModel viewModel) {
            InitializeComponent();

            DataContext = _viewModel = viewModel;
        }
    }
}
