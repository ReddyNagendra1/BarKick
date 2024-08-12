using System.Collections.Generic;

namespace BarKick.Models.ViewModels
{
    public class DetailsBartender
    {
        public bool IsAdmin { get; set; }
        public BartenderDto SelectedBartender { get; set; }
        public IEnumerable<CocktailDto> CocktailsMade { get; set; }
        //Venues the bartender is working at
        public IEnumerable<VenueDto> VenueBartenders { get; set; }
        //venues the bartender is not working at but may be hired for
        public IEnumerable<VenueDto> AvailableVenues { get; set; }
    }
}