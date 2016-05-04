using NtErp.Shared.Entities.MasterFileData;
using NtErp.Shared.Services.Base;

namespace NtErp.Shared.Entities.CashJournal {
    public class JournalEntryPosition : EntityBase {
        private decimal _delta;
        private string _description;
        private decimal _prepaidTax;
        private TaxRate _taxRate;
        private JournalEntry _journalEntry;


        public decimal Delta {
            get { return _delta; }
            set { _delta = value; RaisePropertyChanged(); }
        }

        public string Description {
            get { return _description; }
            set { _description = value; RaisePropertyChanged(); }
        }

        public decimal PrepaidTax {
            get { return _prepaidTax; }
            set { _prepaidTax = value; RaisePropertyChanged(); }
        }

        public virtual TaxRate TaxRate {
            get { return _taxRate; }
            set { _taxRate = value; RaisePropertyChanged(); }
        }

        public virtual JournalEntry JournalEntry {
            get { return _journalEntry; }
            set { _journalEntry = value; RaisePropertyChanged(); }
        }


        public JournalEntryPosition() {
            trackProperties(nameof(Description));
        }
    }
}
