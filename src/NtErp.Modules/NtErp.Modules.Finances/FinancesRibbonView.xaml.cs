using NtErp.Modules.Finances.ViewModels;
using System.Windows.Controls.Ribbon;

namespace NtErp.Modules.Finances {
    /// <summary>
    /// Interaction logic for CashJournalViewRibbon.xaml
    /// </summary>
    public partial class FinancesRibbonView : RibbonTab {
        public FinancesRibbonView(CashJournalViewModel viewModel) {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
