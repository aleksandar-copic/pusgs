using System;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using WebApp.Models;

namespace WebApp.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<TicketType> TicketType { get; set; }
        public DbSet<Ticket> Ticket { get; set; }
        public DbSet<TicketPrice> TicketPrice { get; set; }
        public DbSet<PriceList> Pricelist { get; set; }
        public DbSet<UserType> UserType { get; set; }
        public DbSet<Line> Line { get; set; }
        public DbSet<Station> Station { get; set; }
        public DbSet<TimeTable> TimeTable { get; set; }
        public DbSet<Vehicle> Vehicle { get; set; }



        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}