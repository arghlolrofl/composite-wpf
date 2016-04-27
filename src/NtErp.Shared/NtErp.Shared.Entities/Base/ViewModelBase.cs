using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NtErp.Shared.Entities.Base {
    public class ViewModelBase : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged([CallerMemberName]string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


        public event EventHandler CloseDialogRequested;
        protected void RaiseCloseDialogRequested() => CloseDialogRequested?.Invoke(this, EventArgs.Empty);
    }
}
