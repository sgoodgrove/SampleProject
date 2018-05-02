using LiteDB;

namespace WebApplication1.Models
{
    public class User
    {
        public Address Address { get; set; }

        public Company Company { get; set; }

        public string Email { get; set; }

        [BsonId]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public string UserName { get; set; }

        public string Website { get; set; }
    }
}