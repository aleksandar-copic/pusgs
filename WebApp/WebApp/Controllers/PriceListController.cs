using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web.Http;
using System.Web.Http.Description;
using WebApp.Models;
using WebApp.Persistence;
using WebApp.Persistence.UnitOfWork;

namespace WebApp.Controllers
{
    [Authorize]
    public class PriceListController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public IUnitOfWork Db { get; set; }

        private const string LocalLoginProvider = "Local";
        private ApplicationUserManager _userManager;

        public PriceListController() { }

        public PriceListController(IUnitOfWork db)
        {
            this.Db = db;
        }

        // GET: api/PriceList/UserType/{loggedUser}
        [AllowAnonymous]
        [ResponseType(typeof(string))]
        [Route("api/PriceList/UserType")]
        public IHttpActionResult GetUserType()
        {
            var userStore = new UserStore<ApplicationUser>(db);
            var userManager = new UserManager<ApplicationUser>(userStore);
            string type = "";
            try
            {
                var id = User.Identity.GetUserId();


                ApplicationUser user = userManager.FindById(id);

                var typeId = user.TypeId;

                type = Db.userTypeRepository.Find(x => x.Id.Equals(typeId)).FirstOrDefault().Name;

            }
            catch (Exception)
            {
                type = "neregistrovan" /*+ e.Message.ToString()*/;

            }

            return Ok(type);
        }

        // GET: api/PriceList/UserType/{loggedUser}
        [AllowAnonymous]
        [ResponseType(typeof(double))]
        [Route("api/PriceList/Prices/{tipKarte}/{tipKorisnika}")]
        public IHttpActionResult GetPrice(string tipKarte, string tipKorisnika)
        {
            int s = Db.ticketTypeRepository.Find(x => x.Name.Equals(tipKarte)).FirstOrDefault().Id;
            double ret = Db.ticketPriceRepository.Find(x => x.TicketTypeId.Equals(s)).FirstOrDefault().Price;

            if (tipKorisnika.Equals("Penzioner"))
            {
                ret = ret * 0.8;
            }
            else if (tipKorisnika.Equals("Đak"))
            {
                ret = ret * 0.9;
            }

            return Ok(ret);
        }

        // POST api/Cenovnik/KupiKartu
        [AllowAnonymous]
        [Route("api/PriceList/BuyTicket")]
        public IHttpActionResult KupiKartu(TicketPurchaseBindingModel ticketPurchase)
        {
            if (ticketPurchase.Price == 0)
            {
                int s = Db.ticketTypeRepository.Find(x => x.Name.Equals(ticketPurchase.TipKarte)).FirstOrDefault().Id;
                double ret = Db.ticketPriceRepository.Find(x => x.TicketTypeId.Equals(s)).FirstOrDefault().Price;
                ticketPurchase.Price = ret;
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (ticketPurchase == null)
            {
                return BadRequest();
            }

            var userStore = new UserStore<ApplicationUser>(db);
            var userManager = new UserManager<ApplicationUser>(userStore);
            Ticket ticket = new Ticket();
            ApplicationUser user = new ApplicationUser();
            ticket.FinalPrice = ticketPurchase.Price;
            string idUsera = User.Identity.GetUserId();
            DateTime vaziOd = new DateTime();
            DateTime vaziDo = new DateTime();

            if (idUsera != null)
            {
                var id = User.Identity.GetUserId();
                user = userManager.FindById(id);
                ticket.UserId = user.Id;
            }

            if (ticketPurchase.TipKarte.Equals("Vremenska karta"))
            {
                vaziOd = DateTime.Now;
                vaziDo = DateTime.Now.AddHours(1);
            }
            else if (ticketPurchase.TipKarte.Equals("Dnevna karta"))
            {
                vaziOd = DateTime.Now;
                vaziDo = KrajDana(DateTime.Now);
            }
            else if (ticketPurchase.TipKarte.Equals("Mesečna karta"))
            {
                vaziOd = DateTime.Now;
                DateTime temp = DateTime.Now;
                temp = vaziOd.AddMonths(1);
                vaziDo = new DateTime(temp.Year, temp.Month, 1, 0, 0, 0);
            }
            else if (ticketPurchase.TipKarte.Equals("Godišnja karta"))
            {
                vaziOd = DateTime.Now;
                DateTime temp = DateTime.Now;
                temp = vaziOd.AddYears(1);
                vaziDo = new DateTime(temp.Year, 1, 1, 0, 0, 0);
            }

            PriceList pl = new PriceList();
            pl.From = vaziOd.ToString();
            pl.To = vaziDo.ToString();

            try
            {
                db.Entry(pl).State = EntityState.Added;
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }

            PriceList pricelist = db.Pricelist.Where(x => x.From.Equals(pl.From) && x.To.Equals(pl.To)).FirstOrDefault();
            ticket.PricelistId = pricelist.Id;

            try
            {
                db.Entry(ticket).State = EntityState.Added;
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }

            if(user.Email != null)
                SendEmail("JGSP", "jgspns71@gmail.com", user.Email, "JGSP, kupovina karte.", $"Salo pederu");
            return Ok("uspesno");
        }

        private void SendEmail(string sendername, string sender, string recipient, string subject, string body)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com", 587)
            {
                UseDefaultCredentials = true,
                Credentials = new System.Net.NetworkCredential()
                {
                    UserName = "jgspns71",
                    Password = "jgspnovisad1!"
                },

                EnableSsl = true
            };

            var from = new MailAddress(sender, sendername);
            var to = new MailAddress(recipient, "");
            var mailMessage = new MailMessage(from, to)
            {
                Subject = subject,
                Body = body
            };

            try
            {
                smtpClient.Send(mailMessage);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        // PriceList edit api
        [Route("api/ticketPriceEdit/ticketPriceEditGetPrice/{ticketTypeId}")]
        [HttpGet]
        public IHttpActionResult GetPrice(int ticketTypeId)
        {
            var price = Db.ticketPriceRepository.Find(x => x.TicketTypeId == ticketTypeId).FirstOrDefault().Price;

            return Ok(price);
        }

        [HttpPost]
        [Route("api/ticketPriceEdit/UpdateTicketPrice/{ticketTypeId}/{price}")]
        public IHttpActionResult UpdateTimetable(int ticketTypeId, int price)
        {
            TicketPrice ticket = new TicketPrice();
            ticket = Db.ticketPriceRepository.Find(x => x.TicketTypeId.Equals(ticketTypeId)).FirstOrDefault();
            ticket.Price = price;
            db.Entry(ticket).State = EntityState.Modified;

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

        private bool KartaExists(int id)
        {
            return db.Ticket.Count(e => e.Id == id) > 0;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TimetableExist(int id)
        {
            throw new NotImplementedException();
            // return Db.timeTableRepository.GetAll().Count(e => e.Id == id) > 0;
        }

        public static DateTime PocetakDana(DateTime dateTime)
        {
            return dateTime.Date;
        }

        public static DateTime KrajDana(DateTime dateTime)
        {
            return PocetakDana(dateTime).AddDays(1).AddTicks(-1);
        }
    }
}
