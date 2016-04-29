using NtErp.Modules.CashJournal.ViewModels;
using System.Windows.Controls;

namespace NtErp.Modules.CashJournal.Views {
    public partial class TaxRateView : UserControl {
        private TaxRateViewModel _viewModel;

        public TaxRateView(TaxRateViewModel viewModel) {
            InitializeComponent();

            DataContext = _viewModel = viewModel;
        }
    }
}
