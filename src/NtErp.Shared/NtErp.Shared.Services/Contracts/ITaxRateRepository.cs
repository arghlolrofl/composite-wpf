using NtErp.Shared.Entities.MasterFileData;

namespace NtErp.Shared.Services.Contracts {
    public interface ITaxRateRepository : IRepository<TaxRate, long> {
        TaxRate New();
    }
}
