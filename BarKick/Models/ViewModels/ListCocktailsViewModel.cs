using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BarKick.Models.ViewModels;


namespace BarKick.Models.ViewModels
{
    public class ListCocktailsViewModel
    {
        public List<CocktailDto> Cocktails { get; set; }
    }
}