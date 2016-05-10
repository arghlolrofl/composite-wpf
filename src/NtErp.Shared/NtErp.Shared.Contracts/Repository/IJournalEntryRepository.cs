using NtErp.Shared.Entities.CashJournal;

namespace NtErp.Shared.Contracts.Repository {
    public interface IJournalEntryRepository : IRepository<CashJournalEntry, long> {
        CashJournalEntry New();

        CashJournalEntryPosition NewPosition();

        void AddPosition(CashJournalEntry entry, CashJournalEntryPosition position);

        void RemovePosition(CashJournalEntryPosition position);
    }
}
