using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BarKick.Models
{
    public class Bartender
    {
        [Key]
        public int BartenderId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        //a bartender can work at many venues
        public virtual ICollection<VenueBartender> Venues { get; set; }
    }

    //Data transfer object (DTO) - Communicating the bartenders info externally
    [JsonObject]
    public class BartenderDto
    {
        public int BartenderId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public List<VenueDto> Venues { get; set; }

    }
}