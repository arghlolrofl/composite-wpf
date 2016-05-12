using NtErp.Shared.Contracts.Repository;
using NtErp.Shared.DataAccess;
using NtErp.Shared.Entities.CashJournal;
using NtErp.Shared.Entities.MasterFileData;
using NtErp.Shared.Services.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
            TaxRate defaultTaxRate = _context.TaxRates.Where(tr => tr.Category == "VAT").Single();

            return new CashJournalEntryPosition() { Entry = entry, TaxRate = defaultTaxRate };
        }

        public override IEnumerable<CashJournalEntry> Fetch(int maxResultCount = -1) {
            if (maxResultCount < 0)
                return _context.CashJournalEntries.ToList();
            else
                return _context.CashJournalEntries.Take(maxResultCount).ToList();
        }

        public override CashJournalEntry Find(long id) {
            return _context.CashJournalEntries.Where(e => e.Id == id).Include(e => e.Positions).FirstOrDefault();
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
                _context.Entry(entry).State = System.Data.Entity.EntityState.Modified;
            } else
                _context.Entry(position).State = System.Data.Entity.EntityState.Added;

            _context.SaveChanges();
        }

        public void RemovePosition(CashJournalEntryPosition position) {
            if (position.Id > 0)
                _context.CashJournalEntryPositions.Attach(position);

            _context.Entry(position).State = System.Data.Entity.EntityState.Deleted;

            _context.SaveChanges();
        }

        public override void Delete(EntityBase entity) {
            var entry = entity as CashJournalEntry;
            if (entry == null)
                throw new InvalidCastException("Unable to cast EntityBase to CashJournalEntry!");

            if (entry.Exists) {
                // Deletes all positions of this entry too.
                _context.CashJournalEntries.Remove(entry);
                _context.SaveChanges();
            }
        }
    }
}
