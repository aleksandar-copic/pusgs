using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class TicketType
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}