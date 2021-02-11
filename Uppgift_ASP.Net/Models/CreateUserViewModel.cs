using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uppgift_ASP.Net.Data;

namespace Uppgift_ASP.Net.Models
{
    public class CreateUserViewModel: AppUser
    {
        public string Password { get; set; }
    }
}
