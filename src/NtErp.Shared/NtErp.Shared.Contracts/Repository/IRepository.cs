using NtErp.Shared.Services.Base;
using System;
using System.Collections.Generic;

namespace NtErp.Shared.Contracts.Repository {
    public interface IRepository<TEntity, in TKey> : IDisposable where TEntity : EntityBase {
        IEnumerable<TEntity> GetAll();
        TEntity GetSingle(TKey id);
        void Save(EntityBase entity);
        void Delete(EntityBase entity);
        void Refresh(EntityBase entity);
    }
}
