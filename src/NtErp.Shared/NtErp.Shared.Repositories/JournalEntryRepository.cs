using NtErp.Shared.Contracts.Repository;
using NtErp.Shared.DataAccess;
using NtErp.Shared.Entities.CashJournal;
using NtErp.Shared.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NtErp.Shared.Repositories {
    public class JournalEntryRepository : RepositoryBase<CashJournalEntry>, IJournalEntryRepository {
        public JournalEntryRepository(NtErpContext context) : base(context) {

        }

        public CashJournalEntry New() {
            return new CashJournalEntry() {
                Id = 0,
                Date = DateTime.Now
            };
        }

        public CashJournalEntryPosition NewPosition() {
            return new CashJournalEntryPosition();
        }

        public override IEnumerable<CashJournalEntry> Fetch(int maxResultCount = -1) {
            if (maxResultCount < 0)
                return _context.CashJournalEntries.ToList();
            else
                return _context.CashJournalEntries.Take(maxResultCount).ToList();
        }

        public override CashJournalEntry Find(long id) {
            return _context.CashJournalEntries.Find(id);
        }

        public override void Save(EntityBase entity) {
            CashJournalEntry e = null;

            if (entity.Exists && ((e = entity as CashJournalEntry) != null))
                _context.CashJournalEntries.Attach(e);

            base.Save(entity);
        }

        public void AddPosition(CashJournalEntry entry, CashJournalEntryPosition position) {
            _context.CashJournalEntries.Attach(entry);

            if (position.Id > 0) {
                _context.CashJournalEntryPositions.Attach(position);
            }

            entry.Positions.Add(position);

            _context.SaveChanges();
        }

        public void RemovePosition(CashJournalEntryPosition position) {
            if (position.Id > 0)
                _context.CashJournalEntryPositions.Attach(position);

            _context.Entry(position).State = System.Data.Entity.EntityState.Deleted;

            _context.SaveChanges();
        }
    }
}
