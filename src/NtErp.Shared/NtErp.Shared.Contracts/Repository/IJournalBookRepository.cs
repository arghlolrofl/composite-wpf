using NtErp.Shared.Entities.CashJournal;

namespace NtErp.Shared.Contracts.Repository {
    public interface IJournalBookRepository : IRepository<CashJournal, long> {
        CashJournal New();
    }
}
