using NtErp.Shared.Contracts.Repository;
using NtErp.Shared.DataAccess;
using NtErp.Shared.Entities.Base;
using NtErp.Shared.Entities.Finances;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NtErp.Shared.Repositories {
    public class CashJournalRepository : RepositoryBase<CashJournal>, ICashJournalRepository {
        public CashJournalRepository(NtErpContext context) : base(context) {

        }


        public CashJournal New() {
            return new CashJournal() {
                Id = 0,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(1)
            };
        }

        public override CashJournal Find(long id) {
            var journal = _context.CashJournals.Find(id);

            _context.Entry(journal).Reload();

            return journal;
        }

        public override IEnumerable<CashJournal> Fetch(int maxResultCount = -1) {
            if (maxResultCount < 0)
                return _context.CashJournals.ToList();
            else
                return _context.CashJournals.Take(maxResultCount).ToList();
        }

        public override void Save(EntityBase entity) {
            CashJournal journal = null;

            if (entity.Exists && ((journal = entity as CashJournal) != null))
                _context.CashJournals.Attach(journal);

            base.Save(entity);
        }
    }
}