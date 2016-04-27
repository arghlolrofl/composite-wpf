using Autofac;
using NtErp.Modules.CashJournal.ViewModels;
using System.Windows;

namespace NtErp.Modules.Base.Views {
    public partial class JournalBookSearchWindow : Window {
        readonly ILifetimeScope _scope;
        readonly JournalBookSearchViewModel _viewModel;

        public JournalBookSearchWindow(ILifetimeScope scope, JournalBookSearchViewModel viewModel) {
            InitializeComponent();

            DataContext = _viewModel = viewModel;
            _scope = scope;

            _viewModel.CloseDialogRequested += (sender, args) => Close();
        }
    }
}
