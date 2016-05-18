using NtErp.Modules.Finances.ViewModels;
using System.Windows.Controls;

namespace NtErp.Modules.Finances.Views {
  /// <summary>
  /// Interaction logic for CashJournalViewRibbon.xaml
  /// </summary>
  public partial class CashJournalViewRibbon : UserControl {
    public CashJournalViewRibbon(CashJournalViewModel viewModel) {
      DataContext = viewModel;

      InitializeComponent();
    }
  }
}
