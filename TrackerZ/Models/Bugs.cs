using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrackerZ.Models
{
    public class Bugs
    {
        public Guid Id { get; set; }
        [StringLength(30, MinimumLength = 3)]
        [Required]
        public string Title { get; set; }
        public string Text { get; set; }
        public string Status { get; set; }
        public DateTime Added { get; set; }
        public DateTime? Closed { get; set; }
        public string CatId { get; set; }
        public string CatIdNr { get; set; }
        public string CatName { get; set; }
    }
}
