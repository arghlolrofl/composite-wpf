using System;

namespace NtErp.Shared.Contracts.Entity.CashJournal {
    public interface IJournalEntry {
        decimal CashBalance { get; set; }
        DateTime Date { get; set; }
        string DocumentFolderPath { get; set; }
        string DocumentName { get; set; }
        IJournalBook JournalBook { get; set; }
        System.Collections.ObjectModel.ObservableCollection<IJournalEntryPosition> Positions { get; set; }
        string ProcessDescription { get; set; }
    }
}