using NtErp.Shared.Services.Base;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace NtErp.Shared.Entities.CashJournal {
    public class CashJournal : EntityBase {
        private string _number;
        private DateTime _startDate;
        private DateTime _endDate;
        private string _description;
        private ObservableCollection<CashJournalEntry> _entries = new ObservableCollection<CashJournalEntry>();


        /// <summary>
        /// A sequential number
        /// </summary>
        [Required]
        public string Number {
            get { return _number; }
            set { _number = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// Start date of the journal
        /// </summary>
        [Required]
        public DateTime StartDate {
            get { return _startDate; }
            set { _startDate = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// End date of the journal
        /// </summary>
        [Required]
        public DateTime EndDate {
            get { return _endDate; }
            set { _endDate = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// Description of the journal (Kassenbuchbeschreibung)
        /// </summary>
        public string Description {
            get { return _description; }
            set { _description = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// List of journal entries
        /// </summary>
        public virtual ObservableCollection<CashJournalEntry> Entries {
            get { return _entries; }
            set { _entries = value; RaisePropertyChanged(); }
        }


        protected override void RegisterPropertiesToTrack() {
            TrackProperties(nameof(Number), nameof(StartDate), nameof(EndDate), nameof(Description));
        }
    }
}