using NtErp.Shared.Contracts.Entity;
using System.Collections.ObjectModel;

namespace NtErp.Shared.Entities.MasterFileData {
    public interface IProduct : IEntityBase {
        ObservableCollection<IProductComponent> Components { get; set; }
        string Description { get; set; }
        string Name { get; set; }
        string Number { get; set; }
        int Version { get; set; }
    }
}