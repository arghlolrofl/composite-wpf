using Autofac;
using Microsoft.Practices.Prism.Commands;
using NtErp.Modules.Base.Views;
using NtErp.Shared.Entities.Base;
using NtErp.Shared.Services.Regions;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Windows.Input;

namespace NtErp.Modules.Base.ViewModels {
    public class ModuleMenuViewModel : ViewModelBase {
        private ICommand _testCommand;
        private IRegionManager _regionManager;
        private ILifetimeScope _scope;
        private IModuleManager _moduleManager;

        public ICommand TestCommand {
            get { return _testCommand ?? (_testCommand = new DelegateCommand(TestCommand_OnExecute)); }
        }

        public ModuleMenuViewModel(ILifetimeScope scope, IRegionManager regionManager, IModuleManager moduleManager) {
            _scope = scope;
            _regionManager = regionManager;
            _moduleManager = moduleManager;
        }

        private void TestCommand_OnExecute() {
            IRegion region = _regionManager.Regions[RegionNames.MainContent];
            Uri viewUri = new Uri(nameof(ProductView), UriKind.Relative);

            region.RequestNavigate(viewUri, Callback);
        }

        private void Callback(NavigationResult obj) {

        }
    }
}