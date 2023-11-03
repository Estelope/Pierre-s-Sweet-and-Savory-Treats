using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bakery.Models 
{
    public class Flavor
    {
        public int FlavorId { get; set; }
        
        [Required(ErrorMessage = "Flavor must have a name: please add a name.")]
        public string Name { get; set; }
        public ApplicationUser User { get; set; }  
        public List<FlavorTreat> FlavorTreats { get; set; }
    }
}