using NtErp.Shared.Entities.CashJournal;

namespace NtErp.Shared.Services.Contracts {
    public interface IJournalEntryRepository : IRepository<JournalEntry, long> {
        JournalEntry New();
    }
}
