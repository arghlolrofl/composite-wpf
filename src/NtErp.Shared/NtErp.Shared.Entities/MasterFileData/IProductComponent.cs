using NtErp.Shared.Contracts.Entity;

namespace NtErp.Shared.Entities.MasterFileData {
    public interface IProductComponent : IEntityBase {
        int Amount { get; set; }
        Product Component { get; set; }
        Product Product { get; set; }
    }
}