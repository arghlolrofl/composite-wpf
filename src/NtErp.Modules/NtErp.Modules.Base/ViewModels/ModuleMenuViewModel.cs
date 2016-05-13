﻿using Autofac;
using Microsoft.Practices.Prism.Commands;
using Prism.Events;
using Prism.Modularity;
using Prism.Regions;
using System.Windows.Input;

namespace NtErp.Modules.Base.ViewModels {
    public class ModuleMenuViewModel {
        private ICommand _goToProductViewCommand;
        private ICommand _goToTaxRateViewCommand;
        private IModuleManager _moduleManager;

        public ICommand GoToProductViewCommand {
            get { return _goToProductViewCommand ?? (_goToProductViewCommand = new DelegateCommand(GoToProductViewCommand_OnExecute)); }
        }

        public ICommand GoToTaxRateViewCommand {
            get { return _goToTaxRateViewCommand ?? (_goToTaxRateViewCommand = new DelegateCommand(GoToTaxRateViewCommand_OnExecute)); }
        }

        public ModuleMenuViewModel(ILifetimeScope scope, IRegionManager regionManager, IModuleManager moduleManager, IEventAggregator eventAggregator)
            /*: base(scope, eventAggregator, regionManager)*/ {
            _moduleManager = moduleManager;
        }

        private void GoToProductViewCommand_OnExecute() {
            //IRegion region = _regionManager.Regions[RegionNames.MainContent];
            //Uri viewUri = new Uri(nameof(ProductView), UriKind.Relative);

            //region.RequestNavigate(viewUri, Callback);
        }

        private void GoToTaxRateViewCommand_OnExecute() {
            //IRegion region = _regionManager.Regions[RegionNames.MainContent];
            //Uri viewUri = new Uri(nameof(TaxRateView), UriKind.Relative);

            //region.RequestNavigate(viewUri);
        }

        private void Callback(NavigationResult obj) {

        }

    }
}