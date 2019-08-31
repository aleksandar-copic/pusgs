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
        [Route("api/Profil/User")]
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
        [Route("api/Profil/UplaodPicture/{username}")]
        [AllowAnonymous]
        public IHttpActionResult UploadImage(string username)
        {
            var httpRequest = HttpContext.Current.Request;

            try
            {
                if (httpRequest.Files.Count > 0)
                {
                    foreach (string file in httpRequest.Files)
                    {

                        ApplicationUser ret = new ApplicationUser();

                        var userStore = new UserStore<ApplicationUser>(db);
                        var userManager = new UserManager<ApplicationUser>(userStore);

                        List<ApplicationUser> list = userManager.Users.ToList();

                        foreach (ApplicationUser a in list)
                        {
                            if (a.UserName.Equals(username))
                            {
                                ret = a;
                                break;
                            }
                        }

                        if (ret == null)
                        {
                            return BadRequest("User does not exists.");
                        }

                        if (ret.ImageUrl != null)
                        {
                            File.Delete(HttpContext.Current.Server.MapPath("~/UploadFile/" + ret.ImageUrl));
                        }

                        var postedFile = httpRequest.Files[file];
                        string fileName = username + "_" + postedFile.FileName;
                        var filePath = HttpContext.Current.Server.MapPath("~/UploadFile/" + fileName);

                        ret.ImageUrl = fileName;

                        db.Entry(ret).State = EntityState.Modified;

                        try
                        {
                            db.SaveChanges();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            return StatusCode(HttpStatusCode.BadRequest);
                        }

                        postedFile.SaveAs(filePath);
                    }

                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }

        }

        [HttpPost]
        [AllowAnonymous]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("api/Profil/UpdateUser")]
        public IHttpActionResult EditUserInfo(RegisterBindingModel model)
        {

            var userStore = new UserStore<ApplicationUser>(db);
            var userManager = new UserManager<ApplicationUser>(userStore);
            ApplicationUser user = new ApplicationUser();
            string idUsera = User.Identity.GetUserId();
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
            //user.ImageUrl = model.ImageUrl;

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
