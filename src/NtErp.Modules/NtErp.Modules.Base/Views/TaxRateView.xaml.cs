using NtErp.Modules.Base.ViewModels;
using System.Windows.Controls;

namespace NtErp.Modules.Base.Views {
    public partial class TaxRateView : UserControl {
        private TaxRateViewModel _viewModel;

        public TaxRateView(TaxRateViewModel viewModel) {
            InitializeComponent();

            DataContext = _viewModel = viewModel;
        }
    }
}
