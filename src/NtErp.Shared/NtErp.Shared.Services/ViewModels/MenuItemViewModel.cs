using Autofac;
using NtErp.Shared.Contracts.Infrastructure;
using NtErp.Shared.Services.Base;
using Prism.Events;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace NtErp.Shared.Services.ViewModels {
    public abstract class MenuItemViewModel : CommonViewModel, IMenuItemViewModel {
        private string _header;
        private ICommand _command;
        private EntityBase _selectedEntity;
        private ObservableCollection<IMenuItemViewModel> _items = new ObservableCollection<IMenuItemViewModel>();


        public string Header {
            get { return _header; }
            set { _header = value; RaisePropertyChanged(); }
        }

        public ICommand Command {
            get { return _command; }
            set { _command = value; RaisePropertyChanged(); }
        }

        public ObservableCollection<IMenuItemViewModel> Items {
            get { return _items; }
            set { _items = value; RaisePropertyChanged(); }
        }


        public MenuItemViewModel(ILifetimeScope scope, IEventAggregator eventAggregator) : base(scope, eventAggregator) {

        }

    }
}
