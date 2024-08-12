using BarKick.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BarKick.Models
{
    public class Venue
    { 
    [Key]
    public int VenueID { get; set; }
    public string VenueName { get; set; }
    public string VenueLocation { get; set; }

    //Many teams plays in venues
    public virtual ICollection<Team> Teams { get; set; }
    public virtual ICollection<VenueBartender> VenueBartenders { get; set; }
    }
    public class VenueDto
    {
    public VenueDto()
    {
        Teams = new List<TeamDto>();
    }
    public int VenueID { get; set; }
    public string VenueName { get; set; }
    public string VenueLocation { get; set; }

    public ICollection<TeamDto> Teams { get; set; }
    public List<VenueDto> Venues { get; set; }
    }
}