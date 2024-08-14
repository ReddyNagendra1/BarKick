using Microsoft.AspNet.Identity;
using BarKick.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using System;

namespace BarKick.Controllers
{
    public class BartenderDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// recieves a GET request and returns an http response and a list object of all the bartenders in the system
        /// </summary>
        /// <example>
        /// curl https://localhost:44307/api/bartenderdata/listbartenders -->
        /// [{"BartenderId":3,"FirstName":"Alex","LastName":"Turner","Email":"alexturner@example.com","NumDrinks":4},{"BartenderId":4,"FirstName":"Noah","LastName":"Kahan","Email":"noahkahan@example.com","NumDrinks":3},{ "BartenderId":5,"FirstName":"Max","LastName":"Kerman","Email":"maxKerman@example.com","NumDrinks":5},{ "BartenderId":6,"FirstName":"Hannah","LastName":"Wicklund","Email":"hannahw@gmail.com","NumDrinks":2}]
        /// </example>
        // GET: api/bartenderdata/listbartenders
        [HttpGet]
        [Route("api/BartenderData/ListBartenders")]
        [ResponseType(typeof(IEnumerable<BartenderDto>))]
        public IHttpActionResult ListBartenders()
        {
            List<Bartender> Bartenders = db.Bartenders.ToList();
            List<BartenderDto> BartenderDtos = new List<BartenderDto>();

            Bartenders.ForEach(b => BartenderDtos.Add(new BartenderDto()
            {
                BartenderId = b.BartenderId,
                FirstName = b.FirstName,
                LastName = b.LastName,
                Email = b.Email
            }));

            return Ok(BartenderDtos);
        }
        /// <summary>
        /// Receives  BartenderId and returns a http response with all information about said bartender
        /// </summary>
        /// <param name="BartenderId">Unique integer to differentiate bartenders</param>
        /// <returns>
        /// Ok Http response and BartenderDto
        /// </returns>
        /// <example>
        /// curl https://localhost:44307/api/bartenderdata/findbartender/3  --->
        /// {"BartenderId":3,"FirstName":"Alex","LastName":"Turner","Email":"alexturner@example.com","NumDrinks":4}
        /// </example>
        // GET: api/BartenderData/FindBartender/id
        [HttpGet]
        [Route("api/BartenderData/FindBartender/{id}")]
        [ResponseType(typeof(BartenderDto))]
        public IHttpActionResult FindBartender(int id)
        {
            Bartender Bartender = db.Bartenders.Find(id);
            Debug.WriteLine(Bartender);

            if (Bartender == null)
            {
                return NotFound();
            }

            BartenderDto BartenderDto = new BartenderDto()
            {
                BartenderId = Bartender.BartenderId,
                FirstName = Bartender.FirstName,
                LastName = Bartender.LastName,
                Email = Bartender.Email
            };

            return Ok(BartenderDto);
        }
        /// <summary>
        /// Recieves a BartenderId and sends updated data to a database then returns and http response 
        /// </summary>
        /// <param name="id"> Id of Bartender user is searching for</param>
        /// <param name="b"> alias for the bartenderobject in the body of the Http response</param>
        /// <returns>HttpStatus code</returns>
        // POST: api/bartenderdata/updateBartender/id
        [HttpPost]
        [Route("api/BartenderData/UpdateBartender/{id}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult UpdateBartender(int id, [FromBody] Bartender b)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != b.BartenderId)
            {
                return BadRequest();
            }

            db.Entry(b).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BartenderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return StatusCode(HttpStatusCode.NoContent);
        }
        /// <summary>
        /// Recieves a bartender object and sends it to the database
        /// </summary>
        /// <param name="Bartender">The bartender being added to the database</param>
        /// <returns>
        /// A new bartender id for the new bartender object
        /// </returns>
        // POST: api/bartenderdata/addbartender
        [HttpPost]
        [Route("api/BartenderData/AddBartender")]
        [ResponseType(typeof(Bartender))]
        public IHttpActionResult AddBartender([FromBody] Bartender bartender)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Bartenders.Add(bartender);
            db.SaveChanges();

            // Generate URL for the newly created resource
            string url = Url.Link("DefaultApi", new { id = bartender.BartenderId });

            if (url == null)
            {
                return InternalServerError(new InvalidOperationException("Route name not found."));
            }

            return Created(url, bartender);
        }




