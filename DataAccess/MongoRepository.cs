namespace DataAccess
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using MongoDB.Driver;

    public class MongoRepository<TEntity> : IRepository<TEntity>
        where TEntity : IEntity
    {
        private const string ConnectionStringSettingsField = "ConnectionString";
        private const string UsersDatabaseNameSettingsField = "UsersDatabaseName";
        private readonly MongoCollection<TEntity> _collection;

        public MongoRepository()
        {
            var client = new MongoClient(ConfigurationManager.AppSettings[ConnectionStringSettingsField]);
            var server = client.GetServer();
            var database = server.GetDatabase(ConfigurationManager.AppSettings[UsersDatabaseNameSettingsField]);

            _collection = database.GetCollection<TEntity>(typeof (TEntity).Name);
        }

        public IList<TEntity> GetAll()
        {
            return _collection
                .FindAllAs<TEntity>()
                .ToList();
        }

        public void Insert(TEntity entity)
        {
            _collection.Insert(entity);
        }
    }
}