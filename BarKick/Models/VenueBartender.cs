using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BarKick.Models
{
    public class VenueBartender
    {
        public int VenueID { get; set; }
        public Venue Venue { get; set; }

        public int BartenderId { get; set; }
        public Bartender Bartender { get; set; }
    }


}