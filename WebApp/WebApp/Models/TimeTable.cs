using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class TimeTable
    {
        public int Id { get; set; }

        public int TimetableTypeId { get; set; }
        public string TimetableType { get; set; }

        public int DayTypeId { get; set; }
        public string DayType { get; set; }

        public int BusLineId { get; set; }
        public Line BusLine { get; set; }

        public string Times { get; set; }
    }
}