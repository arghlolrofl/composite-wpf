using NtErp.Shared.Entities.MasterFileData;
using System.Collections.Generic;

namespace NtErp.Shared.Services.Contracts {
    public interface ITaxRateRepository : IRepository<TaxRate, long> {
        IEnumerable<TaxRate> FindAll();
        IEnumerable<TaxRate> Find(long key);
        TaxRate New();
    }
}
