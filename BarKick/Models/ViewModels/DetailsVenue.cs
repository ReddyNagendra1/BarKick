using System.Collections.Generic;

namespace BarKick.Models.ViewModels
{
    public class DetailsVenue
    {
        public bool IsAdmin { get; set; }
        public VenueDto Venue { get; set; }
        public IEnumerable<TeamDto> Teams { get; set; }
        public IEnumerable<TeamDto> AvailableTeams { get; set; }
        //Bartenders working at the venue
        public IEnumerable<BartenderDto> Bartenders { get; set; }
        //bartenders not working at venue but may be hired
        public IEnumerable<BartenderDto> AvailableBartenders { get; set; }
    }
}