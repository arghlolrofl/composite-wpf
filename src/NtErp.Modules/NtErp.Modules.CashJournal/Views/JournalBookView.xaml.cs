using NtErp.Modules.CashJournal.ViewModels;
using System.Windows.Controls;

namespace NtErp.Modules.CashJournal.Views {
    public partial class JournalBookView : UserControl {
        private JournalBookViewModel _viewModel;

        public JournalBookView(JournalBookViewModel viewModel) {
            InitializeComponent();

            DataContext = _viewModel = viewModel;
        }
    }
}
