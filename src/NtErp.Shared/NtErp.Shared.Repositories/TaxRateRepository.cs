using NtErp.Shared.Contracts.Repository;
using NtErp.Shared.DataAccess;
using NtErp.Shared.Entities.MasterFileData;
using NtErp.Shared.Services.Base;
using System.Collections.Generic;
using System.Linq;

namespace NtErp.Shared.Repositories {
    public class TaxRateRepository : RepositoryBase<TaxRate>, ITaxRateRepository {
        public TaxRateRepository(NtErpContext context) : base(context) {

        }

        public TaxRate New() {
            return new TaxRate() {
                Id = 0,
                Category = "Default",
                Description = "None",
                Value = 19.00m
            };
        }

        public override TaxRate GetSingle(long id) {
            return _context.TaxRates.Find(id);
        }

        public override IEnumerable<TaxRate> GetAll() {
            return _context.TaxRates.ToList();
        }

        public override void Save(EntityBase entity) {
            TaxRate tr = null;

            if (entity.Exists && ((tr = entity as TaxRate) != null))
                _context.TaxRates.Attach(tr);

            base.Save(entity);
        }
    }
}
