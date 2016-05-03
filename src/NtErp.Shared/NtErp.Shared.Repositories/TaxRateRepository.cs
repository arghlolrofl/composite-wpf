using NtErp.Shared.DataAccess;
using NtErp.Shared.Entities.MasterFileData;
using NtErp.Shared.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace NtErp.Shared.Repositories {
    public class TaxRateRepository : ITaxRateRepository {
        private readonly NtErpContext _context;

        public TaxRateRepository(NtErpContext context) {
            _context = context;
        }

        public TaxRate New() {
            return new TaxRate() {
                Id = 0,
                Category = "Default",
                Description = "None",
                Value = 19.00m
            };
        }

        public TaxRate GetSingle(long id) {
            return _context.TaxRates.Find(id);
        }

        public IEnumerable<TaxRate> GetAll() {
            return _context.TaxRates.ToList();
        }

        public IEnumerable<TaxRate> Find(long key) {
            throw new NotImplementedException();
        }

        public IEnumerable<TaxRate> FindAll() {
            throw new NotImplementedException();
        }

        public void Save(TaxRate entity) {
            if (entity.Id > 0) {
                _context.TaxRates.Attach(entity);
                _context.Entry(entity).State = EntityState.Modified;
            } else {
                _context.Entry(entity).State = EntityState.Added;
            }

            _context.SaveChanges();

            entity.UpdateTrackedProperties();
        }
        public void Delete(TaxRate entity) {
            _context.Entry(entity).State = EntityState.Deleted;

            _context.SaveChanges();
        }

        public void Refresh(TaxRate entity) {
            _context.Entry(entity).Reload();
        }

        #region IDisposable

        private bool disposed = false;

        protected virtual void Dispose(bool disposing) {
            if (!this.disposed) {
                if (disposing) {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
