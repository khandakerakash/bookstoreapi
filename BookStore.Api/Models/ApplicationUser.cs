using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace BookStore.Api.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Address { get; set; }

        public ICollection<Book> Books { get; set; }

    }
}
