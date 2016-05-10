using NtErp.Modules.Finances.ViewModels;
using System.Windows.Controls;

namespace NtErp.Modules.Finances.Views {
    public partial class CashJournalEntryView : UserControl {
        private CashJournalEntryViewModel _viewModel;

        public CashJournalEntryView(CashJournalEntryViewModel viewModel) {
            InitializeComponent();

            DataContext = _viewModel = viewModel;
        }
    }
}
