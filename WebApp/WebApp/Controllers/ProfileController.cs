using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
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
    [Authorize]
    public class ProfileController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public IUnitOfWork Db { get; set; }

        private const string LocalLoginProvider = "Local";
        private ApplicationUserManager _userManager;

        public ProfileController() { }

        public ProfileController(IUnitOfWork db)
        {
            this.Db = db;
        }

        // GET: api/Profil/User
        [AllowAnonymous]
        [ResponseType(typeof(List<ApplicationUser>))]
        [Route("api/Profile/User")]
        public IHttpActionResult GetUser()
        {
            ApplicationUser ret = new ApplicationUser();

            var userStore = new UserStore<ApplicationUser>(db);
            var userManager = new UserManager<ApplicationUser>(userStore);

            string idUsera = User.Identity.GetUserId();
            if (idUsera != null)
            {
                var id = User.Identity.GetUserId();
                ret = userManager.FindById(id);
            }
            else
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }

            return Ok(ret);
        }

        [HttpPost]
        [Route("api/Profile/UplaodPicture/{username}")]
        [AllowAnonymous]
        public IHttpActionResult UploadImage(string username)
        {
            var httpRequest = HttpContext.Current.Request;

            try
            {
                if (httpRequest.Files.Count == 0)
                    return BadRequest();

                foreach (string file in httpRequest.Files)
                {
                    var userStore = new UserStore<ApplicationUser>(db);
                    var userManager = new UserManager<ApplicationUser>(userStore);

                    var list = userManager.Users.ToList();

                    ApplicationUser ret = null;
                    foreach (var user in list)
                    {
                        if (!user.UserName.Equals(username)) continue;

                        ret = user;
                        break;
                    }

                    if (ret == null)
                        return BadRequest("User does not exists.");

                    var postedFile = httpRequest.Files[file];
                    var fileName = username + "_" + postedFile.FileName;

                    var path = HttpContext.Current.Server.MapPath("~/UploadFile/");
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    if (ret.ImageUrl != null)
                        File.Delete(path + fileName);

                    ret.ImageUrl = fileName;

                    db.Entry(ret).State = EntityState.Modified;

                    db.SaveChanges();

                    var filePath = HttpContext.Current.Server.MapPath("~/UploadFile/" + fileName);

                    postedFile.SaveAs(filePath);
                }

                return Ok();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }

        }

        [HttpPost]
        [AllowAnonymous]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("api/Profile/UpdateUser")]
        public IHttpActionResult EditUserInfo(RegisterBindingModel model)
        {

            var userStore = new UserStore<ApplicationUser>(db);
            var userManager = new UserManager<ApplicationUser>(userStore);
            var user = new ApplicationUser();

            var idUsera = User.Identity.GetUserId();
            if (idUsera != null)
            {
                var id = User.Identity.GetUserId();
                user = userManager.FindById(id);
            }

            user.Name = model.Name;
            user.Surname = model.Surname;
            user.Address = model.Address;
            user.Date = model.Date;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.ImageUrl = model.ImageUrl;

            db.Entry(user).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }

            return Ok("uspesno");
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
