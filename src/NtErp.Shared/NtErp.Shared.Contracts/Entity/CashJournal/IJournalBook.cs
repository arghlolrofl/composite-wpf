using System;

namespace NtErp.Shared.Contracts.Entity.CashJournal {
    public interface IJournalBook {
        string Description { get; set; }
        DateTime EndDate { get; set; }
        System.Collections.ObjectModel.ObservableCollection<IJournalEntry> Entries { get; set; }
        string Number { get; set; }
        DateTime StartDate { get; set; }
    }
}