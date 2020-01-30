using System;
using System.Collections.Generic;
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


        //[Route("api/RedVoznje/getAll")]
        //[HttpGet]
        //public IHttpActionResult GetAll()
        //{
        //    var lines = Db.red.GetAll();

        //    return Ok(lines);
        //}


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
            var line = Db.lineRepository.Find(x => x.Id == int.Parse(tt.lineId)).FirstOrDefault();

            var timeTable = new TimeTable()
            {
                BusLine = line,
                BusLineId = line.Id,
                DayTypeId = int.Parse(tt.dayTypeId),
                DayType = tt.dayTypeId == "1" ? "Urban" : "Suburban",
                Id = (new Random()).Next(1, 100),
                Times = tt.times,
                TimetableType = tt.Id == "1" ? "Work day" : tt.Id == "2" ? "Saturday" : "Sunday"
            };

            line.Timetables.Add(timeTable);

            return Ok("success");
        }

        [ResponseType(typeof(string))]
        [Route("api/TimeTable/AddTimetable")]
        public IHttpActionResult GetTimeTable(AddTimeTable tt)
        {
            var line = Db.lineRepository.Find(x => x.Id == int.Parse(tt.lineId)).FirstOrDefault();

            if (line.Timetables == null)
                return Ok("no timetables for selected line");

            TimeTable timeTable = null;
            foreach (var table in line.Timetables)
            {
                if (table.Id.ToString() == tt.Id && table.DayTypeId.ToString() == tt.dayTypeId &&
                    table.TimetableTypeId.ToString() == tt.Id)
                {
                    timeTable = table;
                    break;
                }
            }

            return Ok(timeTable == null ? "this timetable does not exist." : timeTable.Times);
        }
    }
}
