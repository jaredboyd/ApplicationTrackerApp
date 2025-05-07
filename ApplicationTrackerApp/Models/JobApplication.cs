using System.ComponentModel.DataAnnotations;

namespace ApplicationTrackerApp.Models
{
    public class JobApplication
    {
        public int Id { get; set; }
        [Required]
        public string? Position { get; set; }
        [Required]
        public string? Company { get; set; }
        public DateTime DateApplied { get; set; } = DateTime.Now;

        public int UserId { get; set; }
        public User? User { get; set; }
        public DateTime? DateEdited { get; set; } = null;
        public string? Notes { get; set; }
    }
}
