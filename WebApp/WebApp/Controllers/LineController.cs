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
            List<int> ret = new List<int>();
            List<Line> line = new List<Line>();

            line = Db.lineRepository.GetAll().ToList();

            foreach (Line l in line)
            {
                ret.Add(l.SerialNumber);
            }

            return Ok(ret);
        }

        [AllowAnonymous]
        [Route("api/LineEdit/Linijas")]
        public List<string> GetLinije()
        {
            IQueryable<Line> linije = Db.lineRepository.GetAll().AsQueryable();
            List<string> BrojeviLinija = new List<string>();
            foreach (Line l in linije)
            {
                BrojeviLinija.Add(l.SerialNumber.ToString());
            }
            return BrojeviLinija;
        }

        // GET: api/LineEdit/SelectedLine
        [AllowAnonymous]
        [ResponseType(typeof(Line))]
        [Route("api/LineEdit/SelectedLine/{serial}")]
        public IHttpActionResult GetSelectedLine(string serial)
        {
            List<Line> line = new List<Line>();
            Line ret = new Line();
            line = Db.lineRepository.GetAll().ToList();
            int serialNumber = Int32.Parse(serial);

            bool found = false;
            foreach (var l in line)
            {
                if (!l.SerialNumber.Equals(serialNumber)) continue;
                ret = l;
                found = true;
                break;
            }

            // resava problem prikaza Serial Number-a za liniju.
            // FIXME: BUDZ
            ret = new Line() {SerialNumber = serialNumber};

            return Ok(ret);

        }

        // GET: api/LineEdit/SelectedLine
        [Authorize(Roles = "Admin")]
        [ResponseType(typeof(List<string>))]
        [Route("api/LineEdit/GetStations/{serial}")]
        public IHttpActionResult GetStations(string serial)
        {
            List<Line> line = new List<Line>();
            Line ret = new Line();
            line = Db.lineRepository.GetAll().ToList();
            int serialNumber = Int32.Parse(serial);

            foreach (var l in line)
            {
                if (l.SerialNumber.Equals(serialNumber))
                {
                    ret = l;
                    break;
                }
            }

            List<Station> list = Db.stationRepository.Find(x => x.Lines.All(y => y.Id.Equals(ret.Id))).ToList();

            List<string> returnsList = new List<string>();

            foreach (Station s in list)
            {
                returnsList.Add(s.Name);
            }

            if (returnsList != null)
                return Ok(returnsList);
            else
                return StatusCode(HttpStatusCode.BadRequest);

        }

        // GET: api/LineEdit/SelectedLine
        [Authorize(Roles = "Admin")]
        [ResponseType(typeof(Station))]
        [Route("api/LineEdit/GetSelectedStation/{name}")]
        public IHttpActionResult GetSelectedStation(string name)
        {
            List<Station> stations = new List<Station>();
            Station ret = new Station();
            stations = Db.stationRepository.GetAll().ToList();

            foreach (Station l in stations)
            {
                if (l.Name.Equals(name))
                {
                    ret = l;
                    break;
                }
            }

            if (ret != null)
                return Ok(ret);
            else
                return StatusCode(HttpStatusCode.BadRequest);

        }

        // DELETE: api/LineEdit/DeleteSelectedLine/{serial}
        [Authorize(Roles = "Admin")]
        [ResponseType(typeof(string))]
        [Route("api/LineEdit/DeleteSelectedLine/{serial}")]
        public IHttpActionResult DeleteSelectedLine(string serial)
        {
            List<Line> line = new List<Line>();
            Line ret = new Line();
            line = Db.lineRepository.GetAll().ToList();
            int serialNumber = Int32.Parse(serial);

            foreach (var l in line)
            {
                if (l.SerialNumber.Equals(serialNumber))
                {
                    ret = l;
                    break;
                }
            }

            if (ret != null)
            {
                db.Entry(ret).State = EntityState.Deleted;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException e)
                {
                    return StatusCode(HttpStatusCode.BadRequest);
                }

                return Ok("uspesno");
            }
            else
                return StatusCode(HttpStatusCode.BadRequest);

        }

        // POST: api/LineEdit/AddLine
        [ResponseType(typeof(string))]
        [Route("api/LineEdit/AddLine")]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult AddLine(Line line)
        {
            bool postoji = false;
            List<Line> provera = new List<Line>();
            provera = Db.lineRepository.GetAll().ToList();

            foreach (var l in provera)
            {
                if (l.SerialNumber.Equals(line.SerialNumber))
                {
                    postoji = true;
                    break;
                }
            }

            if (!postoji)
            {
                Line ret = new Line();
                ret.SerialNumber = line.SerialNumber;

                List<Station> temp = new List<Station>();
                temp = Db.stationRepository.GetAll().ToList();
                List<Station> stations = new List<Station>();

                foreach (Station s in temp)
                {
                    //if (line.Stations.Contains(s))
                    foreach(var ss in line.Stations)
                    {
                        if (ss.Name == s.Name)
                        {
                            stations.Add(s);
                            ret.Stations = stations;

                            Db.lineRepository.Add(ret);
                            Db.Complete();
                            return Ok("uspesno");
                        }
                    }
                }


                return Ok("uspesno");
            }
            else
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }
        }


        // POST: api/LineEdit/Spoji/{linija}/{stanica}
        [ResponseType(typeof(string))]
        [Route("api/LineEdit/Spoji/{linija}/{stanica}")]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult Spoji(string linija, string stanica)
        {
            Line lin = Db.lineRepository.GetAll().Where(x => x.SerialNumber.ToString() == linija).FirstOrDefault();
            Station sta = Db.stationRepository.GetAll().Where(x => x.Name == stanica).FirstOrDefault();
            lin.Stations = new List<Station>();
            lin.Stations.Add(sta);
            sta.Lines = new List<Line>();
            sta.Lines.Add(lin);
            Db.stationRepository.Update(sta);
            Db.lineRepository.Update(lin);
            Db.Complete();
            return Ok("");

        }

        // GET: api/LineEdit/GetAllStations
        [Authorize(Roles = "Admin")]
        [ResponseType(typeof(List<string>))]
        [Route("api/LineEdit/GetAllStations")]
        public IHttpActionResult GetAllStations()
        {
            List<Station> stations = new List<Station>();
            List<string> ret = new List<string>();

            stations = Db.stationRepository.GetAll().ToList();

            foreach (Station l in stations)
            {
                ret.Add(l.Name);
            }

            if (ret != null)
                return Ok(ret);
            else
                return StatusCode(HttpStatusCode.BadRequest);

        }

        //[AllowAnonymous]
        //[ResponseType(typeof(string))]
        //[Route("api/LineEdit/GetStanica/{linijaBroj}")]
        //public IHttpActionResult GetStanica(string linijaBroj)
        //{
        //    List<Line> sveLinije = Db.busLineRepository.GetAll().ToList();
        //    Line izabranaLinija = new Line();

        //    foreach (var l in sveLinije)
        //    {
        //        if (l.SerialNumber.ToString() == linijaBroj)
        //        {
        //            izabranaLinija = l;
        //            break;
        //        }
        //    }

        //    if (izabranaLinija == null)
        //    {
        //        return NotFound();
        //    }

        //    List<Koordinate> listaKoordinata = new List<Koordinate>();
        //    foreach (var stanica in izabranaLinija.Stations)
        //    {
        //        Koordinate k = new Koordinate() { x = stanica.X, y = stanica.Y, name = stanica.Name };
        //        listaKoordinata.Add(k);
        //    }

        //    return Ok(listaKoordinata);
        //}


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
