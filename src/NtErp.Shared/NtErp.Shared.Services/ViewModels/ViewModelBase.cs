using NtErp.Shared.Services.Base;
using Prism.Regions;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NtErp.Shared.Services.ViewModels {
    public abstract class ViewModelBase : INotifyPropertyChanged {
        protected string _nextView;
        protected IRegionManager _regionManager;
        private EntityBase _selectedEntity;


        public EntityBase SelectedEntity {
            get { return _selectedEntity; }
            protected set { _selectedEntity = value; RaisePropertyChanged(); }
        }

        public bool HasEntitySelected {
            get { return SelectedEntity != null; }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged([CallerMemberName]string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


        public event EventHandler CloseDialogRequested;
        protected void RaiseCloseDialogRequested() => CloseDialogRequested?.Invoke(this, EventArgs.Empty);

        protected virtual void NavigateToView(string viewName, string regionName, NavigationParameters parameters = null) {
            IRegion region = _regionManager.Regions[regionName];

            string queryString = parameters == null ? String.Empty : parameters.ToString();

            region.RequestNavigate(new Uri(viewName + queryString, UriKind.Relative));
        }
    }
}
