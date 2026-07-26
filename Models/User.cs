using System;

namespace FadTask.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; } // plaintext for demo only
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
