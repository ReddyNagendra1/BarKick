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
using BarKick.Models;


namespace BarKick.Controllers
{
    public class TeamDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Lists all teams in the database.
        /// </summary>
        /// <returns>Returns an array of all teams.</returns>
        /// <example>
        /// GET: api/TeamData/ListTeams 
        /// --> [{"TeamID":1,"TeamName":"Lakers","TeamBio":"Raising stars"},
        /// {"TeamID":2,"TeamName":"Toronto","TeamBio":"Always wins"}]
        /// </example>
        [HttpGet]
        [Route("api/TeamData/ListTeams")]
        public IEnumerable<TeamDto> ListTeams()
        {
            List<Team> Teams = db.Teams.ToList();
            List<TeamDto> TeamDtos = new List<TeamDto>();

            Teams.ForEach(t => TeamDtos.Add(new TeamDto()
            {
                TeamID = t.TeamID,
                TeamName = t.TeamName,
                TeamBio = t.TeamBio
            }));

            return TeamDtos;
        }
        /// <summary>
        /// Finds a specific team by ID.
        /// </summary>
        /// <param name="id">The ID of the team.</param>
        /// <returns>Returns the team details.</returns>
        /// <example>
        /// GET: api/TeamData/FindTeam/12
        /// </example>
        [HttpGet]
        [Route("api/TeamData/FindTeam/{id}")]
        [ResponseType(typeof(TeamDto))]
        public IHttpActionResult FindTeam(int id)
        {
            Team Team = db.Teams.Find(id);

            if (Team == null)
            {
                return NotFound();
            }

            TeamDto TeamDto = new TeamDto()
            {
                TeamID = Team.TeamID,
                TeamName = Team.TeamName,
                TeamBio = Team.TeamBio
            };

            return Ok(TeamDto);
        }

        /// <summary>
        /// Updates a specific team.
        /// </summary>
        /// <param name="id">The ID of the team to update.</param>
        /// <param name="team">The updated team details.</param>
        /// <returns>Returns no content if the update is successful.</returns>
        /// <example>
        /// POST: api/TeamData/UpdateTeam/5
        /// </example>
        [HttpPost]
        [Route("api/TeamData/AddTeam")]
        [ResponseType(typeof(void))]
       
        public IHttpActionResult UpdateTeam(int id, Team team)
        {
            Debug.WriteLine("I have reached the update team method");
            if (!ModelState.IsValid)
            {
                Debug.WriteLine("Model state is invalid");
                return BadRequest(ModelState);
            }

            if (id != team.TeamID)
            {
                Debug.WriteLine("ID mismatch");
                return BadRequest();
            }

            db.Entry(team).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeamExists(id))
                {
                    Debug.WriteLine("Team not found");
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
        /// Adds a new team to the database.
        /// </summary>
        /// <param name="team">The team details to add.</param>
        /// <returns>Returns the created team details.</returns>
        /// <example>
        /// POST: api/TeamData/AddTeam
        /// </example>
        [HttpPost]
        [Route("api/TeamData/UpdateTeam/{id}")]
        [ResponseType(typeof(Team))]
       
        public IHttpActionResult AddTeam(Team team)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Teams.Add(team);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = team.TeamID }, team);
        }
        /// <summary>
        /// Deletes a specific team by ID.
        /// </summary>
        /// <param name="id">The ID of the team to delete.</param>
        /// <returns>Returns the deleted team details.</returns>
        /// <example>
        /// POST: api/TeamData/DeleteTeam/5
        /// </example>
        [HttpPost]
        [Route("api/TeamData/DeleteTeam/{id}")]
        [ResponseType(typeof(Team))]
        
        public IHttpActionResult DeleteTeam(int id)
        {
            Team team = db.Teams.Find(id);
            if (team == null)
            {
                return NotFound();
            }

            db.Teams.Remove(team);
            db.SaveChanges();

            return Ok(team);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TeamExists(int id)
        {
            return db.Teams.Count(e => e.TeamID == id) > 0;
        }
    }

}