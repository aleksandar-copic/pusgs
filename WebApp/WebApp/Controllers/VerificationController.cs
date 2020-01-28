using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using WebApp.Models;
using WebApp.Persistence;
using WebApp.Persistence.UnitOfWork;

namespace WebApp.Controllers
{
    public class VerificationController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public IUnitOfWork Db { get; set; }
        private const string LocalLoginProvider = "Local";
        private ApplicationUserManager _userManager;

        public VerificationController() { }

        public VerificationController(IUnitOfWork db)
        {
            this.Db = db;
        }

        [AllowAnonymous]
        [ResponseType(typeof(ApplicationUser))]
        [Route("api/Verification/ReturnUsers")]
        public IHttpActionResult GetUsers()
        {
            //var ret = new List<ApplicationUser>();

            var userStore = new UserStore<ApplicationUser>(db);
            var userManager = new UserManager<ApplicationUser>(userStore);

            var list = userManager.Users.ToList();

            var ret = (from u in list where u.VerificateAcc == 0 select u).ToList();

            return Ok(ret);
        }

        // GET: api/UserVerification/SelectedUser/{username}
        [Authorize(Roles = "Controller")]
        [ResponseType(typeof(ApplicationUser))]
        [Route("api/Verification/SelectedUser/{id}")]
        public IHttpActionResult GetSelectedUser(string id)
        {
            var userStore = new UserStore<ApplicationUser>(db);
            var userManager = new UserManager<ApplicationUser>(userStore);

            var list = userManager.Users.ToList();

            var ret = list.Find(_ => _.Id == id);

            if (ret == null)
                return StatusCode(HttpStatusCode.BadRequest);

            return Ok(ret);
        }

        [HttpGet]
        [Route("api/Verification/DownloadPicture/{id}")]
        public IHttpActionResult DownloadPicture(string id)
        {
            var userStore = new UserStore<ApplicationUser>(db);
            var userManager = new UserManager<ApplicationUser>(userStore);

            var list = userManager.Users.ToList();

            var ret = list.Find(_ => _.Id == id);

            if (ret == null)
                return BadRequest("User doesn't exists.");

            if (ret.ImageUrl == null)
                return BadRequest("Picture doesn't exists.");


            var filePath = HttpContext.Current.Server.MapPath("~/UploadFile/" + ret.ImageUrl);

            var fileInfo = new FileInfo(filePath);
            var type = fileInfo.Extension.Split('.')[1];
            var data = new byte[fileInfo.Length];

            var response = new HttpResponseMessage();
            using (FileStream fs = fileInfo.OpenRead())
            {
                fs.Read(data, 0, data.Length);
                response.StatusCode = HttpStatusCode.OK;
                response.Content = new ByteArrayContent(data);
                response.Content.Headers.ContentLength = data.Length;
            }

            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/png");

            return Ok(data);
        }

        // GET: api/UserVerification/Users
        [Authorize(Roles = "Controller")]
        [HttpGet]
        [ResponseType(typeof(ApplicationUser))]
        [Route("api/Verification/Odluka/{id}/{odluka}")]
        public IHttpActionResult Odluka(string id, string odluka)
        {
            var userStore = new UserStore<ApplicationUser>(db);
            var userManager = new UserManager<ApplicationUser>(userStore);

            var list = userManager.Users.ToList();

            var ret = list.Find(_ => _.UserName == id);

            if (ret == null)
                return StatusCode(HttpStatusCode.BadRequest);

            if (odluka.Equals("prihvati"))
                ret.VerificateAcc = 1;
            else if (odluka.Equals("odbij"))
                ret.VerificateAcc = 2;
            else
                ret.VerificateAcc = 0;

            db.Entry(ret).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException e)
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }

            return Ok(ret);
        }

        // GET: api/CardVerification/Check/{id}
        [Route("api/TicketVerification/Check/{id}")]
        [HttpGet]
        [ResponseType(typeof(string))]
        [Authorize(Roles = "Controller")]
        public IHttpActionResult GetCard(int id)
        {
            Ticket ticket = db.Ticket.FirstOrDefault(x => x.Id.Equals(id));

            if (ticket == null)
                return StatusCode(HttpStatusCode.BadRequest);

            var priceList = db.Pricelist.FirstOrDefault(x => x.Id.Equals(ticket.PricelistId));

            if (priceList == null)
                return StatusCode(HttpStatusCode.BadRequest);

            var validFrom = DateTime.Parse(priceList.From);
            var validTo = DateTime.Parse(priceList.To);
            var now = DateTime.Now;

            if (validFrom < now && now < validTo)
                return Ok("true");

            return Ok("false");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
