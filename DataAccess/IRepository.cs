namespace DataAccess
{
    using System.Collections.Generic;

    public interface IRepository<TEntity>
        where TEntity : IEntity
    {
        IList<TEntity> GetAll();
        void Insert(TEntity entity);
    }
}