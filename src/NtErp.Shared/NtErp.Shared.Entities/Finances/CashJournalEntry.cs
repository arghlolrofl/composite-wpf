﻿using NtErp.Shared.Entities.Base;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;

namespace NtErp.Shared.Entities.Finances {
    public class CashJournalEntry : EntityBase {
        private DateTime _date;
        private string _documentFolderPath;
        private string _documentName;
        private string _processDescription;
        private CashJournal _journal;
        private ObservableCollection<CashJournalEntryPosition> _positions = new ObservableCollection<CashJournalEntryPosition>();


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
        [Required]
        public DateTime Date {
            get { return _date; }
            set { _date = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// Business process (Geschäftsvorgangsbeschreibung)
        /// </summary>
        [Required]
        public string ProcessDescription {
            get { return _processDescription; }
            set { _processDescription = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// Current cash balance (Bestand)
        /// </summary>
        [Required]
        public decimal CashBalance {
            get { return Positions.Sum(p => p.Delta); }
            //set { _cashBalance = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// Navigation property to <see cref="Entities.CashJournal.CashJournal"/>
        /// </summary>
        [Required]
        public virtual CashJournal Journal {
            get { return _journal; }
            set { _journal = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// List of positions for this entry.
        /// </summary>
        public virtual ObservableCollection<CashJournalEntryPosition> Positions {
            get { return _positions; }
            set { _positions = value; RaisePropertyChanged(); }
        }


        public void RefreshCashBalance() {
            RaisePropertyChanged(nameof(CashBalance));
        }

        protected override void RegisterPropertiesToTrack() {
            TrackProperties(nameof(DocumentFolderPath), nameof(DocumentName), nameof(Date), nameof(ProcessDescription));
        }
    }
}