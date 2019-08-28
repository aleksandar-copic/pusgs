using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class UserType
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public List<ApplicationUser> Users { get; set; }
    }
}