using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Policy;
using System.Web.Http;
using System.Web.Http.Description;
using BarKick.Models;

namespace BarKick.Controllers
{
    public class PlayerDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Lists all players in the database
        /// </summary>
        /// <returns>Returns an array of all players</returns>
        /// <example>
        // GET: api/PlayerData/ListPlayers --> [{"PlayerID":1,"PlayerName":"Jenni","PlayerPosition":"Forward"},
        // {"PlayerID":2,"PlayerName":"Harry","PlayerPosition":"Forward"}]
        /// 
        /// </example>
        [HttpGet]
        [Route("api/PlayerData/ListPlayers")]
        public IEnumerable<PlayerDto> ListPlayers()
        {
            List<Player> Players = db.Players.ToList();
            List<PlayerDto> PlayerDtos = new List<PlayerDto>();

            Players.ForEach(p => PlayerDtos.Add(new PlayerDto()
            {
                PlayerID = p.PlayerID,
                PlayerName = p.PlayerName,
                PlayerPosition = p.PlayerPosition,
                TeamName = p.Team.TeamName

            }));

            return PlayerDtos;
        }

        /// <summary>
        /// Finds a specific player by ID.
        /// </summary>
        /// <param name="id">The ID of the player.</param>
        /// <returns>Returns the player details.</returns>
        /// <example>
        /// GET: api/PlayerData/FindPlayer/12
        /// </example>

        [HttpGet]
        [Route("api/PlayerData/FindPlayer/{id}")]
        [ResponseType(typeof(Player))]
        public IHttpActionResult FindPlayer(int id)
        {
            Debug.WriteLine("FindPlayer method called with ID: " + id);

            Player Player = db.Players.Find(id);

            if (Player == null)
            {
                return NotFound();
            }

            PlayerDto PlayerDto = new PlayerDto()
            {
                PlayerID = Player.PlayerID,
                PlayerName = Player.PlayerName,
                PlayerPosition = Player.PlayerPosition,
                TeamID = Player.Team.TeamID,
                TeamName = Player.Team.TeamName
            };

            Debug.WriteLine("Player found: " + PlayerDto.PlayerName);


            return Ok(PlayerDto);
        }

        /// <summary>
        /// Updates a specific player.
        /// </summary>
        /// <param name="id">The ID of the player to update.</param>
        /// <param name="player">The updated player details.</param>
        /// <returns>Returns no content if the update is successful.</returns>
        /// <example>
        /// POST: api/PlayerData/UpdatePlayer/5
        /// </example>
        // POST: api/PlayerData/UpdatePlayer/5
        [HttpPost]
        [Route("api/PlayerData/UpdatePlayer/{id}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult UpdatePlayer(int id, Player player)
        {
            Debug.WriteLine("I have reached the update player method");
            if (!ModelState.IsValid)
            {
                Debug.WriteLine("Model state is invalid");
                return BadRequest(ModelState);
            }

            if (id != player.PlayerID)
            {
                Debug.WriteLine("ID mismatch");
                return BadRequest();
            }

            db.Entry(player).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlayerExists(id))
                {
                    Debug.WriteLine("Player not found");
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
        /// Adds a new player to the database.
        /// </summary>
        /// <param name="player">The player details to add.</param>
        /// <returns>Returns the created player details.</returns>
        /// <example>
        /// POST: api/PlayerData/AddPlayer
        /// </example>
        [HttpPost]
        [Route("api/PlayerData/AddPlayer")]
        [ResponseType(typeof(Player))]
        public IHttpActionResult AddPlayer(Player player)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Ensure the TeamID is valid before adding the player
            var team = db.Teams.Find(player.TeamID);
            if (team == null)
            {
                return BadRequest("Invalid TeamID");
            }

            db.Players.Add(player);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = player.PlayerID }, player);
        }

        /// <summary>
        /// Deletes a specific player by ID.
        /// </summary>
        /// <param name="id">The ID of the player to delete.</param>
        /// <returns>Returns the deleted player details.</returns>
        /// <example>
        /// POST: api/PlayerData/DeletePlayer/5
        /// </example>
        // DELETE: api/PlayerData/DeletePlayer/5
        [HttpPost]
        [Route("api/PlayerData/DeletePlayer/{id}")]
        [ResponseType(typeof(Player))]
        
        public IHttpActionResult DeletePlayer(int id)
        {
            Player player = db.Players.Find(id);
            if (player == null)
            {
                return NotFound();
            }

            db.Players.Remove(player);
            db.SaveChanges();

            return Ok(player);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PlayerExists(int id)
        {
            return db.Players.Count(e => e.PlayerID == id) > 0;
        }
    }
}