using NtErp.Modules.Finances.ViewModels;
using System.Windows.Controls;

namespace NtErp.Modules.Finances.Views {
    public partial class CashJournalEntryViewRibbon : UserControl {
        public CashJournalEntryViewRibbon(CashJournalEntryViewModel viewModel) {
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}
