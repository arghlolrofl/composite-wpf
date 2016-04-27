using NtErp.Shared.DataAccess;
using NtErp.Shared.Entities.CashJournal;
using NtErp.Shared.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace NtErp.Shared.Repositories {
    public class JournalBookRepository : IJournalBookRepository {
        private NtErpContext _context;

        public JournalBookRepository(NtErpContext context) {
            _context = context;
        }


        public JournalBook New() {
            return new JournalBook() {
                Id = 0,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(1)
            };
        }

        public JournalBook GetSingle(long id) {
            var book = _context.CashJournals.Include(j => j.Entries).Single(j => j.Id == id);
            return book;
        }

        public IEnumerable<JournalBook> GetAll() {
            return _context.CashJournals.ToList();
        }

        public IEnumerable<JournalBook> Find(long key) {
            throw new NotImplementedException();
        }

        public IEnumerable<JournalBook> FindAll() {
            throw new NotImplementedException();
        }

        public void Save(JournalBook entity) {
            if (entity.Id > 0) {
                _context.CashJournals.Attach(entity);
                _context.Entry(entity).State = EntityState.Modified;
            } else {
                _context.Entry(entity).State = EntityState.Added;
            }

            _context.SaveChanges();
        }

        public void Delete(JournalBook entity) {
            _context.Entry(entity).State = EntityState.Deleted;

            _context.SaveChanges();
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