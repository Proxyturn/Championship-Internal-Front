using Championship_Internal_Front.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Championship_Internal_Front.Controllers
{
    public class ApiMatchController : Controller
    {
        HttpClient client;

        public ApiMatchController(IHttpClientFactory factory)
        {
            client = factory.CreateClient();
        }

        public IActionResult Index()
        {
            return View();
        }


        //[Route("matchs")]
        //public async Task<IActionResult> List()
        //{

        //    if (Request.Cookies["AuthToken"] == null) return RedirectToAction("login", "ApiLogin");
        //    string token = Request.Cookies["AuthToken"];

        //    client.BaseAddress = new Uri("http://localhost:7232/");
        //    client.DefaultRequestHeaders.Accept.Add(new
        //        MediaTypeWithQualityHeaderValue("application/json"));
        //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


        //    try
        //    {
        //        HttpResponseMessage response = client.GetAsync("internal").Result;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            var listChampionships = await response.Content.ReadAsAsync<ChampionshipInternal[]>();
        //            //var listaForuns = await response.Content.ReadFromJsonAsync<ForumClient[]>();

        //            return View(listChampionships.ToList());
        //        }
        //        else
        //        {
        //            throw new Exception("An error ocurred upon listing");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return View("_Error", ex);
        //    }
        //}


        [HttpGet]
        [Route("match_details")]
        public async Task<IActionResult> GetMatchById(Guid id)
        {
            if (Request.Cookies["AuthToken"] == null) return RedirectToAction("login", "ApiLogin");
            string? token = Request.Cookies["AuthToken"];

            client.BaseAddress = new Uri("http://localhost:7232/");
            client.DefaultRequestHeaders.Accept.Add(new
                MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            try
            {
                HttpResponseMessage response = client.GetAsync($"GetMatchById/{id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    var match = await response.Content.ReadAsAsync<Match[]>();
                    //var listaForuns = await response.Content.ReadFromJsonAsync<ForumClient[]>();

                    return View(match);
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
        [Route("match_by_referee")]
        public async Task<IActionResult> GetByRefereeId(Guid id)
        {
            if (Request.Cookies["AuthToken"] == null) return RedirectToAction("login", "ApiLogin");
            string? token = Request.Cookies["AuthToken"];

            client.BaseAddress = new Uri("http://localhost:7232/");
            client.DefaultRequestHeaders.Accept.Add(new
                MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            try
            {
                HttpResponseMessage response = client.GetAsync($"getByRefereeId/{id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    var match = await response.Content.ReadAsAsync<Match[]>();
                    //var listaForuns = await response.Content.ReadFromJsonAsync<ForumClient[]>();

                    return View(match);
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

    }
}
