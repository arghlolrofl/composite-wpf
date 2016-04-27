using NtErp.Shared.Entities.Base;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace NtErp.Shared.Entities.CashJournal {
    public class JournalBook : EntityBase {
        private string _Number;
        private DateTime _StartDate;
        private DateTime _EndDate;
        private string _Description;
        private ObservableCollection<JournalEntry> _entries;


        private bool _hasChanges;
        public bool HasChanges {
            get { return _hasChanges; }
            set { _hasChanges = value; RaisePropertyChanged(); }
        }



        /// <summary>
        /// A sequential number
        /// </summary>
        public string Number {
            get { return _Number; }
            set { _Number = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// Start date of the journal
        /// </summary>
        public DateTime StartDate {
            get { return _StartDate; }
            set { _StartDate = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// End date of the journal
        /// </summary>
        public DateTime EndDate {
            get { return _EndDate; }
            set { _EndDate = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// Description of the journal (Kassenbuchbeschreibung)
        /// </summary>
        public string Description {
            get { return _Description; }
            set { _Description = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// List of journal entries
        /// </summary>
        public virtual ObservableCollection<JournalEntry> Entries {
            get { return _entries; }
            set { _entries = value; RaisePropertyChanged(); }
        }


        public JournalBook() {
            Entries = new ObservableCollection<JournalEntry>();

            PropertyChanged += (sender, args) => {
                Debug.WriteLine("Changes detected for current journal: " + args.PropertyName);

                switch (args.PropertyName) {
                    case nameof(Number):
                        HasChanges = true;
                        Debug.WriteLine("    > " + Number);
                        break;
                    case nameof(StartDate):
                        HasChanges = true;
                        Debug.WriteLine("    > " + StartDate);
                        break;
                    case nameof(EndDate):
                        HasChanges = true;
                        Debug.WriteLine("    > " + EndDate);
                        break;
                    case nameof(Description):
                        HasChanges = true;
                        Debug.WriteLine("    > " + Description);
                        break;
                    default:
                        break;
                }
            };
        }
    }
}