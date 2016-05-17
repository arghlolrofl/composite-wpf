using System.Windows.Controls.Ribbon;

namespace NtErp.Modules.Finances {
    /// <summary>
    /// Interaction logic for CashJournalViewRibbon.xaml
    /// </summary>
    public partial class RibbonView : RibbonTab {
        public RibbonView(RibbonViewModel viewModel) {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
