using System.Collections.Generic;

namespace BarKick.Models.ViewModels
{
    public class DetailsBartender
    {
        public BartenderDto SelectedBartender { get; set; }
        public IEnumerable<CocktailDto> CocktailsMade { get; set; }
    }
}