using NtErp.Shared.Entities.Finances;

namespace NtErp.Shared.Contracts.Repository {
    public interface ICashJournalRepository : IRepository<CashJournal, long> {
        CashJournal New();
    }
}
