using NtErp.Modules.Finances.ViewModels;
using System.Windows.Controls;

namespace NtErp.Modules.Finances.Views {
    public partial class CashJournalView : UserControl {
        private CashJournalViewModel _viewModel;

        public CashJournalView(CashJournalViewModel viewModel) {
            InitializeComponent();

            DataContext = _viewModel = viewModel;
        }
    }
}
