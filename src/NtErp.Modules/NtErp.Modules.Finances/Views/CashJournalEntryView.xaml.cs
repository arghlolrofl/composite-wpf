using NtErp.Modules.Finances.ViewModels;
using NtErp.Shared.Services.Views;

namespace NtErp.Modules.Finances.Views {
    public partial class CashJournalEntryView : CommonView {
        private CashJournalEntryViewModel _viewModel;

        public CashJournalEntryView(CashJournalEntryViewModel viewModel) {
            InitializeComponent();

            DataContext = _viewModel = viewModel;
        }
    }
}
