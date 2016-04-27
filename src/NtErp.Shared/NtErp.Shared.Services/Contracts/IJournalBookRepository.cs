﻿using NtErp.Shared.Entities.CashJournal;
using System.Collections.Generic;

namespace NtErp.Shared.Services.Contracts {
    public interface IJournalBookRepository : IRepository<JournalBook, long> {
        IEnumerable<JournalBook> FindAll();
        IEnumerable<JournalBook> Find(long key);
        JournalBook New();
    }
}
