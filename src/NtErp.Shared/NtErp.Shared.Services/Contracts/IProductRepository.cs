using NtErp.Shared.Entities.MasterFileData;
using System.Collections.Generic;

namespace NtErp.Shared.Services.Contracts {
    public interface IProductRepository : IRepository<Product, long> {
        IEnumerable<Product> FindAll();
    }
}
