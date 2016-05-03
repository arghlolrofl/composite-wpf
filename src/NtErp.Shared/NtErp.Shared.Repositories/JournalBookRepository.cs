using NtErp.Shared.DataAccess;
using NtErp.Shared.Entities.CashJournal;
using NtErp.Shared.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace NtErp.Shared.Repositories {
    public class JournalBookRepository : IJournalBookRepository {
        private readonly NtErpContext _context;

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

        //public JournalEntry NewEntry(JournalBook book) {
        //    return new JournalEntry() {
        //        Id = 0,
        //        Date = DateTime.Now,
        //        JournalBook = book
        //    };
        //}

        public JournalBook GetSingle(long id) {
            var book = _context.CashJournals.Find(id);

            _context.Entry(book).Reload();

            return book;
        }

        public IEnumerable<JournalBook> GetAll() {
            return _context.CashJournals.ToList();
        }

        public void Save(JournalBook entity) {
            if (entity.Id > 0) {
                _context.CashJournals.Attach(entity);
                _context.Entry(entity).State = EntityState.Modified;
            } else {
                _context.Entry(entity).State = EntityState.Added;
            }

            _context.SaveChanges();

            entity.UpdateTrackedProperties();
        }

        public void Delete(JournalBook entity) {
            _context.Entry(entity).State = EntityState.Deleted;

            _context.SaveChanges();
        }

        //public void UpdateEntry(JournalEntry entry) {

        //    try {
        //        if (entry.JournalBook != null)
        //            _context.CashJournals.Attach(entry.JournalBook);

        //        if (entry.Id > 0) {
        //            _context.CashJournalEntries.Attach(entry);
        //            _context.Entry(entry).State = EntityState.Modified;
        //        } else {
        //            _context.Entry(entry).State = EntityState.Added;
        //        }

        //        _context.SaveChanges();
        //    } catch (DbEntityValidationException ex) {
        //        Debug.WriteLine("ERROR: " + ex.GetType().Name);
        //        Debug.WriteLine("MESSAGE: " + ex.Message);
        //        Debugger.Break();
        //    }
        //}

        //public void DeleteEntry(JournalEntry entry) {
        //    _context.Entry(entry).State = EntityState.Deleted;

        //    _context.SaveChanges();
        //}

        public void Refresh(JournalBook entity) {
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