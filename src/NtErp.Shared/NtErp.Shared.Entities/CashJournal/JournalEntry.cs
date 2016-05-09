using NtErp.Shared.Services.Base;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace NtErp.Shared.Entities.CashJournal {
    public class JournalEntry : EntityBase {
        private DateTime _date;
        private string _documentFolderPath;
        private string _documentName;
        private string _processDescription;
        private decimal _cashBalance;
        private JournalBook _journalBook;
        private ObservableCollection<JournalEntryPosition> _positions = new ObservableCollection<JournalEntryPosition>();


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
        /// DocumentDate (Belegdatum)
        /// </summary>
        public DateTime Date {
            get { return _date; }
            set { _date = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// Business process (Geschäftsvorgangsbeschreibung)
        /// </summary>
        public string ProcessDescription {
            get { return _processDescription; }
            set { _processDescription = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// Current cash balance (Bestand)
        /// </summary>
        public decimal CashBalance {
            get { return _cashBalance; }
            set { _cashBalance = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// Navigation property to <see cref="JournalBook"/>
        /// </summary>
        public virtual JournalBook JournalBook {
            get { return _journalBook; }
            set { _journalBook = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// List of positions for this entry.
        /// </summary>
        public virtual ObservableCollection<JournalEntryPosition> Positions {
            get { return _positions; }
            set { _positions = value; RaisePropertyChanged(); }
        }


        protected override void RegisterPropertiesToTrack() {
            TrackProperties(nameof(DocumentFolderPath), nameof(DocumentName), nameof(Date), nameof(ProcessDescription));
        }
    }
}