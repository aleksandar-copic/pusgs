namespace WebApp.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Validation;
    using System.Linq;
    using WebApp.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<WebApp.Persistence.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(WebApp.Persistence.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            // System.Diagnostics.Debugger.Launch();

            try
            {
                //  UserType
                if (!context.UserType.Any(u => u.Name == "Đak"))
                {
                    UserType userType = new UserType() { Name = "Đak", Id = 1 };
                    context.UserType.Add(userType);
                    context.SaveChanges();
                }

                if (!context.UserType.Any(u => u.Name == "Penzioner"))
                {
                    UserType userType = new UserType() { Name = "Penzioner", Id = 2 };
                    context.UserType.Add(userType);
                    context.SaveChanges();
                }

                if (!context.UserType.Any(u => u.Name == "Regularan"))
                {
                    UserType userType = new UserType() { Name = "Regularan", Id = 3 };
                    context.UserType.Add(userType);
                    context.SaveChanges();
                }

                if (!context.Roles.Any(r => r.Name == "Admin"))
                {
                    var store = new RoleStore<IdentityRole>(context);
                    var manager = new RoleManager<IdentityRole>(store);
                    var role = new IdentityRole { Name = "Admin" };

                    manager.Create(role);
                }

                if (!context.Roles.Any(r => r.Name == "Controller"))
                {
                    var store = new RoleStore<IdentityRole>(context);
                    var manager = new RoleManager<IdentityRole>(store);
                    var role = new IdentityRole { Name = "Controller" };

                    manager.Create(role);
                }

                if (!context.Roles.Any(r => r.Name == "AppUser"))
                {
                    var store = new RoleStore<IdentityRole>(context);
                    var manager = new RoleManager<IdentityRole>(store);
                    var role = new IdentityRole { Name = "AppUser" };

                    manager.Create(role);
                }

                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);

                if (!context.Users.Any(u => u.UserName == "admin@yahoo.com"))
                {
                    var user = new ApplicationUser() { Id = "admin", UserName = "admin@yahoo.com", Email = "admin@yahoo.com", PasswordHash = ApplicationUser.HashPassword("Admin123!"), TypeId = 2 };
                    userManager.Create(user);
                    userManager.AddToRole(user.Id, "Admin");
                }

                if (!context.Users.Any(u => u.UserName == "controller@yahoo.com"))
                {
                    var user = new ApplicationUser() { Id = "controller", UserName = "controller@yahoo.com", Email = "controller@yahoo.comm", PasswordHash = ApplicationUser.HashPassword("Controller123!"), TypeId = 2 };
                    userManager.Create(user);
                    userManager.AddToRole(user.Id, "Controller");
                }

                if (!context.Users.Any(u => u.UserName == "appu@yahoo"))
                {
                    var user = new ApplicationUser() { Id = "appu", UserName = "appu@yahoo", Email = "appu@yahoo.com", PasswordHash = ApplicationUser.HashPassword("Appu123!"), TypeId = 2};

                    userManager.Create(user);

                    userManager.AddToRole(user.Id, "AppUser");
                }

                // Ticket type
                if (!context.TicketType.Any(t => t.Name == "Vremenska karta"))
                {
                    TicketType ticketType = new TicketType() { Name = "Vremenska karta", Id = 1 };
                    context.TicketType.Add(ticketType);
                    try
                    {
                        context.SaveChanges();
                    }
                    catch (DbEntityValidationException e)
                    {
                        //foreach (var eve in e.EntityValidationErrors)
                        //{
                        //    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        //        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        //    foreach (var ve in eve.ValidationErrors)
                        //    {
                        //        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                        //            ve.PropertyName, ve.ErrorMessage);
                        //    }
                        //}
                        //throw;

                        var outputLines = new List<string>();
                        foreach (var eve in e.EntityValidationErrors)
                        {
                            outputLines.Add(string.Format(
                                "{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:",
                                DateTime.Now, eve.Entry.Entity.GetType().Name, eve.Entry.State));
                            foreach (var ve in eve.ValidationErrors)
                            {
                                outputLines.Add(string.Format(
                                    "- Property: \"{0}\", Error: \"{1}\"",
                                    ve.PropertyName, ve.ErrorMessage));
                            }
                        }
                        // asasdasd
                        System.IO.File.AppendAllLines(@"c:\errors.txt", outputLines);
                        throw;
                    }
                }


                if (!context.TicketType.Any(t => t.Name == "Dnevna karta"))
                {
                    TicketType ticketType = new TicketType() { Name = "Dnevna karta", Id = 2 };
                    context.TicketType.Add(ticketType);
                    context.SaveChanges();
                }

                if (!context.TicketType.Any(t => t.Name == "Mesečna karta"))
                {
                    TicketType ticketType = new TicketType() { Name = "Mesečna karta", Id = 3 };
                    context.TicketType.Add(ticketType);
                    context.SaveChanges();
                }

                if (!context.TicketType.Any(t => t.Name == "Godišnja karta"))
                {
                    TicketType ticketType = new TicketType() { Name = "Godišnja karta", Id = 4 };
                    context.TicketType.Add(ticketType);
                    context.SaveChanges();
                }

                // Pricelist
                if (!context.Pricelist.Any(t => t.Id == 1))
                {
                    PriceList pricelist = new PriceList() { Id = 1, From = DateTime.Now.ToString(), To = DateTime.Now.ToString() };
                    context.Pricelist.Add(pricelist);
                    context.SaveChanges();
                }

                if (!context.Pricelist.Any(t => t.Id == 2))
                {
                    PriceList pricelist = new PriceList() { Id = 2, From = DateTime.Now.ToString(), To = DateTime.Now.ToString() };
                }
                if (!context.Pricelist.Any(t => t.Id == 3))
                {
                    PriceList pricelist = new PriceList() { Id = 3, From = DateTime.Now.ToString(), To = DateTime.Now.ToString() };
                    context.Pricelist.Add(pricelist);
                    context.SaveChanges();
                }
                if (!context.Pricelist.Any(t => t.Id == 4))
                {
                    PriceList pricelist = new PriceList() { Id = 4, From = DateTime.Now.ToString(), To = DateTime.Now.ToString() };
                    context.Pricelist.Add(pricelist);
                    context.SaveChanges();
                }

                //  ticketPrice
                if (!context.TicketPrice.Any(t => t.Id == 1))
                {
                    TicketPrice ticketPrice = new TicketPrice() { Price = 50, PricelistId = 1, TicketTypeId = 1 };
                    context.TicketPrice.Add(ticketPrice);
                    context.SaveChanges();
                }
                if (!context.TicketPrice.Any(t => t.Id == 2))
                {
                    TicketPrice ticketPrice = new TicketPrice() { Price = 250, PricelistId = 2, TicketTypeId = 2 };
                    context.TicketPrice.Add(ticketPrice);
                    context.SaveChanges();
                }
                if (!context.TicketPrice.Any(t => t.Id == 3))
                {
                    TicketPrice ticketPrice = new TicketPrice() { Price = 1500, PricelistId = 3, TicketTypeId = 3 };
                    context.TicketPrice.Add(ticketPrice);
                    context.SaveChanges();
                }
                if (!context.TicketPrice.Any(t => t.Id == 4))
                {
                    TicketPrice ticketPrice = new TicketPrice() { Price = 4500, PricelistId = 4, TicketTypeId = 4 };
                    context.TicketPrice.Add(ticketPrice);
                    context.SaveChanges();
                }

                // Lines
                if (!context.Line.Any(t => t.Id == 1))
                {
                    Line line = new Line() { Id = 1, SerialNumber = 1 };
                    context.Line.Add(line);
                    context.SaveChanges();
                }

                if (!context.Line.Any(t => t.Id == 2))
                {
                    Line line = new Line() { Id = 2, SerialNumber = 2 };
                    context.Line.Add(line);
                    context.SaveChanges();
                }
                if (!context.Line.Any(t => t.Id == 3))
                {
                    Line line = new Line() { Id = 3, SerialNumber = 3 };
                    context.Line.Add(line);
                    context.SaveChanges();
                }
                if (!context.Line.Any(t => t.Id == 4))
                {
                    Line line = new Line() { Id = 4, SerialNumber = 4 };
                    context.Line.Add(line);
                    context.SaveChanges();
                }
                if (!context.Line.Any(t => t.Id == 5))
                {
                    Line line = new Line() { Id = 5, SerialNumber = 5 };
                    context.Line.Add(line);
                    context.SaveChanges();
                }
                // Stations
                if (!context.Station.Any(t => t.Name == "Prva"))
                {
                    Station station = new Station() { Id = 1, Name = "Prva", Address = "Adresa Prve" };
                    context.Station.Add(station);
                    context.SaveChanges();
                }

                if (!context.Station.Any(t => t.Name == "Druga"))
                {
                    Station station = new Station() { Id = 1, Name = "Prva", Address = "Adresa Druge" };
                    context.Station.Add(station);
                    context.SaveChanges();
                }
                // Timetable
                if (!context.TimeTable.Any(t => t.Id == 1))
                {
                    TimeTable timetable = new TimeTable() { Id = 1, BusLineId = 1, TimetableTypeId = 1, DayTypeId = 1, Times = "9:50 10:50 11:50" };
                    context.TimeTable.Add(timetable);
                    context.SaveChanges();
                }

                if (!context.TimeTable.Any(t => t.Id == 2))
                {
                    TimeTable timetable = new TimeTable() { Id = 2, BusLineId = 2, TimetableTypeId = 2, DayTypeId = 2, Times = "9:30 10:30 11:30 12:30 13:30" };
                    context.TimeTable.Add(timetable);
                    context.SaveChanges();
                }
                if (!context.TimeTable.Any(t => t.Id == 3))
                {
                    TimeTable timetable = new TimeTable() { Id = 3, BusLineId = 1, TimetableTypeId = 1, DayTypeId = 2, Times = "9:10 10:10 11:10 12:10 13:10" };
                    context.TimeTable.Add(timetable);
                    context.SaveChanges();
                }

            }
            catch (DbEntityValidationException e)
            {
                //foreach (var eve in e.EntityValidationErrors)
                //{
                //    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                //        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                //    foreach (var ve in eve.ValidationErrors)
                //    {
                //        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                //            ve.PropertyName, ve.ErrorMessage);
                //    }
                //}
                //throw;

                var outputLines = new List<string>();
                foreach (var eve in e.EntityValidationErrors)
                {
                    outputLines.Add(string.Format(
                        "{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:",
                        DateTime.Now, eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        outputLines.Add(string.Format(
                            "- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage));
                    }
                }
                // asasdasd
                System.IO.File.AppendAllLines(@"C:\Users\Stefan\errors.txt", outputLines);
                throw;
            }
        }
    }
}
