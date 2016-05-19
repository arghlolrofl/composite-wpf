using System.Windows.Controls;

namespace NtErp.Modules.Finances {
  /// <summary>
  /// Interaction logic for RibbonControl.xaml
  /// </summary>
  public partial class FinancesModuleRibbon : UserControl {
    public FinancesModuleRibbon(FinancesModuleRibbonViewModel viewModel) {
      DataContext = viewModel;

      InitializeComponent();
    }
  }
}
