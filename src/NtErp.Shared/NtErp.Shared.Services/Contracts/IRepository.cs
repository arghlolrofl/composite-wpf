using NtErp.Shared.Entities.Base;
using System;
using System.Collections.Generic;

namespace NtErp.Shared.Services.Contracts {
    public interface IRepository<TEntity, in TKey> : IDisposable where TEntity : EntityBase {
        IEnumerable<TEntity> GetAll();
        TEntity GetSingle(TKey id);
        void Save(TEntity entity);
        void Delete(TEntity entity);
        void Refresh(TEntity entity);
    }
}
