
using NtErp.Shared.Entities.Base;
using NtErp.Shared.Services.Contracts;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace NtErp.Shared.Services.ViewModels {
    public class MenuItemViewModel : ViewModelBase, IMenuItemViewModel {
        private string _header;
        private ICommand _command;
        private ObservableCollection<IMenuItemViewModel> _items = new ObservableCollection<IMenuItemViewModel>();


        public ObservableCollection<IMenuItemViewModel> Items {
            get { return _items; }
            set { _items = value; RaisePropertyChanged(); }
        }

        public ICommand Command {
            get { return _command; }
            set { _command = value; RaisePropertyChanged(); }
        }

        public string Header {
            get { return _header; }
            set { _header = value; RaisePropertyChanged(); }
        }


        public MenuItemViewModel() {

        }
    }
}
