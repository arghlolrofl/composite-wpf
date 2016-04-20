using NtErp.Shared.Entities.Base;
using System.Collections.ObjectModel;

namespace NtErp.Shared.Entities.MasterFileData {
    public class Product : EntityBase {
        private int _Version;
        private string _Number;
        private string _Name;
        private string _Description;
        private ObservableCollection<Component> _components = new ObservableCollection<Component>();


        public int Version {
            get { return _Version; }
            set { _Version = value; RaisePropertyChanged(); }
        }

        public string Number {
            get { return _Number; }
            set { _Number = value; RaisePropertyChanged(); }
        }

        public string Name {
            get { return _Name; }
            set { _Name = value; RaisePropertyChanged(); }
        }

        public string Description {
            get { return _Description; }
            set { _Description = value; RaisePropertyChanged(); }
        }

        public virtual ObservableCollection<Component> Components {
            get { return _components; }
            set { _components = value; RaisePropertyChanged(); }
        }


        public Product() {

        }
    }
}
