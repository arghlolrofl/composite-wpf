using System.Windows.Controls.Ribbon;

namespace NtErp.Modules.MasterFileData {
    /// <summary>
    /// Interaction logic for MasterFileDataModuleRibbonView.xaml
    /// </summary>
    public partial class RibbonView : RibbonTab {
        public RibbonView(RibbonViewModel viewModel) {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
