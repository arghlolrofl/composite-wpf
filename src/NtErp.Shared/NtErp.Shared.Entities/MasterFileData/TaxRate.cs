using NtErp.Shared.Entities.Base;

namespace NtErp.Shared.Entities.MasterFileData {
    public class TaxRate : EntityBase {
        private string _category;
        private decimal _value;
        private string _description;

        public string Category {
            get { return _category; }
            set { _category = value; RaisePropertyChanged(); }
        }

        public decimal Value {
            get { return _value; }
            set { _value = value; RaisePropertyChanged(); }
        }

        public string Description {
            get { return _description; }
            set { _description = value; RaisePropertyChanged(); }
        }


        public TaxRate() {
            trackProperties(nameof(Category), nameof(Value), nameof(Description));
        }
    }
}