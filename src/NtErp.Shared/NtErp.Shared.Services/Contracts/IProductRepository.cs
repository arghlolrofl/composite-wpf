using NtErp.Shared.Entities.MasterFileData;
using System.Collections.Generic;

namespace NtErp.Shared.Services.Contracts {
  public interface IProductRepository : IRepository<Product, long> {
    IEnumerable<Product> FindAll();
    IEnumerable<Product> Find(long key);
    Product New(int version = 1);

    Product AddComponent(Product component);
    Product RemoveComponent(Product component);
  }
}
