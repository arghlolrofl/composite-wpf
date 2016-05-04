using NtErp.Shared.Contracts.Repository;
using NtErp.Shared.DataAccess;
using NtErp.Shared.Entities.CashJournal;
using NtErp.Shared.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NtErp.Shared.Repositories {
    public class JournalEntryRepository : RepositoryBase<JournalEntry>, IJournalEntryRepository {
        public JournalEntryRepository(NtErpContext context) : base(context) {

        }

        public JournalEntry New() {
            return new JournalEntry() {
                Id = 0,
                Date = DateTime.Now
            };
        }

        public JournalEntryPosition NewPosition() {
            return new JournalEntryPosition();
        }

        public override IEnumerable<JournalEntry> GetAll() {
            return _context.CashJournalEntries.ToList();
        }

        public override JournalEntry GetSingle(long id) {
            return _context.CashJournalEntries.Find(id);
        }

        public override void Save(EntityBase entity) {
            JournalEntry e = null;

            if (entity.Exists && ((e = entity as JournalEntry) != null))
                _context.CashJournalEntries.Attach(e);

            base.Save(entity);
        }

        public void AddPosition(JournalEntry entry, JournalEntryPosition position) {
            _context.CashJournalEntries.Attach(entry);

            if (position.Id > 0) {
                _context.CashJournalEntryPositions.Attach(position);
            }

            entry.Positions.Add(position);

            _context.SaveChanges();
        }

        public void RemovePosition(JournalEntryPosition position) {
            if (position.Id > 0)
                _context.CashJournalEntryPositions.Attach(position);

            _context.Entry(position).State = System.Data.Entity.EntityState.Deleted;

            _context.SaveChanges();
        }
    }
}
