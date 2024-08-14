using System.Collections.Generic;

namespace BarKick.Models.ViewModels
{
    public class DetailsBartender
    {
        public BartenderDto SelectedBartender { get; set; }
        public IEnumerable<CocktailDto> CocktailsMade { get; set; }
        public IEnumerable<VenueDto> VenueBartenders { get; set; }//Venues the bartender is working at
        public IEnumerable<VenueDto> AvailableVenues { get; set; }//venues the bartender is not working at but may be hired for
    }
}