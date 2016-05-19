using System.Windows.Controls;

namespace NtErp.Modules.MasterFileData {
    public partial class MasterFileDataModuleRibbon : UserControl {
        public MasterFileDataModuleRibbon(MasterFileDataRibbonViewModel viewModel) {
            DataContext = viewModel;

            InitializeComponent();
        }
    }
}
