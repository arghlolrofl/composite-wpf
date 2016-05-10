using NtErp.Shared.Contracts.Repository;
using NtErp.Shared.DataAccess;
using NtErp.Shared.Services.Base;
using System;
using System.Collections.Generic;

namespace NtErp.Shared.Repositories {
    public abstract class RepositoryBase<TEntity> : IDisposable, IRepository<TEntity, long> where TEntity : EntityBase {
        protected NtErpContext _context;

        public RepositoryBase(NtErpContext context) {
            _context = context;
        }

        public abstract IEnumerable<TEntity> Fetch(int maxResultCount = -1);

        public abstract TEntity Find(long id);

        public virtual void Save(EntityBase entity) {
            if (entity.Exists)
                _context.Entry(entity).State = System.Data.Entity.EntityState.Modified;
            else
                _context.Entry(entity).State = System.Data.Entity.EntityState.Added;

            _context.SaveChanges();

            entity.ResetChangedProperties();
        }

        public virtual void Refresh(EntityBase entity) {
            _context.Entry(entity).Reload();

            entity.ResetChangedProperties();
        }

        public void Delete(EntityBase entity) {
            _context.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
            _context.SaveChanges();

            entity = null;
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
