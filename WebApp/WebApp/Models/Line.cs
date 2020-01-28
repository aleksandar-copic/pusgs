using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class Line
    {
        public int Id { get; set; }

        [Required] public int SerialNumber { get; set; }

        public List<Vehicle> Vehicles { get; set; }
        public virtual ICollection<Station> Stations { get; set; }
        public List<TimeTable> Timetables { get; set; }


    }
}