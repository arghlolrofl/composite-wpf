using NtErp.Shared.Contracts.Repository;
using NtErp.Shared.DataAccess;
using NtErp.Shared.Entities.CashJournal;
using NtErp.Shared.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NtErp.Shared.Repositories {
    public class JournalBookRepository : RepositoryBase<JournalBook>, IJournalBookRepository {
        public JournalBookRepository(NtErpContext context) : base(context) {

        }


        public JournalBook New() {
            return new JournalBook() {
                Id = 0,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(1)
            };
        }

        public override JournalBook GetSingle(long id) {
            var book = _context.CashJournals.Find(id);

            _context.Entry(book).Reload();

            return book;
        }

        public override IEnumerable<JournalBook> GetAll() {
            return _context.CashJournals.ToList();
        }

        public override void Save(EntityBase entity) {
            JournalBook book = null;

            if (entity.Exists && ((book = entity as JournalBook) != null))
                _context.CashJournals.Attach(book);

            base.Save(entity);
        }
    }
}