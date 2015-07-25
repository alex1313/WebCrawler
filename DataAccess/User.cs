namespace DataAccess
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    public class User : IEntity
    {
        public User()
        {
            Id = ObjectId.GenerateNewId();
        }

        [BsonId]
        public ObjectId Id { get; set; }
        public long VkId { get; set; }
        public int Age { get; set; }
        public int FriendsCount { get; set; }
    }
}