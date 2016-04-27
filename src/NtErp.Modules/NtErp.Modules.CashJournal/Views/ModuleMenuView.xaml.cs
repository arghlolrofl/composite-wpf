using Autofac;
using NtErp.Modules.CashJournal.ViewModels;
using System.Windows.Controls;

namespace NtErp.Modules.CashJournal.Views {
    /// <summary>
    /// Interaction logic for ModuleMenuView.xaml
    /// </summary>
    public partial class ModuleMenuView : MenuItem {
        private ILifetimeScope _scope;
        private ModuleMenuViewModel _viewModel;

        public ModuleMenuView(ILifetimeScope scope, ModuleMenuViewModel viewModel) {
            InitializeComponent();

            DataContext = _viewModel = viewModel;
            _scope = scope;
        }
    }
}
