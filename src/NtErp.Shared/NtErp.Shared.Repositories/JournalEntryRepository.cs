using NtErp.Shared.DataAccess;
using NtErp.Shared.Entities.CashJournal;
using NtErp.Shared.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NtErp.Shared.Repositories {
    public class JournalEntryRepository : IJournalEntryRepository {
        private NtErpContext _context;

        public JournalEntryRepository(NtErpContext context) {
            _context = context;
        }

        public IEnumerable<JournalEntry> GetAll() {
            return _context.CashJournalEntries.ToList();
        }

        public JournalEntry GetSingle(long id) {
            return _context.CashJournalEntries.Find(id);
        }

        public JournalEntry New() {
            return new JournalEntry() {
                Id = 0,
                Date = DateTime.Now
            };
        }

        public void Refresh(JournalEntry entity) {
            _context.Entry(entity).Reload();
        }

        public void Save(JournalEntry entity) {
            if (entity.Id > 0) {
                _context.CashJournalEntries.Attach(entity);
                _context.Entry(entity).State = System.Data.Entity.EntityState.Modified;
            } else {
                _context.Entry(entity).State = System.Data.Entity.EntityState.Added;
            }

            _context.SaveChanges();
        }

        public void Delete(JournalEntry entity) {
            if (entity.Id > 0) {
                _context.Entry(entity).State = System.Data.Entity.EntityState.Deleted;

                _context.SaveChanges();
            } else
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
