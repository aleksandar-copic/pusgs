using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebApp.Models;

namespace WebApp.Persistence.Repository
{
    public class TimeTableRepository : Repository<TimeTable, int>, ITimeTableRepository
    {
        public TimeTableRepository(DbContext context) : base(context)
        {

        }
    }
}