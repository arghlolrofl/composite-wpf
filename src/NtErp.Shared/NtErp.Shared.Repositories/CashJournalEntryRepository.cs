using NtErp.Shared.Contracts.Repository;
using NtErp.Shared.DataAccess;
using NtErp.Shared.Entities.CashJournal;
using NtErp.Shared.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NtErp.Shared.Repositories {
    public class CashJournalEntryRepository : RepositoryBase<CashJournalEntry>, ICashJournalEntryRepository {
        public CashJournalEntryRepository(NtErpContext context) : base(context) {

        }

        public CashJournalEntry New(CashJournal journal) {
            return new CashJournalEntry() {
                Id = 0,
                Date = DateTime.Now,
                Journal = journal
            };
        }

        public CashJournalEntryPosition NewPosition(CashJournalEntry entry) {
            return new CashJournalEntryPosition() { Entry = entry };
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

        /// <summary>
        /// Saves the entity.
        /// </summary>
        /// <param name="entity"><see cref="CashJournalEntry"/></param>
        public override void Save(EntityBase entity) {
            CashJournalEntry e = entity as CashJournalEntry;
            if (e == null)
                throw new InvalidCastException("Unable to cast entity to CashJournalEntry!");

            if (e.Journal != null)
                _context.CashJournals.Attach(e.Journal);

            if (e.Exists)
                _context.CashJournalEntries.Attach(e);

            base.Save(e);
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
