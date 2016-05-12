using NtErp.Modules.Finances.ViewModels;
using NtErp.Shared.Services.Views;

namespace NtErp.Modules.Finances.Views {
    public partial class CashJournalView : CommonView {
        private CashJournalViewModel _viewModel;

        public CashJournalView(CashJournalViewModel viewModel) {
            InitializeComponent();

            DataContext = _viewModel = viewModel;
        }
    }
}
