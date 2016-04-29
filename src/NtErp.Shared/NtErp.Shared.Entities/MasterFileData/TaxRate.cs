using NtErp.Shared.Entities.Base;
using NtErp.Shared.Entities.CashJournal;
using System.Collections.ObjectModel;

namespace NtErp.Shared.Entities.MasterFileData {
    public class TaxRate : EntityBase {
        private string _category;
        private decimal _value;
        private ObservableCollection<JournalEntry> _journalEntries;
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

        public virtual ObservableCollection<JournalEntry> JournalEntries {
            get { return _journalEntries; }
            set { _journalEntries = value; RaisePropertyChanged(); }
        }
    }
}