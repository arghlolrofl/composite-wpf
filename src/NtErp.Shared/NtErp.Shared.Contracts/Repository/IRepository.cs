using NtErp.Shared.Entities.Base;
using System.Collections.Generic;

namespace NtErp.Shared.Contracts.Repository {
    public interface IRepository<TEntity, in TKey> where TEntity : EntityBase {
        IEnumerable<TEntity> Fetch(int maxResultCount = -1);
        TEntity Find(TKey id);
        void Save(EntityBase entity);
        void Delete(EntityBase entity);
        void Refresh(EntityBase entity);
    }
}
