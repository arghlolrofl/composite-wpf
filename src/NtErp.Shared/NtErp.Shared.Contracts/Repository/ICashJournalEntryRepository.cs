using NtErp.Shared.Entities.CashJournal;

namespace NtErp.Shared.Contracts.Repository {
    public interface ICashJournalEntryRepository : IRepository<CashJournalEntry, long> {
        CashJournalEntry New(CashJournal journal);

        CashJournalEntryPosition NewPosition(CashJournalEntry parentEntry);

        void AddPosition(CashJournalEntry entry, CashJournalEntryPosition position);

        void RemovePosition(CashJournalEntryPosition position);
    }
}