        /// <summary>
        /// Recieves a bartenderId and sends a post request to delete that bartender from the database
        /// </summary>
        /// <param name="id">Id of bartender being deleted</param>
        /// <returns>an Ok Http response</returns>
        // POST: api/BartenderData/DeleteBartender/id
        [HttpPost]
        [Route("api/BartenderData/DeleteBartender/{id}")]
        [ResponseType(typeof(Bartender))]
        public IHttpActionResult DeleteBartender(int id)
        {
            Bartender Bartender = db.Bartenders.Find(id);
            if (Bartender == null)
            {
                return NotFound();
            }
            db.Bartenders.Remove(Bartender);
            db.SaveChanges();

            return Ok();
        }

        [HttpGet]
        [Route("api/BartenderData/ListVenuesForBartender/{id}")]
        [ResponseType(typeof(VenueDto))]
        public IHttpActionResult ListVenuesForBartender(int id)
        {
            // Fetch all venues that are associated with the bartender whose ID matches the provided ID
            List<Venue> Venues = db.Venues.Where(
                v => v.VenueBartenders.Any(
                    b => b.BartenderId == id
                )).ToList();

            List<VenueDto> VenueDtos = new List<VenueDto>();

            // Map the Venue entities to VenueDto objects
            Venues.ForEach(v => VenueDtos.Add(new VenueDto()
            {
                VenueID = v.VenueID,
                VenueName = v.VenueName,
                VenueLocation = v.VenueLocation
            }));

            return Ok(VenueDtos);
        }


        [HttpPost]
        [Route("api/BartenderData/AssociateVenue/{BartenderId}/{VenueID}")]
        //[Authorize(Roles = "Admin")]
        public IHttpActionResult AssociateVenue(int BartenderId, int VenueID)
        {
            // Fetch the bartender including the related VenueBartenders
            Bartender SelectedBartender = db.Bartenders.Include(b => b.Venues).FirstOrDefault(b => b.BartenderId == BartenderId);
            Venue SelectedVenue = db.Venues.Find(VenueID);

            if (SelectedBartender == null || SelectedVenue == null)
            {
                return NotFound();
            }

            // Debug information
            Debug.WriteLine("input bartender id is: " + BartenderId);
            Debug.WriteLine("selected bartender name is: " + SelectedBartender.FirstName + " " + SelectedBartender.LastName);
            Debug.WriteLine("input Venue id is: " + VenueID);
            Debug.WriteLine("selected venue name is: " + SelectedVenue.VenueName);

            // Check if the association already exists
            var existingAssociation = db.VenueBartenders
                .FirstOrDefault(vb => vb.BartenderId == BartenderId && vb.VenueID == VenueID);

            if (existingAssociation == null)
            {
                // Create a new VenueBartender entity
                VenueBartender newAssociation = new VenueBartender
                {
                    BartenderId = BartenderId,
                    VenueID = VenueID
                };

                // Add the association to the context
                db.VenueBartenders.Add(newAssociation);
                db.SaveChanges();
            }

            return Ok();
        }



        [HttpPost]
        [Route("api/BartenderData/UnAssociateVenue/{BartenderId}/{VenueID}")]
        //[Authorize(Roles = "Admin")]
        public IHttpActionResult UnAssociateVenue(int BartenderId, int VenueID)
        {
            // Fetch the existing association from the join table
            var existingAssociation = db.VenueBartenders
                .FirstOrDefault(vb => vb.BartenderId == BartenderId && vb.VenueID == VenueID);

            if (existingAssociation == null)
            {
                return NotFound();
            }

            // Debug information
            Debug.WriteLine("input bartender id is: " + BartenderId);
            Debug.WriteLine("input Venue id is: " + VenueID);

            // Remove the association
            db.VenueBartenders.Remove(existingAssociation);
            db.SaveChanges();

            return Ok();
        }





        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        //Checking to see if a given bartender exists in the database
        private bool BartenderExists(int id)
        {
            return db.Bartenders.Count(b => b.BartenderId == id) > 0;
        }
    }
}