using NtErp.Shared.Contracts.Repository;
using NtErp.Shared.DataAccess;
using NtErp.Shared.Entities.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace NtErp.Shared.Repositories {
    public abstract class RepositoryBase<TEntity> : IDisposable, IRepository<TEntity, long> where TEntity : EntityBase {
        protected NtErpContext _context;

        public RepositoryBase(NtErpContext context) {
            _context = context;
        }

        public abstract IEnumerable<TEntity> Fetch(int maxResultCount = -1);

        public abstract TEntity Find(long id);

        public virtual void Save(EntityBase entity) {
            try {
                if (entity.Exists)
                    _context.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                else
                    _context.Entry(entity).State = System.Data.Entity.EntityState.Added;

                _context.SaveChanges();

                entity.ResetChangedProperties();
            } catch (DbEntityValidationException ex) {
                Debug.WriteLine(ex.GetType().Name.ToUpper());
                Debug.WriteLine(ex.Message);
                foreach (DbEntityValidationResult result in ex.EntityValidationErrors) {
                    Debug.WriteLine(" > Entity: " + result.Entry.GetType().Name);
                    foreach (DbValidationError error in result.ValidationErrors)
                        Debug.WriteLine("       > ERROR in Property " + error.PropertyName + ": " + error.ErrorMessage);

                }
            }
        }

        public virtual void Refresh(EntityBase entity) {
            _context.Entry(entity).Reload();

            entity.ResetChangedProperties();
        }

        public virtual void Delete(EntityBase entity) {
            try {

                _context.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
                _context.SaveChanges();

            } catch (DbEntityValidationException ex) {
                Debug.WriteLine(Environment.NewLine + "Got Validation Errors:");

                foreach (var eve in ex.EntityValidationErrors) {
                    Debug.WriteLine(" > " + eve.Entry.Entity.GetType().Name);

                    foreach (var err in eve.ValidationErrors) {
                        Debug.WriteLine("    > " + err.PropertyName + " --> " + err.ErrorMessage + Environment.NewLine);
                    }
                }
            }
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
