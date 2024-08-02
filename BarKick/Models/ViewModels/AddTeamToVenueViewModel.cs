using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BarKick.Models;
using BarKick.Models.ViewModels;

namespace BarKick.Models.ViewModels
{
    public class AddTeamToVenueViewModel
    {
        public VenueDto Venue { get; set; }
        public IEnumerable<TeamDto> Teams { get; set; }
    }
}