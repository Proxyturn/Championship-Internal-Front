using Championship_Internal_Front.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Common;
using System.Net.Http.Headers;
using System.Text;

namespace Championship_Internal_Front.Controllers
{
    public class ApiRefereeController : Controller
    {

        HttpClient client;

        public ApiRefereeController(IHttpClientFactory factory)
        {
            client = factory.CreateClient();
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("Referees")]
        public async Task<IActionResult> RefereeList()
        {

            if (Request.Cookies["AuthToken"] == null) return RedirectToAction("login", "ApiLogin");
            string token = Request.Cookies["AuthToken"];

            client.BaseAddress = new Uri("https://localhost:44334/");
            client.DefaultRequestHeaders.Accept.Add(new
                MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            try
            {
                HttpResponseMessage response = client.GetAsync("api/referee").Result;
                if (response.IsSuccessStatusCode)
                {
                    var listReferee = await response.Content.ReadAsAsync<User[]>();

                    return View(listReferee.ToList());
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

        [Authorize]
        [HttpGet("/protected-resource")]
        public IActionResult Add()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Add(User user)
        {
            try
            {
                client.BaseAddress = new Uri("https://localhost:44334/");
                client.DefaultRequestHeaders.Accept.Add(new
                    MediaTypeWithQualityHeaderValue("application/json"));

                string json = JsonConvert.SerializeObject(user);

                HttpContent content = new StringContent(json, Encoding.Unicode, "application/json");

                var response = await client.PostAsync("api/user", content);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("RefereeList");
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
        [Route("edit_user")]
        public async Task<IActionResult> EditChampionship(User editedUser)
        {
            if (Request.Cookies["AuthToken"] == null) return RedirectToAction("login", "ApiLogin");
            string token = Request.Cookies["AuthToken"];
            try
            {
                client.BaseAddress = new Uri("https://localhost:44334/");
                client.DefaultRequestHeaders.Accept.Add(new
                    MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                string json = JsonConvert.SerializeObject(editedUser);

                HttpContent content = new StringContent(json, Encoding.Unicode, "application/json");

                var response = await client.PutAsync($"api/user/{editedUser.Id}", content);
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
        public async Task<IActionResult> DeleteChampionship(User user)
        {
            if (Request.Cookies["AuthToken"] == null) return RedirectToAction("login", "ApiLogin");
            string token = Request.Cookies["AuthToken"];
            try
            {
                client.BaseAddress = new Uri("https://localhost:44334/");
                client.DefaultRequestHeaders.Accept.Add(new
                    MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await client.DeleteAsync($"api/user/{user.Id}");
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
