using System.Collections.ObjectModel;
using System.Windows.Input;

namespace NtErp.Shared.Services.Contracts {
    public interface IMenuItemViewModel {
        string Header { get; set; }
        ICommand Command { get; set; }
        ObservableCollection<IMenuItemViewModel> Items { get; set; }
    }
}
