using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebApp.Models;

namespace WebApp.Persistence.Repository
{
    public class TicketPriceRepository : Repository<TicketPrice, int>, ITicketPriceRepository
    {
        public TicketPriceRepository(DbContext context) : base(context)
        {

        }
    }
}