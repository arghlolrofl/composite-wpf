using NtErp.Shared.Entities.MasterFileData;
using System.Data.Entity.ModelConfiguration;

namespace NtErp.Data.Migrations {
    public class ProductConfiguration : EntityTypeConfiguration<Product> {
        public ProductConfiguration() {
            //this.HasMany(c => c.Components)
            //    .WithMany(c => c.Products)
            //    .Map(pc => {
            //        pc.MapLeftKey("ProductId");
            //        pc.MapRightKey("ComponentId");
            //        pc.ToTable("ProductComponents");
            //    });
        }
    }
}
