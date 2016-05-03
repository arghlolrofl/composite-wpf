using NtErp.Shared.Entities.CashJournal;

namespace NtErp.Shared.Services.Contracts {
    public interface IJournalBookRepository : IRepository<JournalBook, long> {
        JournalBook New();
    }
}
