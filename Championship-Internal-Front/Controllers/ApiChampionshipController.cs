using Championship_Internal_Front.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using NuGet.Common;

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


        [Route("Championships")]
        public async Task<IActionResult> List()
        {


            if (Request.Cookies["AuthToken"] == null) return RedirectToAction("login", "ApiLogin");

			string token = Request.Cookies["AuthToken"];

            
            client.BaseAddress = new Uri("https://localhost:44334/");
            client.DefaultRequestHeaders.Accept.Add(new
                MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


            try
			{
				HttpResponseMessage response = client.GetAsync("internal").Result;
				if (response.IsSuccessStatusCode)
				{
					var listChampionships = await response.Content.ReadAsAsync<ChampionshipInternal[]>();
					//var listaForuns = await response.Content.ReadFromJsonAsync<ForumClient[]>();

					return View(listChampionships.ToList());
				}
				else
				{
					throw new Exception("An error ocurred upon listing");
				}
			}
			catch (Exception ex)
			{
				return View("_Error", ex);
			}
		}

        [Route("championship_details")]
        public IActionResult GetChampionshipById() { return View(); }

		[HttpGet]
        
        public async Task<IActionResult> GetChampionshipById(Guid id)
		{
            if (Request.Cookies["AuthToken"] == null) return RedirectToAction("login", "ApiLogin");
            string token = Request.Cookies["AuthToken"];

            client.BaseAddress = new Uri("https://localhost:44334/");
            client.DefaultRequestHeaders.Accept.Add(new
                MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            try
            {
                HttpResponseMessage response = client.GetAsync($"internal/{id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    var Championship = await response.Content.ReadAsAsync<ChampionshipInternal[]>();
                    //var listaForuns = await response.Content.ReadFromJsonAsync<ForumClient[]>();

                    return View(Championship);
                }
                else
                {
                    throw new Exception("An error ocurred upon listing");
                }
            }
            catch (Exception ex)
            {
                return View("_Error", ex);
            }

            return RedirectToAction();
		}


        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddChampionship(NewChampionship championship)
        {
            if (Request.Cookies["AuthToken"] == null) return RedirectToAction("login", "ApiLogin");
            string token = Request.Cookies["AuthToken"];
            try
            {
                client.BaseAddress = new Uri("https://localhost:44334/");
                client.DefaultRequestHeaders.Accept.Add(new
                    MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                string json = JsonConvert.SerializeObject(championship);

                HttpContent content = new StringContent(json, Encoding.Unicode, "application/json");

                var response = await client.PostAsync("api/championship", content);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("List");
                }
                else
                {
                    string error = $"{response.StatusCode} - {response.ReasonPhrase}";
                    throw new Exception(error);
                }
            }
            catch (Exception ex)
            {

                return View("_Error", ex);
            }
        }

        [HttpPut]
        [Route("edit_championship")]
        public async Task<IActionResult> EditChampionship(ChampionshipInternal editedchampionship)
        {
            if (Request.Cookies["AuthToken"] == null) return RedirectToAction("login", "ApiLogin");
            string token = Request.Cookies["AuthToken"];
            try
            {
                client.BaseAddress = new Uri("https://localhost:44334/");
                client.DefaultRequestHeaders.Accept.Add(new
                    MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                string json = JsonConvert.SerializeObject(editedchampionship);

                HttpContent content = new StringContent(json, Encoding.Unicode, "application/json");

                var response = await client.PutAsync($"api/championship/{editedchampionship.Id}", content);
                if (response.IsSuccessStatusCode) return RedirectToAction("List");
                else
                {
                    string error = $"{response.StatusCode} - {response.ReasonPhrase}";
                    throw new Exception(error);
                }
            }
            catch (Exception ex)
            {
                return View("_Error", ex);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteChampionship(ChampionshipInternal championship)
        {
            if (Request.Cookies["AuthToken"] == null) return RedirectToAction("login", "ApiLogin");
            string token = Request.Cookies["AuthToken"];
            try
            {
                client.BaseAddress = new Uri("https://localhost:44334/");
                client.DefaultRequestHeaders.Accept.Add(new
                    MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await client.DeleteAsync($"api/championship/{championship.Id}");
                if (response.IsSuccessStatusCode) return RedirectToAction("List");
                else
                {
                    string error = $"{response.StatusCode} - {response.ReasonPhrase}";
                    throw new Exception(error);
                }
            }
            catch (Exception ex)
            {
                return View("_Error", ex);
            }
        }
    }
}
