
using NtErp.Shared.Contracts.Entity.MasterFileData;

namespace NtErp.Shared.Contracts.Entity.CashJournal {
    public interface IJournalEntryPosition {
        decimal Delta { get; set; }
        string Description { get; set; }
        IJournalEntry JournalEntry { get; set; }
        decimal PrepaidTax { get; set; }
        ITaxRate TaxRate { get; set; }
    }
}