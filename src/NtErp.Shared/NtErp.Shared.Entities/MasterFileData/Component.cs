using NtErp.Shared.Entities.Base;

namespace NtErp.Shared.Entities.MasterFileData {
    public class Component : EntityBase {
        private int _Amount;
        private Product _parent;
        private Product _product;


        public virtual Product Parent {
            get { return _parent; }
            set { _parent = value; RaisePropertyChanged(); }
        }


        public virtual Product ProductComponent {
            get { return _product; }
            set { _product = value; RaisePropertyChanged(); }
        }

        public int Amount {
            get { return _Amount; }
            set { _Amount = value; RaisePropertyChanged(); }
        }

    }
}