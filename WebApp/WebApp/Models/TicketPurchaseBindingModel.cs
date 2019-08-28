using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class TicketPurchaseBindingModel
    {
        public string Username { get; set; }
        public double Price { get; set; }

        public string TipKarte { get; set; }
    }
}