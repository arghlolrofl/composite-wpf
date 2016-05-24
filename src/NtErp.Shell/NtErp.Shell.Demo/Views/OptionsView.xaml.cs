using NtErp.Shell.Demo.ViewModels;
using System.Windows;

namespace NtErp.Shell.Demo.Views {
    public partial class OptionsView : Window {
        public OptionsView(ShellViewModel viewModel) {
            DataContext = viewModel;
            viewModel.CloseWindowRequested += (sender, args) => Close();

            InitializeComponent();
        }
    }
}
