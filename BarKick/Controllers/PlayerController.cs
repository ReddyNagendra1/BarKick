using BarKick.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;

namespace BarKick.Controllers
{
    public class PlayerController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static PlayerController()
        {


            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44335/api/");
        }

        // GET: Player/List
        [Route("Player/List")]
        public ActionResult List()
        {
            //Objective: communicate with our player data api to retrieve a list of player
            //curl "https://localhost:44338/api/playerdata/listplayers


            string url = "playerdata/listplayers";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<PlayerDto> players = response.Content.ReadAsAsync<IEnumerable<PlayerDto>>().Result;
            //Debug.WriteLine("The response code ");
            //Debug.WriteLine(response.StatusCode);
            //Debug.WriteLine("Number of players received : ");
            //Debug.WriteLine(players.Count());

            return View(players);
        }

        // GET: Player/Details/5
        public ActionResult Details(int id)
        {
            //Objective: communicate with our player data api to retrieve a list of one player
            //curl "https://localhost:44338/api/playerdata/findplayer/{id}

            string url = "playerdata/findplayer/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            PlayerDto SelectedPlayer = response.Content.ReadAsAsync<PlayerDto>().Result;

            // ViewModel.SelectedPlayer = SelectedPlayer;

            //return View(ViewModel);
            //Debug.WriteLine("The response code ");
            //Debug.WriteLine(response.StatusCode);
            //Debug.WriteLine("Player received : ");
            //Debug.WriteLine(SelectedPlayer.PlayerName);

            return View(SelectedPlayer);
        }

        public ActionResult Error()
        {

            return View();
        }


        // GET: Player/New
        public ActionResult New()
        {
            var teams = db.Teams.ToList();
            ViewBag.Teams = teams;
            return View();
        }
        // POST: Player/Create
        [HttpPost]
        public ActionResult Create(PlayerDto playerDto)
        {
            Debug.WriteLine("the json payload is :");
            Debug.WriteLine(playerDto.PlayerName);
            //Objective: Add a new player into out system using API
            //curl -d @player.json -H "Content-type:application/json" https://localhost:44335/api/playerdata/addplayer
            string url = "playerdata/AddPlayer";


            string jsonpayload = jss.Serialize(playerDto);
            Debug.WriteLine(jsonpayload);


            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("List");

        }


        // GET: Player/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "playerdata/findplayer/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            PlayerDto playerDto = response.Content.ReadAsAsync<PlayerDto>().Result;

            var teams = db.Teams.ToList();
            ViewBag.Teams = teams;

            return View(playerDto);
        }

        // POST: Player/Update/5
        [HttpPost]
        public ActionResult Update(int id, PlayerDto playerDto)
        {
            if (id != playerDto.PlayerID)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            string url = "playerdata/updateplayer/" + id;
            string jsonpayload = jss.Serialize(playerDto);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Player/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "playerdata/FindPlayer/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            PlayerDto PlayerDto = response.Content.ReadAsAsync<PlayerDto>().Result;
            return View(PlayerDto);
        }

        // POST: Player/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "playerdata/DeletePlayer/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("List");
        }
    }
}