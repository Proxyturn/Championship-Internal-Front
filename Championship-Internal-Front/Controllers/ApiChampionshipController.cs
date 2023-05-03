using Championship_Internal_Front.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;

namespace Championship_Internal_Front.Controllers
{
	
    public class ApiChampionshipController : Controller
    {
        HttpClient client;

        public ApiChampionshipController(IHttpClientFactory factory)
        {
            client = factory.CreateClient();
        }

        public IActionResult Index()
        {
            return View();
        }


        [Route("championships")]
        public async Task<IActionResult> List()
        {
            if (Request.Cookies["AuthToken"] == null) return RedirectToAction("login", "Auth");
			string? token = Request.Cookies["AuthToken"];

            client.BaseAddress = new Uri("https://champscoreapi.azurewebsites.net/");
            client.DefaultRequestHeaders.Accept.Add(new
                MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            try
			{
				HttpResponseMessage response = client.GetAsync("internal").Result;
				if (!response.IsSuccessStatusCode) throw new Exception("An error ocurred upon listing");
                var listChampionships = await response.Content.ReadAsAsync<ChampionshipInternal[]>();
                return View(listChampionships.ToList());
            }
			catch (Exception ex)
			{
				return View("_Error", ex);
			}
		}

        

		
        [Route("championship_details")]
        public async Task<IActionResult> GetChampionshipById(string championship_id)
		{

            if (Request.Cookies["AuthToken"] == null) return RedirectToAction("login", "Auth");
            string? token = Request.Cookies["AuthToken"];

            Guid id;
            Guid.TryParse(championship_id, out id);
            NewChampionship champ = new();
            client.BaseAddress = new Uri("https://champscoreapi.azurewebsites.net/");
            client.DefaultRequestHeaders.Accept.Add(new
                MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            try
            {
                HttpResponseMessage response = client.GetAsync($"internal/{id}").Result;
                if (!response.IsSuccessStatusCode) throw new Exception("An error ocurred upon listing");
                ChampionshipExternalDetailed championship = await response.Content.ReadAsAsync<ChampionshipExternalDetailed>();
                if (championship.Matchs != null)
                {
                    List<List<Match>> matchList = new();
                    for(int i = 0; i < championship.TotalPhases; i++)
                    {
                        matchList.Add(
                                (from matchs in championship.Matchs
                                where matchs.PhaseNumber == i+1
                                select matchs).ToList());
                    }
                    ViewBag.MatchList = matchList;
                }
                return View(championship);
            }
            catch (Exception ex)
            {
                return View("_Error", ex);
            }
		}

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddChampionship(NewChampionship championship)
        {
            if (Request.Cookies["AuthToken"] == null) return RedirectToAction("login", "Auth");
            string? token = Request.Cookies["AuthToken"];
            try
            {
                client.BaseAddress = new Uri("https://champscoreapi.azurewebsites.net/");
                client.DefaultRequestHeaders.Accept.Add(new
                    MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                string json = JsonConvert.SerializeObject(championship);

                HttpContent content = new StringContent(json, Encoding.Unicode, "application/json");

                var response = await client.PostAsync("api/championship", content);
                if (!response.IsSuccessStatusCode)
                {
                    string error = $"{response.StatusCode} - {response.ReasonPhrase}";
                    throw new Exception(error);
                }
                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                return View("_Error", ex);
            }
        }

        [Route("SubmitChampionship")]
        public IActionResult EditChampionship(string id, string title, string description, DateTime startdate, int totalphases)
        {
            Guid championship_id;
            Guid.TryParse(id, out championship_id);
            NewChampionship championship = new();
            championship.Id = championship_id;
            championship.Title = title;
            championship.Description = description;
            championship.StartDate = startdate;
            championship.TotalPhases = totalphases;
            return View(championship);
        }

        
        public async Task<IActionResult> SubmitChampionship(NewChampionship editedChampionship)
        {
            if (Request.Cookies["AuthToken"] == null) return RedirectToAction("login", "Auth");
            string? token = Request.Cookies["AuthToken"];
            try
            {
                client.BaseAddress = new Uri("https://champscoreapi.azurewebsites.net/");
                client.DefaultRequestHeaders.Accept.Add(new
                    MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                string json = JsonConvert.SerializeObject(editedChampionship);

                HttpContent content = new StringContent(json, Encoding.Unicode, "application/json");

                var response = await client.PutAsync($"api/championship", content);
                if (!response.IsSuccessStatusCode)
                {
                    string error = $"{response.StatusCode} - {response.ReasonPhrase}";
                    throw new Exception(error);
                }
                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                return View("_Error", ex);
            }
        }

        
        public async Task<IActionResult> DeleteChampionship(string id)
        {
            if (Request.Cookies["AuthToken"] == null) return RedirectToAction("login", "Auth");
            string? token = Request.Cookies["AuthToken"];
            try
            {
                client.BaseAddress = new Uri("https://champscoreapi.azurewebsites.net/");
                client.DefaultRequestHeaders.Accept.Add(new
                    MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await client.DeleteAsync($"api/championship/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    string error = $"{response.StatusCode} - {response.ReasonPhrase}";
                    throw new Exception(error);
                }
                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                return View("_Error", ex);
            }
        }

        public async Task<IActionResult> StartChampionship(string id)
        {
            if (Request.Cookies["AuthToken"] == null) return RedirectToAction("login", "Auth");
            string? token = Request.Cookies["AuthToken"];
            try
            {
                client.BaseAddress = new Uri("https://champscoreapi.azurewebsites.net/");
                client.DefaultRequestHeaders.Accept.Add(new
                    MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                string json = JsonConvert.SerializeObject(id);

                HttpContent content = new StringContent(json, Encoding.Unicode, "application/json");

                var response = await client.PostAsync($"startChampionship", content);
                if (!response.IsSuccessStatusCode)
                {
                    string error = $"{response.StatusCode} - {response.ReasonPhrase}";
                    throw new Exception(error);
                }
                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                return View("_Error", ex);
            }
        }

        public async Task<IActionResult> EndChampionship(string id)
        {
            if (Request.Cookies["AuthToken"] == null) return RedirectToAction("login", "Auth");
            string? token = Request.Cookies["AuthToken"];
            try
            {
                client.BaseAddress = new Uri("https://champscoreapi.azurewebsites.net/");
                client.DefaultRequestHeaders.Accept.Add(new
                    MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                string json = JsonConvert.SerializeObject(id);

                HttpContent content = new StringContent(json, Encoding.Unicode, "application/json");

                var response = await client.PostAsync($"finishChampionship", content);
                if (!response.IsSuccessStatusCode)
                {
                    string error = $"{response.StatusCode} - {response.ReasonPhrase}";
                    throw new Exception(error);
                }
                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                return View("_Error", ex);
            }
        }
    }
}
