using System.Windows.Controls;

namespace NtErp.Modules.Finances {
  /// <summary>
  /// Interaction logic for RibbonControl.xaml
  /// </summary>
  public partial class RibbonControl : UserControl {
    public RibbonControl(RibbonViewModel viewModel) {
      DataContext = viewModel;

      InitializeComponent();
    }
  }
}
