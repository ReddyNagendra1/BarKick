using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BarKick.Models
{
    public class Team
    {
        [Key]
        public int TeamID { get; set; }

        public string TeamName { get; set; }

        public string TeamBio { get; set; }
        //A team has many players
        public virtual ICollection<Player> Players { get; set; }
        //A team plays in many venues
        public virtual ICollection<Venue> Venues { get; set; }
    }
    public class TeamDto
    {
        public int TeamID { get; set; }
        public string TeamName { get; set; }
        public string TeamBio { get; set; }
    }
}