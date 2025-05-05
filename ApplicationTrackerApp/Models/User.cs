using System.ComponentModel.DataAnnotations;

namespace ApplicationTrackerApp.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? Password { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

    }
}
