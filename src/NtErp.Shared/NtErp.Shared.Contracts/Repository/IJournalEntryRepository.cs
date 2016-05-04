using NtErp.Shared.Entities.CashJournal;

namespace NtErp.Shared.Contracts.Repository {
    public interface IJournalEntryRepository : IRepository<JournalEntry, long> {
        JournalEntry New();

        JournalEntryPosition NewPosition();

        void AddPosition(JournalEntry entry, JournalEntryPosition position);

        void RemovePosition(JournalEntryPosition position);
    }
}
