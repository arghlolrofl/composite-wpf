using NtErp.Shared.Entities.Base;
using NtErp.Shared.Entities.MasterFileData;
using System;
using System.ComponentModel.DataAnnotations;

namespace NtErp.Shared.Entities.Finances {
    public class CashJournalEntryPosition : EntityBase {
        private decimal _delta;
        private string _description;
        private decimal _prepaidTax;
        private TaxRate _taxRate;
        private CashJournalEntry _entry;

        [Required]
        public decimal Delta {
            get { return Math.Round(_delta, 2); }
            set {
                _delta = value;
                RaisePropertyChanged();
                CalculatePrepaidTax();
            }
        }

        public string Description {
            get { return _description; }
            set { _description = value; RaisePropertyChanged(); }
        }

        public decimal PrepaidTax {
            get { return _prepaidTax; }
            set { _prepaidTax = value; RaisePropertyChanged(); }
        }

        [Required]
        public virtual TaxRate TaxRate {
            get { return _taxRate; }
            set {
                _taxRate = value;
                RaisePropertyChanged();

                CalculatePrepaidTax();
            }
        }

        [Required]
        public virtual CashJournalEntry Entry {
            get { return _entry; }
            set { _entry = value; RaisePropertyChanged(); }
        }


        private void CalculatePrepaidTax() {
            if (_delta == 0 || TaxRate == null) {
                PrepaidTax = 0.00m;
            } else {
                decimal divisor = 100 + TaxRate.Value;

                decimal prepaidTax = _delta - (_delta / divisor * 100);

                PrepaidTax = Math.Round(prepaidTax, 2);
            }

            RaisePropertyChanged(nameof(PrepaidTax));
        }

        protected override void RegisterPropertiesToTrack() {
            TrackProperties(nameof(Description));
        }
    }
}
