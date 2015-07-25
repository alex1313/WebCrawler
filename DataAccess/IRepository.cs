namespace DataAccess
{
    using System.Collections.Generic;

    internal interface IRepository<TEntity>
        where TEntity : IEntity
    {
        IList<TEntity> GetAll();
        void Insert(TEntity entity);
    }
}