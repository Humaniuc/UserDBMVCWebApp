using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppLogic
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [EmailAddress]
        public string Email { get; set; }
        public string Description { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
    }
}
