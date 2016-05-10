﻿using NtErp.Shared.Services.Base;
using Prism.Regions;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NtErp.Shared.Services.ViewModels {
    public abstract class ViewModelBase : INotifyPropertyChanged {
        protected string _nextView;
        protected IRegionManager _regionManager;
        private EntityBase _selectedEntity;


        public EntityBase RootEntity {
            get { return _selectedEntity; }
            protected set {
                _selectedEntity = value;
                RaisePropertyChanged();
                RefreshEnabledBindings();

                if (RootEntity != null) {
                    RootEntity.PropertyChanged += (sender, args) => {
                        if (args.PropertyName == nameof(RootEntity.HasChanges))
                            RaisePropertyChanged(nameof(CanSave));
                    };
                }
            }
        }
        public virtual bool HasRootEntity {
            get { return RootEntity != null; }
        }

        public virtual bool CanCreateNew {
            get { return !HasRootEntity && (HasRootEntity && RootEntity.Id > 0); }
        }

        public virtual bool CanRefresh {
            get { return HasRootEntity && RootEntity.Id > 0; }
        }

        public virtual bool CanSave {
            get { return HasRootEntity && RootEntity.HasChanges; }
        }

        public virtual bool CanDelete {
            get { return HasRootEntity && RootEntity.Id > 0; }
        }

        protected abstract void RefreshEnabledBindings();



        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged([CallerMemberName]string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #endregion

        #region CloseDialog Event Members

        public event EventHandler CloseDialogRequested;
        protected void RaiseCloseDialogRequested() => CloseDialogRequested?.Invoke(this, EventArgs.Empty);

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
