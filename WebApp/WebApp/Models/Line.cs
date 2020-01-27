using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class Line : EqualityComparer<Line>
    {
        public int Id { get; set; }

        [Required]
        public int SerialNumber { get; set; }

        public List<Vehicle> Vehicles { get; set; }
        public virtual ICollection<Station> Stations { get; set; }
        public List<TimeTable> Timetables { get; set; }


        // Comparer methods.
        public override bool Equals(Line l1, Line l2)
        {
            return l1?.SerialNumber == l2?.SerialNumber;
        }

        public override int GetHashCode(Line l)
        {
            return l.GetHashCode();
        }
    }
}