using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
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
    public class LineController : ApiController
    {
        public IUnitOfWork Db { get; set; }

        private ApplicationDbContext db = new ApplicationDbContext();

        public LineController() { }

        public LineController(IUnitOfWork db)
        {
            this.Db = db;
        }

        [Route("api/LineEdit/getAll")]
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            var lines = Db.lineRepository.GetAll();

            return Ok(lines);
        }


        // GET: api/LineEdit/Lines
        [AllowAnonymous]
        [ResponseType(typeof(List<int>))]
        [Route("api/LineEdit/Lines")]
        public IHttpActionResult GetLines()
        {
            var line = Db.lineRepository.GetAll().ToList();

            var ret = new List<int>();
            foreach (var l in line)
                ret.Add(l.SerialNumber);

            return Ok(ret);
        }

        // GET: api/LineEdit/SelectedLine
        [AllowAnonymous]
        [ResponseType(typeof(Line))]
        [Route("api/LineEdit/SelectedLine/{serial}")]
        public IHttpActionResult GetSelectedLine(string serial)
        {
            var line = Db.lineRepository.GetAll().ToList();
            int serialNumber = Int32.Parse(serial);

            bool found;
            foreach (var l in line)
            {
                if (!l.SerialNumber.Equals(serialNumber)) continue;

                found = true;
                break;
            }

            // FIXME: vratiti listu svih stanica.
            // resava problem prikaza Serial Number-a za liniju.
            // FIXME: BUDZ
            var ret = new Line() {SerialNumber = serialNumber};

            return Ok(ret);

        }

        // GET: api/LineEdit/SelectedLine
        //[Authorize(Roles = "Admin")]
        [ResponseType(typeof(List<string>))]
        [Route("api/LineEdit/GetStations/{serial}")]
        public IHttpActionResult GetStations(string serial)
        {
            var lines = Db.lineRepository.GetAll().ToList();
            var serialNumber = int.Parse(serial);

            Line ret = null;
            foreach (var l in lines)
            {
                if (!l.SerialNumber.Equals(serialNumber)) continue;

                ret = l;
                break;
            }

            if (ret == null)
                return StatusCode(HttpStatusCode.BadRequest);

            var stations = Db.stationRepository.GetAll().ToList();

            var returnsList = new List<string>();

            foreach (var station in stations)
            {
                foreach (var line in station.Lines)
                {
                    if (line.SerialNumber != ret.SerialNumber)
                        continue;
                    returnsList.Add(station.Name);
                    break;
                }
            }

            return Ok(returnsList);
        }

        // GET: api/LineEdit/SelectedLine
        [Authorize(Roles = "Admin")]
        [ResponseType(typeof(Station))]
        [Route("api/LineEdit/GetSelectedStation/{name}")]
        public IHttpActionResult GetSelectedStation(string name)
        {
            var stations = Db.stationRepository.GetAll().ToList();

            foreach (var l in stations)
            {
                if (!l.Name.Equals(name)) continue;

                return Ok(l);
            }

            return StatusCode(HttpStatusCode.BadRequest);
        }

        // DELETE: api/LineEdit/DeleteSelectedLine/{serial}
        [Authorize(Roles = "Admin")]
        [ResponseType(typeof(string))]
        [Route("api/LineEdit/DeleteSelectedLine/{serial}")]
        public IHttpActionResult DeleteSelectedLine(string serial)
        {
            var lines = db.Line.ToList();
            var serialNumber = int.Parse(serial);

            Line ret = null;
            foreach (var l in lines)
            {
                if (!l.SerialNumber.Equals(serialNumber)) continue;

                ret = l;
                break;
            }

            if (ret == null)
                return StatusCode(HttpStatusCode.BadRequest);

            db.Entry(ret).State = EntityState.Deleted;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException e)
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }

            return Ok("success");
        }

        // POST: api/Line/AddLine
        [Authorize(Roles = "Admin")]
        [ResponseType(typeof(string))]
        [Route("api/Line/AddLine/{serial}")]
        public IHttpActionResult AddLine(string serial)
        {
            var found = false;
            var lines = Db.lineRepository.GetAll().ToList();

            var sNumber = int.Parse(serial);
            foreach (var l in lines)
            {
                if (!l.SerialNumber.Equals(sNumber)) continue;

                found = true;
                break;
            }

            if (found)
                return StatusCode(HttpStatusCode.BadRequest);

            
            var ret = new Line
            {
                SerialNumber = sNumber,
                Stations = new List<Station>(),
                Id = sNumber,
                Timetables = new List<TimeTable>()
            };

            // ovde se ne dodaju stanice, vec samo prazna linija.
            // linije se povezuju sa stanicom prilikom dodavanja nove stanice, ili edita neke stanice.
            Db.lineRepository.Add(ret);
            Db.Complete();

            return Ok("uspesno");
        }

        // GET: api/LineEdit/GetAllStations
        [Authorize(Roles = "Admin")]
        [ResponseType(typeof(List<string>))]
        [Route("api/LineEdit/GetAllStations")]
        public IHttpActionResult GetAllStations()
        {
            var stations = Db.stationRepository.GetAll().ToList();

            if (!stations.Any())
                return StatusCode(HttpStatusCode.BadRequest);

            var ret = new List<string>();

            foreach (var l in stations)
                ret.Add(l.Name);

            return Ok(ret);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
