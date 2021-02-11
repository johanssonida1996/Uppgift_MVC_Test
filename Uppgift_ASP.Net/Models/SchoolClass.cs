using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Uppgift_ASP.Net.Data;

namespace Uppgift_ASP.Net.Models
{
    public class SchoolClass
    {
        [Required]
        [Key]
        public string Id { get; set; }
       
        public AppUser Teacher { get; set; }

        public virtual ICollection<AppUser> Students { get; set; }        

        
       //public List<EditClassesViewModel> _schoolClassList { get; set; }
    }
}
