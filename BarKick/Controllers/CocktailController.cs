using BarKick.Models;
using BarKick.Models.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace BarKick.Controllers
{
    public class CocktailController : Controller
    {
        private static readonly HttpClient client = new HttpClient();
        private JavaScriptSerializer serializer = new JavaScriptSerializer();
        //Setting up the Http Client
        static CocktailController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,

                UseCookies = false
            };

            client = new HttpClient(handler);
            client.BaseAddress = new Uri("https://localhost:44335/api/");
        }
        /// <summary>
        /// Displays view with a list of all CocktailDtos in the system
        /// </summary>
        /// GET: Localhost:xxxx/cocktail/list
        /// <returns> Dynamically rendered webpage using List.cshtml </returns>
        public ActionResult List()
        {
            //creating new list of CocktailDtos
            List<CocktailDto> cocktailDtos = new List<CocktailDto>();

            try
            {    //attempting to contact data api to retrieve the list of cocktails
                string url = "cocktaildata/cocktaillist";
                HttpResponseMessage responseMessage = client.GetAsync(url).Result;

                if (responseMessage.IsSuccessStatusCode)
                {    //if successful, the result is deserialized into the CocktailDto list
                    string responseData = responseMessage.Content.ReadAsStringAsync().Result;
                    cocktailDtos = JsonConvert.DeserializeObject<List<CocktailDto>>(responseData);
                }
                else
                {    //if unsuccessful, the following error will be displayed
                    ViewBag.ErrorMessage = "Failed to retrieve cocktails from the API.";
                }
            }
            catch (Exception ex)
            {    //if an exception is caught, the following error will be displayed
                ViewBag.ErrorMessage = "An error occurred: " + ex.Message;
            }
            //Retruning the List view, populated with cocktailDtos
            return View(cocktailDtos);
        }

        /// <summary>
        /// Displays view with the details of a specific cocktail
        /// </summary>
        /// GET: Localhost:xxxx/cocktail/details/{id}
        /// <param name="id"> The id of the cocktail selected </param>
        /// <returns> Dynamically rendered webpage using Details.cshtml and the information about the selected cocktail</returns>

        public ActionResult Details(int id)
        {
            //creating a new instance of the DetailsCocktail viewmodel
            DetailsCocktail viewModel = new DetailsCocktail();

            try
            {    //attempting to contact the api to receive the cocktail information related to the id
                string url = "cocktaildata/findcocktail/" + id;
                HttpResponseMessage responseMessage = client.GetAsync(url).Result;

                if (responseMessage.IsSuccessStatusCode)
                {
                    //if successful, the response is deserialized and stored in the SelectedCocktail variable and assigned to the same variable with in the view model
                    string responseData = responseMessage.Content.ReadAsStringAsync().Result;
                    CocktailDto selectedCocktail = JsonConvert.DeserializeObject<CocktailDto>(responseData);

                    viewModel.SelectedCocktail = selectedCocktail;
                }
                else if (responseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
                {    //if there is not cocktail with that id, the following message will be displayed
                    ViewBag.ErrorMessage = "Cocktail not found.";
                }
                else
                {    //if there are any other errors, this message will be displayed
                    ViewBag.ErrorMessage = "Failed to retrieve cocktail details.";
                }
            }
            catch (Exception ex)
            {    //if an exception is caught, this error will be displayed
                ViewBag.ErrorMessage = "An error occurred: " + ex.Message;
            }
            //returning the Details view populated with the new instance of the DetailsCocktail view model
            return View(viewModel);
        }


        public ActionResult Error()
        {   //if there is an error displaying the view model, the user will be redirected to the error page

            return View();
        }

        /// <summary>
        /// Displays the New view
        /// </summary>
        /// GET: Localhost:xxxx/cocktail/new
        /// <returns> Dynamically rendered webpage including a form where a user can input information to create a new cocktail, including a dropdown menu containing all the bartenders</returns>

        public ActionResult New()
        {
            //gathering information about all bartenders in the system.
            //GET api/bartenderdata/listbartenders
            string url = "bartenderdata/listbartenders";
            HttpResponseMessage responseMessage = client.GetAsync(url).Result;
            IEnumerable<BartenderDto> BartenderOptions = responseMessage.Content.ReadAsAsync<IEnumerable<BartenderDto>>().Result;
            Debug.WriteLine("New method successful");
            // Returning the New view with BartenderOptions populating the select bartender dropdown menu
            return View(BartenderOptions);
        }

        /// <summary>
        /// Serializes the Cocktail Object created from the form data and sends it to the api 
        /// </summary>
        /// POST: Localhost:xxxx/cocktail/Create
        /// <param name="cocktail"> a new instance of the Cocktail object created with information input by the user </param>
        /// <returns> Redirection to the cocktail List page</returns>
        [HttpPost]
        public ActionResult Create(Cocktail cocktail)
        {

            Debug.WriteLine("Json payload: ");
            Debug.WriteLine(cocktail.DrinkName);

            string url = "cocktaildata/AddCocktail";
            string jsonpayload = JsonConvert.SerializeObject(cocktail);

            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload, Encoding.UTF8, "application/json");

            HttpResponseMessage responseMessage = client.PostAsync(url, content).Result;

            return RedirectToAction("List");

        }

        /// <summary>
        /// Displays a view of a form populated with the current data of a cocktail that the user would like to update
        /// </summary>
        /// GET: Localhost:xxxx/cocktail/edit/{id}
        /// <param name="id"> The id of the cocktail selected </param>
        /// <returns> Dynamically rendered webpage using Edit.cshtml and the information about the selected cocktail</returns>

        public ActionResult Edit(int id)
        {
            try
            {
                UpdateCocktail viewModel = new UpdateCocktail();

                // Get selected cocktail information by ID
                string cocktailUrl = "cocktaildata/findcocktail/" + id;
                HttpResponseMessage cocktailResponse = client.GetAsync(cocktailUrl).Result;
                if (!cocktailResponse.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Error fetching cocktail: " + cocktailResponse.StatusCode);
                    return RedirectToAction("Error");
                }

                CocktailDto selectedCocktail = cocktailResponse.Content.ReadAsAsync<CocktailDto>().Result;
                viewModel.SelectedCocktail = selectedCocktail;

                // Get list of bartenders
                string bartendersUrl = "bartenderdata/listbartenders/";
                HttpResponseMessage bartendersResponse = client.GetAsync(bartendersUrl).Result;
                if (!bartendersResponse.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Error fetching bartenders: " + bartendersResponse.StatusCode);
                    return RedirectToAction("Error");
                }

                IEnumerable<BartenderDto> bartenderOptions = bartendersResponse.Content.ReadAsAsync<IEnumerable<BartenderDto>>().Result;
                viewModel.BartenderOptions = bartenderOptions;

                return View(viewModel);
            }
            catch (Exception ex)
            {

                Debug.WriteLine("Exception occurred: " + ex.Message);
                return RedirectToAction("Error");
            }
        }
        /// <summary>
        /// Serializes the Cocktail Object (updated with information from the user) and sends it to the api 
        /// </summary>
        /// POST: Localhost:xxxx/cocktail/Update
        /// <param name="id"> the id number of the coocktail which will be updated </param>
        /// <param name="cocktail"> a new instance of the Cocktail object updated with information input by the user </param>
        /// <returns> Redirection to the cocktail List page</returns>
        /// POST: Cocktail/Update/id
        [HttpPost]
        [Route("api/cocktaildata/UpdateCocktail/{id}")]
        public ActionResult Update(int id, Cocktail cocktail)
        {
            try
            {
                string url = "cocktaildata/UpdateCocktail/" + id;
                string jsonPayload = JsonConvert.SerializeObject(cocktail);
                HttpContent content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                HttpResponseMessage responseMessage = client.PostAsync(url, content).Result;

                if (responseMessage.IsSuccessStatusCode)
                {
                    return RedirectToAction("List");
                }
                else
                {
                    Debug.WriteLine("Error response: " + responseMessage.StatusCode);

                    return RedirectToAction("Error");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception occurred: " + ex.Message);

                return RedirectToAction("Error");
            }
        }

        /// <summary>
        /// Displays a view of a page confirming that the user wants to delete a selected cocktail
        /// </summary>
        /// GET: Localhost:xxxx/cocktail/DeleteConfirm/id
        /// <param name="id"> The id of the cocktail selected </param>
        /// <returns> Dynamically rendered webpage using DeleteConfirm.cshtml and information about the selected cocktail</returns>

        public ActionResult DeleteConfirm(int id)
        {
            string url = "cocktaildata/findcocktail/" + id;
            HttpResponseMessage responseMessage = client.GetAsync(url).Result;
            CocktailDto selectedcocktail = responseMessage.Content.ReadAsAsync<CocktailDto>().Result;
            return View(selectedcocktail);
        }

        /// <summary>
        /// sends the id of a cocktail to the api to delete it 
        /// </summary>
        /// POST: Localhost:xxxx/cocktail/Delete
        /// <param name="id"> the id number of the cocktail which will be deleted </param>
        /// <returns> Redirection to the cocktail List page</returns>
        /// POST: Cocktail/Update/id
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = $"cocktaildata/DeleteCocktail/{id}";

            HttpContent content = new StringContent(""); // No content needed to delete
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            HttpResponseMessage responseMessage = client.PostAsync(url, content).Result;

            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                Debug.WriteLine("Error: response message: " + responseMessage.ReasonPhrase);
                Debug.WriteLine("StatusCode: " + responseMessage.StatusCode);
                Debug.WriteLine("Content: " + responseMessage.Content.ReadAsStringAsync().Result);
                return RedirectToAction("Error");
            }
        }


    }
}