using Championship_Internal_Front.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using NuGet.Common;
using System;

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


            if (Request.Cookies["AuthToken"] == null) return RedirectToAction("login", "ApiLogin");

			string? token = Request.Cookies["AuthToken"];

            
            client.BaseAddress = new Uri("http://localhost:7232/");
            client.DefaultRequestHeaders.Accept.Add(new
                MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


            try
			{
				HttpResponseMessage response = client.GetAsync("external").Result;
				if (response.IsSuccessStatusCode)
				{
					var listChampionships = await response.Content.ReadAsAsync<ChampionshipInternal[]>();
					
                    //foreach(ChampionshipInternal championship in listChampionships)
                    //{
                    //    if (championship.Status == Enums.ChampionshipStatusEnum.Created);
                    //}
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
        public async Task<IActionResult> GetChampionshipById(string championship_id)
		{

            Guid id;
            if(Guid.TryParse(championship_id, out id))

            if (Request.Cookies["AuthToken"] == null) return RedirectToAction("login", "ApiLogin");
            string? token = Request.Cookies["AuthToken"];

            client.BaseAddress = new Uri("http://localhost:7232/");
            client.DefaultRequestHeaders.Accept.Add(new
                MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            try
            {
                HttpResponseMessage response = client.GetAsync($"external/{id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    var Championship = await response.Content.ReadAsAsync<ChampionshipExternalDetailed>();

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
		}


        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddChampionship(ChampionshipInternal championship)
        {
            if (Request.Cookies["AuthToken"] == null) return RedirectToAction("login", "ApiLogin");
            string? token = Request.Cookies["AuthToken"];
            try
            {
                client.BaseAddress = new Uri("http://localhost:7232/");
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
        public async Task<IActionResult> EditChampionship(ChampionshipInternal editedchampionship)
        {
            if (Request.Cookies["AuthToken"] == null) return RedirectToAction("login", "ApiLogin");
            string? token = Request.Cookies["AuthToken"];
            try
            {
                client.BaseAddress = new Uri("http://localhost:7232/");
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
            string? token = Request.Cookies["AuthToken"];
            try
            {
                client.BaseAddress = new Uri("http://localhost:7232/");
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
