using Autofac;
using NtErp.Modules.Finances.ViewModels;
using System.Windows;

namespace NtErp.Modules.Finances.Views {
    public partial class CashJournalSearchWindow : Window {
        readonly ILifetimeScope _scope;
        readonly CashJournalSearchViewModel _viewModel;

        public CashJournalSearchWindow(ILifetimeScope scope, CashJournalSearchViewModel viewModel) {
            InitializeComponent();

            DataContext = _viewModel = viewModel;
            _scope = scope;

            _viewModel.CloseDialogRequested += (sender, args) => Close();
        }
    }
}
