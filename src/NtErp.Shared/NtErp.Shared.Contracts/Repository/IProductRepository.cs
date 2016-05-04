using NtErp.Shared.Entities.MasterFileData;

namespace NtErp.Shared.Contracts.Repository {
    public interface IProductRepository : IRepository<Product, long> {
        Product New(int version = 1);

        Product AddComponent(Product kit, Product component);
        Product RemoveComponent(ProductComponent component);
    }
}
