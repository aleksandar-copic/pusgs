using System.Collections.Generic;

namespace WebApp.Models
{
    public class AddStation
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public List<string> Lines { get; set; }
    }
}