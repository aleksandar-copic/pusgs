using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class Station
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Address { get; set; }

        public virtual ICollection<Line> Lines { get; set; }
    }
}