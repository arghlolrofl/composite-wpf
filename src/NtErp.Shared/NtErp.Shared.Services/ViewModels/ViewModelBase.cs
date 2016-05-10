using NtErp.Shared.Services.Base;
using Prism.Regions;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NtErp.Shared.Services.ViewModels {
    public abstract class ViewModelBase : INotifyPropertyChanged {
        protected IRegionManager _regionManager;
        private EntityBase _rootEntity;


        public EntityBase RootEntity {
            get { return _rootEntity; }
            set {
                _rootEntity = value;
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

        public virtual bool CanSave { get { return true; } }

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
