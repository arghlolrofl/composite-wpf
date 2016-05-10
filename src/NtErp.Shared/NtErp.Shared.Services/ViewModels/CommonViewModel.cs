﻿using Autofac;
using Prism.Events;
using Prism.Regions;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NtErp.Shared.Services.ViewModels {
    public abstract class CommonViewModel : INotifyPropertyChanged {
        protected IRegionManager _regionManager;
        protected ILifetimeScope _scope;
        protected IEventAggregator _eventAggregator;


        public CommonViewModel(ILifetimeScope scope, IEventAggregator eventAggregator) {
            _scope = scope;
            _eventAggregator = eventAggregator;
        }


        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged([CallerMemberName]string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #endregion

        #region Navigation Implementation

        protected virtual void NavigateToView(string viewName, string regionName, NavigationParameters parameters = null) {
            IRegion region = _regionManager.Regions[regionName];

            string queryString = parameters == null ? String.Empty : parameters.ToString();

            region.RequestNavigate(new Uri(viewName + queryString, UriKind.Relative));
        }

        #endregion
    }
}