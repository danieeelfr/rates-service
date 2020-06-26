using System;

namespace Core.Models.Users
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public string Password { get; set; }

        public DateTime LastUpdate { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
