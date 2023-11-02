using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bakery.Models 
{
    public class Treat
    {
        public int TreatId { get; set; }
        
        [Required(ErrorMessage = "Treat must have a name: please add a name.")]
        public string Name { get; set; }
        public ApplicationUser User { get; set; }
        public List<FlavorTreat> FlavorTreats { get; set; }
    }
}