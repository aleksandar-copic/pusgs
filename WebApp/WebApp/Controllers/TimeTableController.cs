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
        public IHttpActionResult GetPolasci(int selectedTeritory, int selectedDay, int selectedLine)
        {
            TimeTable ret2 = new TimeTable();
            var lines = Db.timeTableRepository.GetAll().ToList();

            string ret = "";                 //vremena
            List<Line> line = new List<Line>();

            line = Db.lineRepository.GetAll().ToList();

            foreach (TimeTable ttt in lines)
            {
                if (ttt.TimetableTypeId == selectedTeritory && ttt.DayTypeId == selectedDay && ttt.BusLineId == selectedLine) {
                    ret = ttt.Times;
                    break;
                }
                else
                {
                    ret = "Trenutno nema polazaka za odabranu liniju";
                }
            }

            if (ret != null)
                return Ok(ret);
            else
                return StatusCode(HttpStatusCode.Accepted);

        }
    }
}
