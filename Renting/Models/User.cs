using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Renting.Models
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }

        public ICollection<Rental> Rentals { get; set; }

    }
}
