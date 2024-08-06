using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using BarKick.Migrations;
using BarKick.Models;


namespace BarKick.Controllers
{
    public class VenueDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Lists all venues in the database.
        /// </summary>
        /// <returns>Returns an array of all venues.</returns>
        /// <example>
        /// GET: api/VenueData/ListVenues
        /// </example>
        [HttpGet]
        [Route("api/VenueData/ListVenues")]
        public IEnumerable<VenueDto> ListVenues()
        {
            List<Venue> Venues = db.Venues.ToList();
            List<VenueDto> VenueDtos = new List<VenueDto>();

            Venues.ForEach(v => VenueDtos.Add(new VenueDto()
            {
                VenueID = v.VenueID,
                VenueName = v.VenueName,
                VenueLocation = v.VenueLocation
            }));

            return VenueDtos;
        }

        /// <summary>
        /// Finds a specific venue by ID.
        /// </summary>
        /// <param name="id">The ID of the venue.</param>
        /// <returns>Returns the venue details.</returns>
        /// <example>
        /// GET: api/VenueData/FindVenue/12
        /// </example>
        [HttpGet]
        [ResponseType(typeof(VenueDto))]
        [Route("api/VenueData/FindVenue/{id}")]
        public IHttpActionResult FindVenue(int id)
        {
            Venue Venue = db.Venues.Find(id);
            if (Venue == null)
            {
                return NotFound();
            }

            VenueDto VenueDto = new VenueDto()
            {
                VenueID = Venue.VenueID,
                VenueName = Venue.VenueName,
                VenueLocation = Venue.VenueLocation
            };

            return Ok(VenueDto);
        }

        /// <summary>
        /// Updates a specific venue.
        /// </summary>
        /// <param name="id">The ID of the venue to update.</param>
        /// <param name="venue">The updated venue details.</param>
        /// <returns>Returns no content if the update is successful.</returns>
        /// <example>
        /// POST: api/VenueData/UpdateVenue/5
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        [Route("api/VenueData/UpdateVenue/{id}")]
        public IHttpActionResult UpdateVenue(int id, Venue venue)
        {
            Debug.WriteLine("I have reached the update venue method");
            if (!ModelState.IsValid)
            {
                Debug.WriteLine("Model state is invalid");
                return BadRequest(ModelState);
            }

            if (id != venue.VenueID)
            {
                Debug.WriteLine("ID mismatch");
                return BadRequest();
            }

            db.Entry(venue).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VenueExists(id))
                {
                    Debug.WriteLine("Venue not found");
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            Debug.WriteLine("None of the conditions triggered");
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Adds a new venue to the database.
        /// </summary>
        /// <param name="venue">The venue details to add.</param>
        /// <returns>Returns the created venue details.</returns>
        /// <example>
        /// POST: api/VenueData/AddVenue
        /// </example>
        
        [ResponseType(typeof(Venues))]
        [HttpPost]
        [Route("api/VenueData/AddVenue")]
        public IHttpActionResult AddVenue(Venue venue)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Venues.Add(venue);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = venue.VenueID }, venue);
        }

        /// <summary>
        /// Deletes a specific venue by ID.
        /// </summary>
        /// <param name="id">The ID of the venue to delete.</param>
        /// <returns>Returns the deleted venue details.</returns>
        /// <example>
        /// POST: api/VenueData/DeleteVenue/5
        /// </example>
        [ResponseType(typeof(Venues))]
        [HttpPost]
        [Route("api/VenueData/DeleteVenue/{id}")]
        public IHttpActionResult DeleteVenue(int id)
        {
            Venue venue = db.Venues.Find(id);
            if (venue == null)
            {
                return NotFound();
            }

            db.Venues.Remove(venue);
            db.SaveChanges();

            return Ok(venue);
        }

        /// <summary>
        /// Lists all teams for a specific venue.
        /// </summary>
        /// <param name="venueId">The ID of the venue.</param>
        /// <returns>Returns a list of TeamDto for the specified venue.</returns>
        /// <example>
        /// GET: api/VenueData/ListTeamsForVenue/5
        /// </example>
        [HttpGet]
        [Route("api/VenueData/ListTeamsForVenue/{venueId}")]
        public IEnumerable<TeamDto> ListTeamsForVenue(int venueId)
        {
            List<Team> teams = db.Venues
                                  .Where(v => v.VenueID == venueId)
                                  .SelectMany(v => v.Teams)
                                  .ToList();

            List<TeamDto> teamDtos = new List<TeamDto>();
            teams.ForEach(t => teamDtos.Add(new TeamDto()
            {
                TeamID = t.TeamID,
                TeamName = t.TeamName,
                TeamBio = t.TeamBio
            }));

            return teamDtos;
        }

        /// <summary>
        /// Adds a team to a specific venue.
        /// </summary>
        /// <param name="venueId">The ID of the venue.</param>
        /// <param name="teamId">The ID of the team to add.</param>
        /// <returns>Returns a status indicating the result of the operation.</returns>
        /// <example>
        /// POST: api/VenueData/AddTeamToVenue/5/10
        /// </example>
        [HttpPost]
        [Route("api/VenueData/AddTeamToVenue/{venueId}/{teamId}")]
        public IHttpActionResult AddTeamToVenue(int venueId, int teamId)
        {
            Venue venue = db.Venues.Find(venueId);
            Team team = db.Teams.Find(teamId);

            if (venue == null || team == null)
            {
                return NotFound();
            }

            venue.Teams.Add(team);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Removes a team from a specific venue.
        /// </summary>
        /// <param name="venueId">The ID of the venue.</param>
        /// <param name="teamId">The ID of the team to remove.</param>
        /// <returns>Returns a status indicating the result of the operation.</returns>
        /// <example>
        /// POST: api/VenueData/RemoveTeamFromVenue/5/10
        /// </example>
        [HttpPost]
        [Route("api/VenueData/RemoveTeamFromVenue/{venueId}/{teamId}")]
        public IHttpActionResult RemoveTeamFromVenue(int venueId, int teamId)
        {
            Venue venue = db.Venues.Find(venueId);
            Team team = db.Teams.Find(teamId);

            if (venue == null || team == null)
            {
                return NotFound();
            }

            venue.Teams.Remove(team);
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

        private bool VenueExists(int id)
        {
            return db.Venues.Count(e => e.VenueID == id) > 0;
        }
    }
}