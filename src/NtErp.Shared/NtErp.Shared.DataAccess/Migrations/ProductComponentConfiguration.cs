using NtErp.Shared.Entities.MasterFileData;
using System.Data.Entity.ModelConfiguration;

namespace NtErp.Data.Migrations {
    public class ProductComponentConfiguration : EntityTypeConfiguration<ProductComponent> {
        public ProductComponentConfiguration() {
            this.HasRequired(pc => pc.Product)
                .WithMany(p => p.Components);
        }
    }
}
