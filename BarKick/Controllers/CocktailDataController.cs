using BarKick.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace BarKick.Controllers
{
    public class CocktailDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        /// <summary>
        /// Returns a list of cocktails in the system
        /// </summary>
        /// <returns>Header 200 (OK)
        /// Content: all cocktails in the database</returns>
        /// <example>
        /// GET: /api/CocktailData/ListCocktails ->
        /// [{"drinkId" : 3, "DrinkName" : "Vodka Cran", "DrinkType" : "Bar Rail" , "DrinkRecipe" : "Pour 1 oz of vodka over ice, top off with cranberry juice" , "LiqIn" : "Vodka", "MixIn" : "Cranberry Juice", "bartenderId" : 4, "firstName" : "Noah", "lastName" : "Kahan"},
        /// {"drinkId" : 4, "DrinkName" : "Gin and Tonic", "DrinkType" : "Bar Rail" , "DrinkRecipe" : "Pour 1 oz of gin over ice, top off with tonic water" , "LiqIn" : "Gin", "MixIn" : "Tonic Water", "bartenderId" : 7, "firstName" : "Max", "lastName" : "Kerman"}]
        /// </example>
        [HttpGet]
        [ResponseType(typeof(CocktailDto))]
        public IHttpActionResult CocktailList()
        {
            //fetch cocktails from database and store them in a list
            List<Cocktail> cocktails = db.Cocktails.ToList();
            //create a list of cocktails as data tranferable objects
            List<CocktailDto> cocktailDtos = new List<CocktailDto>();

            //convert each cocktail entity into a cocktaildto and add to the list
            cocktails.ForEach(c => cocktailDtos.Add(new CocktailDto()
            {
                DrinkId = c.DrinkId,
                DrinkName = c.DrinkName,
                DrinkRecipe = c.DrinkRecipe,
                LiqIn = c.LiqIn,
                MixIn = c.MixIn,
                BartenderId = c.BartenderId,
                FirstName = c.Bartender.FirstName,
                LastName = c.Bartender.LastName
            }));
            Debug.WriteLine(cocktailDtos);
            return Ok(cocktailDtos);
        }

        /// <summary>
        /// Gathers information about all Cocktails related to a specific bartenderId
        /// </summary>
        /// <returns>
        /// Https response containing all the cocktails the provided bartender has made
        /// </returns>
        /// <param name="id">bartenderId</param>
        /// <example>
        /// GET: api/CocktailData/ListCocktailsByBartender/2
        /// HEADER: 200 (OK)
        /// CONTENT: [{"drinkId" : 3, "DrinkName" : "Vodka Cran", "DrinkType" : "Bar Rail" , "DrinkRecipe" : "Pour 1 oz of vodka over ice, top off with cranberry juice" , "LiqIn" : "Vodka", "MixIn" : "Cranberry Juice", "bartenderId" : 2, "firstName" : "Noah", "lastName" : "Kahan"},
        /// {"drinkId" : 4, "DrinkName" : "Gin and Tonic", "DrinkType" : "Bar Rail" , "DrinkRecipe" : "Pour 1 oz of gin over ice, top off with tonic water" , "LiqIn" : "Gin", "MixIn" : "Tonic Water", "bartenderId" : 2, "firstName" : "Noah", "lastName" : "Kahan"}]
        /// </example>

        [HttpGet]
        [ResponseType(typeof(CocktailDto))]
        [Route("api/cocktaildata/listcocktailsbybartender/{id}")]
        public IHttpActionResult ListCocktailsByBartender(int id)
        {
            List<Cocktail> Cocktails = db.Cocktails.Where(c => c.BartenderId == id).ToList();
            List<CocktailDto> CocktailDtos = new List<CocktailDto>();

            Cocktails.ForEach(c => CocktailDtos.Add(new CocktailDto()
            {
                DrinkId = c.DrinkId,
                DrinkName = c.DrinkName,
                DrinkRecipe = c.DrinkRecipe,
                LiqIn = c.LiqIn,
                MixIn = c.MixIn,
                BartenderId = c.BartenderId,
                FirstName = c.Bartender.FirstName,
                LastName = c.Bartender.LastName
            }));

            return Ok(CocktailDtos);
        }

        /// <summary>
        /// Finds a cocktail in the database according to the given id
        /// </summary>
        /// <returns>
        /// Http response with the details of a specific cocktail
        /// </returns>
        /// <param name="id">bartenderId</param>
        /// <example>
        /// GET: api/CocktailData/findcocktail/3
        /// HEADER: 200 (OK)
        /// CONTENT: {"drinkId" : 3, "DrinkName" : "Vodka Cran", "DrinkType" : "Bar Rail" , "DrinkRecipe" : "Pour 1 oz of vodka over ice, top off with cranberry juice" , "LiqIn" : "Vodka", "MixIn" : "Cranberry Juice", "bartenderId" : 4, "firstName" : "Noah", "lastName" : "Kahan"}
        /// </example>
        [ResponseType(typeof(CocktailDto))]
        [HttpGet]
        [Route("api/CocktailData/FindCocktail/{id}")]
        public IHttpActionResult FindCocktail(int id)
        {
            Cocktail Cocktail = db.Cocktails.Find(id);

            if (Cocktail == null)
            {
                return NotFound();
            }
            CocktailDto CocktailDto = new CocktailDto()
            {
                DrinkId = Cocktail.DrinkId,
                DrinkName = Cocktail.DrinkName,
                DrinkRecipe = Cocktail.DrinkRecipe,
                LiqIn = Cocktail.LiqIn,
                MixIn = Cocktail.MixIn,
                BartenderId = Cocktail.BartenderId,
                FirstName = Cocktail.Bartender.FirstName,
                LastName = Cocktail.Bartender.LastName
            };

            return Ok(CocktailDto);

        }

        [ResponseType(typeof(void))]
        [HttpPost]
        [Route("api/cocktaildata/UpdateCocktail/{id}")]
        public IHttpActionResult UpdateCocktail(int id, [FromBody] Cocktail cocktail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != cocktail.DrinkId)
            {
                return BadRequest("ID mismatch");
            }

            // Example of updating the database entity
            var existingCocktail = db.Cocktails.FirstOrDefault(c => c.DrinkId == id);
            if (existingCocktail == null)
            {
                return NotFound();
            }

            existingCocktail.DrinkName = cocktail.DrinkName;
            existingCocktail.DrinkRecipe = cocktail.DrinkRecipe;
            existingCocktail.LiqIn = cocktail.LiqIn;
            existingCocktail.MixIn = cocktail.MixIn;
            existingCocktail.BartenderId = cocktail.BartenderId;

            try
            {
                db.SaveChanges();
                return Ok(); // Or return any appropriate success response
            }
            catch (Exception ex)
            {
                // Log the exception
                Debug.WriteLine($"Error updating cocktail: {ex.Message}");
                return InternalServerError(ex);
            }
        }



        // POST: api/CocktailData/AddCocktail
        [ResponseType(typeof(Cocktail))]
        [HttpPost]
        [Route("api/cocktaildata/AddCocktail")]
        public IHttpActionResult AddCocktail([FromBody] Cocktail cocktail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                db.Cocktails.Add(cocktail);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                // Log the exception details
                Debug.WriteLine("Exception occurred: " + ex.Message);
                // You can also log this to a file, database, etc.
                return InternalServerError(ex);
            }

            return CreatedAtRoute("DefaultApi", new { id = cocktail.DrinkId }, cocktail);
        }



        // POST: api/CocktailData/DeleteCocktail/id

        [ResponseType(typeof(Cocktail))]
        [HttpPost]
        [Route("api/cocktaildata/DeleteCocktail/{id}")]
        public IHttpActionResult DeleteCocktail(int id)
        {
            Cocktail cocktail = db.Cocktails.Find(id);

            if (cocktail == null)
            {
                return NotFound();
            }

            db.Cocktails.Remove(cocktail);

            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                // Log the exception details
                Debug.WriteLine("Exception occurred: " + ex.Message);
                // Return an appropriate error response
                return InternalServerError(ex);
            }

            return Ok(cocktail); // Return the deleted cocktail
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CocktailExists(int id)
        {
            return db.Cocktails.Count(c => c.DrinkId == id) > 0;
        }

    }
}