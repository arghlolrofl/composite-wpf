using NtErp.Shell.Demo.ViewModels;
using System.Windows;

namespace NtErp.Shell.Demo.Views {
    /// <summary>
    /// Interaction logic for ShellWindow.xaml
    /// </summary>
    public partial class ShellView : Window {
        private ShellViewModel _viewModel;

        public ShellView(ShellViewModel viewModel) {
            InitializeComponent();

            _viewModel = viewModel;
        }
    }
}
