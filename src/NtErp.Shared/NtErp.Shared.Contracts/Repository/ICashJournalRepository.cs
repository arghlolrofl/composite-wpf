using NtErp.Shared.Entities.CashJournal;

namespace NtErp.Shared.Contracts.Repository {
    public interface ICashJournalRepository : IRepository<CashJournal, long> {
        CashJournal New();
    }
}
