using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uppgift_ASP.Net.Data;

namespace Uppgift_ASP.Net.Models
{
    public class EditClassesViewModel
    {
       public SchoolClass CurrentClass { get; set; }
       public IEnumerable<AppUser> Teachers { get; set; }
       public IEnumerable<AppUser> Students { get; set; }

    }
}
