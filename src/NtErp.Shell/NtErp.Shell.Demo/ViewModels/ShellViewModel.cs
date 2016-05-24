using Autofac;
using NtErp.Shared.Services.Constants;
using NtErp.Shared.Services.ViewModels;
using NtErp.Shell.Demo.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using System.Windows.Input;

namespace NtErp.Shell.Demo.ViewModels {
    public partial class ShellViewModel : CommonViewModel {
        #region Commands

        private ICommand _showOptionsViewCommand;
        private ICommand _shutdownCommand;


        public ICommand ShowOptionsViewCommand {
            get { return _showOptionsViewCommand ?? (_showOptionsViewCommand = new DelegateCommand(ShowOptionsViewCommand_OnExecute)); }
        }

        public ICommand ShutdownCommand {
            get {
                return _shutdownCommand ?? (_shutdownCommand = new DelegateCommand(ShutdownApplication));
            }
        }

        #endregion


        public ShellViewModel(ILifetimeScope scope, IEventAggregator eventAggregator, IRegionManager regionManager) : base(scope, eventAggregator, regionManager) {
            _regionManager.RegisterViewWithRegion(ShellRegions.MainContent, typeof(StartView));

            // See nested partial class
            InitializeSettings();
        }

        private void ShowOptionsViewCommand_OnExecute() {
            var optionsWindow = new OptionsView(this);
            optionsWindow.WindowStyle = System.Windows.WindowStyle.ToolWindow;
            optionsWindow.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            optionsWindow.ShowDialog();
        }

        private void ShutdownApplication() {
            App.Current.Shutdown();
        }

    }
}
