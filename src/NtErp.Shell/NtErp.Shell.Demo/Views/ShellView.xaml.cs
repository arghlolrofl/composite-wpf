using NtErp.Shell.Demo.ViewModels;
using System.Windows.Controls.Ribbon;

namespace NtErp.Shell.Demo.Views {
    /// <summary>
    /// Interaction logic for ShellWindow.xaml
    /// </summary>
    public partial class ShellView : RibbonWindow {
        private ShellViewModel _viewModel;

        public ShellView(ShellViewModel viewModel) {
            InitializeComponent();

            _viewModel = viewModel;
        }
    }
}
