using System;
using System.Collections.Generic;
using System.Data.Entity;
//using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WebApp.Models;
using WebApp.Persistence;
using WebApp.Persistence.UnitOfWork;

namespace WebApp.Controllers
{
    public class TimeTableController : ApiController
    {
        public IUnitOfWork Db { get; set; }

        private ApplicationDbContext db = new ApplicationDbContext();

        public TimeTableController() { }

        public TimeTableController(IUnitOfWork db)
        {
            this.Db = db;
        }

        // TODO: ovo je staro, ako ne bude trebalo obrisace se.
        [ResponseType(typeof(string))]
        [Route("api/TimeTable/GetTables/{selectedTeritory}/{selectedDay}/{selectedLine}")]
        public IHttpActionResult GetDepartures(int selectedTeritory, int selectedDay, int selectedLine)
        {
            var ret2 = new TimeTable();
            var timeTables = Db.timeTableRepository.GetAll().ToList();

            string ret = "";

            foreach (var ttt in timeTables)
            {
                if (ttt.TimetableTypeId == selectedTeritory && ttt.DayTypeId == selectedDay &&
                    ttt.BusLineId == selectedLine)
                {
                    ret = ttt.Times;
                    break;
                }

                ret = "No departures for selected line";
            }

            if (ret != null)
                return Ok(ret);
            return StatusCode(HttpStatusCode.Accepted);
        }

        [ResponseType(typeof(string))]
        [Route("api/TimeTable/AddTimetable")]
        public IHttpActionResult AddTimeTable(AddTimeTable tt)
        {
            var lineId = int.Parse(tt.lineId);
            var line = db.Line.ToList().Find(x => x.SerialNumber == lineId);

            var timeTable = new TimeTable()
            {
                BusLine = line,
                BusLineId = line.Id,
                DayTypeId = int.Parse(tt.dayTypeId),
                DayType = tt.dayTypeId == "1" ? "Urban" : "Suburban",
                Id = (new Random()).Next(1, 100),
                Times = tt.times,
                TimetableType = tt.timetableTypeId == "1" ? "Work day" : tt.timetableTypeId == "2" ? "Saturday" : "Sunday",
                TimetableTypeId = int.Parse(tt.timetableTypeId)
            };

            db.TimeTable.Add(timeTable);
            db.Entry(timeTable).State = EntityState.Added;
            db.SaveChanges();

            return Ok("success");
        }

        [ResponseType(typeof(string))]
        [Route("api/TimeTable/GetTimetable/{lineId}/{timeTableTypeId}/{dayTypeId}")]
        public IHttpActionResult GetTimeTable(string lineId, string timeTableTypeId, string dayTypeId)
        {
            var timetables = db.TimeTable.ToList();

            lineId = db.Line.ToList().Find(x => x.SerialNumber.ToString() == lineId).Id.ToString();
            foreach (var table in timetables)
            {
                if (table.BusLineId.ToString() != lineId || table.DayTypeId.ToString() != dayTypeId ||
                    table.TimetableTypeId.ToString() != timeTableTypeId) continue;

                return Ok(table.Times);
            }

            return Ok("this timetable does not exist");
        }

        [ResponseType(typeof(string))]
        [Route("api/TimeTable/DeleteTimetable/{lineId}/{timeTableTypeId}/{dayTypeId}")]
        public IHttpActionResult DeleteTimeTable(string lineId, string timeTableTypeId, string dayTypeId)
        {
            var timetables = db.TimeTable;

            lineId = db.Line.ToList().Find(x => x.SerialNumber.ToString() == lineId).Id.ToString();
            TimeTable t = null;
            foreach (var table in timetables)
            {
                if (table.BusLineId.ToString() != lineId || table.DayTypeId.ToString() != dayTypeId ||
                    table.TimetableTypeId.ToString() != timeTableTypeId) continue;

                t = table;
                break;
            }

            if (t == null)
                return Ok("this timetable does not exist.");

            db.TimeTable.Remove(t);
            db.SaveChanges();
            return Ok("success");
        }
    }
}
