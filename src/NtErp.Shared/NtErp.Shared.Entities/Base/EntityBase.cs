using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NtErp.Shared.Entities.Base {
    public class EntityBase : INotifyPropertyChanged {
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged([CallerMemberName]string propertyName = "") {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}