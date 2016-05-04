using NtErp.Shared.Entities.CashJournal;

namespace NtErp.Shared.Contracts.Repository {
    public interface IJournalBookRepository : IRepository<JournalBook, long> {
        JournalBook New();
    }
}
