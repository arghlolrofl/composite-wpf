using NtErp.Modules.CashJournal.ViewModels;
using System.Windows.Controls;

namespace NtErp.Modules.CashJournal.Views {
    public partial class JournalEntryView : UserControl {
        private JournalEntryViewModel _viewModel;

        public JournalEntryView(JournalEntryViewModel viewModel) {
            InitializeComponent();

            DataContext = _viewModel = viewModel;
        }
    }
}
