using NtErp.Shared.Entities.MasterFileData;

namespace NtErp.Shared.Contracts.Repository {
    public interface ITaxRateRepository : IRepository<TaxRate, long> {
        TaxRate New();
    }
}
