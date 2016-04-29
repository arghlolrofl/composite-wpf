using NtErp.Shared.Entities.Base;
using NtErp.Shared.Entities.MasterFileData;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace NtErp.Shared.Entities.CashJournal {
    public class JournalEntry : EntityBase {
        private DateTime _date;
        private string _documentNumber;
        private string _documentFolderPath;
        private string _documentName;
        private string _businessProcessDescription;
        private decimal _taxRate;
        private decimal _earning;
        private decimal _expenditure;
        private decimal _prepaidTax;
        private decimal _cashBalance;
        private JournalBook _journalBook;
        private ObservableCollection<TaxRate> _taxRates = new ObservableCollection<MasterFileData.TaxRate>();


        /// <summary>
        /// Navigation property to <see cref="JournalBook"/>
        /// </summary>
        public virtual JournalBook JournalBook {
            get { return _journalBook; }
            set { _journalBook = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// DocumentDate (Belegdatum)
        /// </summary>
        public DateTime Date {
            get { return _date; }
            set { _date = value; RaisePropertyChanged(); }
        }

        [NotMapped]
        public string DocumentFullName {
            get {
                if (!String.IsNullOrEmpty(DocumentFolderPath) && !String.IsNullOrEmpty(DocumentName))
                    return Path.Combine(DocumentFolderPath, DocumentName);
                else
                    return String.Empty;
            }
        }

        /// <summary>
        /// Path to the document file
        /// </summary>
        public string DocumentFolderPath {
            get { return _documentFolderPath; }
            set { _documentFolderPath = value; RaisePropertyChanged(); RaisePropertyChanged(nameof(DocumentFullName)); }
        }

        /// <summary>
        /// Name of the document file
        /// </summary>
        public string DocumentName {
            get { return _documentName; }
            set { _documentName = value; RaisePropertyChanged(); RaisePropertyChanged(nameof(DocumentFullName)); }
        }

        /// <summary>
        /// Document number (Belegnummer)
        /// </summary>
        public string DocumentNumber {
            get { return _documentNumber; }
            set { _documentNumber = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// Business process (Geschäftsvorgangsbeschreibung)
        /// </summary>
        public string BusinessProcessDescription {
            get { return _businessProcessDescription; }
            set { _businessProcessDescription = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// Tax rate (Steuersatz)
        /// </summary>
        public decimal TaxRate {
            get { return _taxRate; }
            set { _taxRate = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// Earning (Einnahmen)
        /// </summary>
        public decimal Earning {
            get { return _earning; }
            set { _earning = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// Expenditure (Ausgabe)
        /// </summary>
        public decimal Expenditure {
            get { return _expenditure; }
            set { _expenditure = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// Prepaid tax (Vorsteuer)
        /// </summary>
        public decimal PrepaidTax {
            get { return _prepaidTax; }
            set { _prepaidTax = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// Current cash balance (Bestand)
        /// </summary>
        public decimal CashBalance {
            get { return _cashBalance; }
            set { _cashBalance = value; RaisePropertyChanged(); }
        }

        public virtual ObservableCollection<TaxRate> TaxRates {
            get { return _taxRates; }
            set { _taxRates = value; RaisePropertyChanged(); }
        }
    }
}