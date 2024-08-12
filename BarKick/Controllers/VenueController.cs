using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using BarKick.Models;
using BarKick.Models.ViewModels;

namespace BarKick.Controllers
{
    public class VenueController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static VenueController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44335/api/");
        }

        // GET: Venue/List
        public ActionResult List()
        {
            string url = "VenueData/ListVenues";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<VenueDto> venues = response.Content.ReadAsAsync<IEnumerable<VenueDto>>().Result;
            return View(venues);
        }

        // GET: Venue/Details/5
        [Route("Venue/Details/{id}")]
        public async Task<ActionResult> Details(int id)
        {
            var viewModel = new DetailsVenue();

            // Fetch the venue details
            string venueUrl = $"VenueData/FindVenue/{id}"; // Use string interpolation
            HttpResponseMessage venueResponse = await client.GetAsync(venueUrl);
            if (!venueResponse.IsSuccessStatusCode)
            {
                // Handle error
                return HttpNotFound();
            }
            VenueDto selectedVenue = await venueResponse.Content.ReadAsAsync<VenueDto>();
            viewModel.Venue = selectedVenue;

            // Fetch the list of teams associated with the venue
            string teamsUrl = $"VenueData/ListTeamsForVenue/{id}"; // Use string interpolation
            HttpResponseMessage teamsResponse = await client.GetAsync(teamsUrl);
            if (!teamsResponse.IsSuccessStatusCode)
            {
                // Handle errors
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
            IEnumerable<TeamDto> teams = await teamsResponse.Content.ReadAsAsync<IEnumerable<TeamDto>>();
            viewModel.Teams = teams;

            // Fetch the list of all bartenders associated with the venue
            string bartendersUrl = $"VenueData/ListBartendersForVenue/{id}"; // Use string interpolation
            HttpResponseMessage bartendersResponse = await client.GetAsync(bartendersUrl);
            if (!bartendersResponse.IsSuccessStatusCode)
            {
                // Handle errors 
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
            IEnumerable<BartenderDto> bartenders = await bartendersResponse.Content.ReadAsAsync<IEnumerable<BartenderDto>>();
            viewModel.Bartenders = bartenders;

            // Fetch the list of all available bartenders
            string availableBartendersUrl = "BartenderData/ListBartenders"; // No need to include id here
            HttpResponseMessage availableBartendersResponse = await client.GetAsync(availableBartendersUrl);
            if (!availableBartendersResponse.IsSuccessStatusCode)
            {
                // Handle errors 
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
            IEnumerable<BartenderDto> availableBartenders = await availableBartendersResponse.Content.ReadAsAsync<IEnumerable<BartenderDto>>();
            viewModel.AvailableBartenders = availableBartenders;

            return View(viewModel);
        }




        // GET: Venue/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Venue/Create
        [HttpPost]
        public ActionResult Create(VenueDto venueDto)
        {
            if (!ModelState.IsValid)
            {
                return View(venueDto); // Returning the view with the model to display validation errors
            }

            string url = "VenueData/AddVenue";
            string jsonPayload = jss.Serialize(venueDto);
            HttpContent content = new StringContent(jsonPayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

                return RedirectToAction("List");
          
        }
    


        // GET: Venue/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "VenueData/FindVenue/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            VenueDto venueDto = response.Content.ReadAsAsync<VenueDto>().Result;
            return View(venueDto);
        }

        // POST: Venue/Update/5
        [HttpPost]
        public ActionResult Update(int id, VenueDto venueDto)
        {
            string url = "VenueData/UpdateVenue/" + id;
            string jsonPayload = jss.Serialize(venueDto);
            HttpContent content = new StringContent(jsonPayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                ModelState.AddModelError("", "Unable to update venue. Try again later.");
                return View("Edit", venueDto);
            }
        }

        // GET: Venue/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "VenueData/FindVenue/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            VenueDto venueDto = response.Content.ReadAsAsync<VenueDto>().Result;
            return View(venueDto);
        }

        // POST: Venue/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "VenueData/DeleteVenue/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            return RedirectToAction("List");
        }

        // GET: Venue/ListTeamsForVenue/5
        public ActionResult ListTeamsForVenue(int id)
        {
            string url = "VenueData/ListTeamsForVenue/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<TeamDto> teams = response.Content.ReadAsAsync<IEnumerable<TeamDto>>().Result;
            return View(teams);
        }

        // GET: Venue/AddTeamToVenue/5
        public ActionResult AddTeamToVenue(int id)
        {
            string url = "VenueData/FindVenue/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            VenueDto venueDto = response.Content.ReadAsAsync<VenueDto>().Result;

            string teamUrl = "TeamData/ListTeams";
            HttpResponseMessage teamResponse = client.GetAsync(teamUrl).Result;
            IEnumerable<TeamDto> allTeams = teamResponse.Content.ReadAsAsync<IEnumerable<TeamDto>>().Result;

            var viewModel = new AddTeamToVenueViewModel
            {
                Venue = venueDto,
                Teams = allTeams
            };

            return View(viewModel);
        }

        // POST: Venue/AddTeamToVenue
        [HttpPost]
        public ActionResult AddTeamToVenue(int venueId, int teamId)
        {
            string url = "VenueData/AddTeamToVenue/" + venueId + "/" + teamId;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            return RedirectToAction("Details", new { id = venueId });
        }

        // POST: Venue/RemoveTeamFromVenue
        [HttpPost]
        public ActionResult RemoveTeamFromVenue(int venueId, int teamId)
        {
            string url = "VenueData/RemoveTeamFromVenue/" + venueId + "/" + teamId;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            return RedirectToAction("Details", new { id = venueId });
        }
    }
}