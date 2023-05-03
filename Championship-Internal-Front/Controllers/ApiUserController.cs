using Championship_Internal_Front.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Text;

namespace Championship_Internal_Front.Controllers
{
    public class ApiUserController : Controller
    {

        readonly HttpClient client;
        User loggedUser = new();
        private readonly IConfiguration _configuration;

        public ApiUserController(IConfiguration configuration, IHttpClientFactory factory)
        {
            _configuration = configuration;
            client = factory.CreateClient();
        }
        public IActionResult Index()
        {
            return View();
        }

        
        [HttpGet]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            string token;
            if (Request.Cookies["AuthToken"] != null) token = Request.Cookies["AuthToken"];
            else
            {
                return RedirectToAction("login", "Auth");
                
            }


            client.BaseAddress = new Uri("https://champscoreapi.azurewebsites.net/");
            client.DefaultRequestHeaders.Accept.Add(new
            MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            JwtSecurityTokenHandler tokenHandler = new();
            var decodedToken = tokenHandler.ReadJwtToken(token);
            var claims = decodedToken.Claims;
            string userId = (claims.First(claim => claim.Type == "userId").Value);

            HttpResponseMessage response = await client.GetAsync($"api/user/{id}");
            Guid num;
            if(Guid.TryParse(userId, out num))
            {
                loggedUser.Id = num;
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> MyProfile()
        {
            string token;
            if (Request.Cookies["AuthToken"] == null) return RedirectToAction("login", "Auth");
            token = Request.Cookies["AuthToken"];

            client.BaseAddress = new Uri("https://champscoreapi.azurewebsites.net/");
            client.DefaultRequestHeaders.Accept.Add(new
            MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            JwtSecurityTokenHandler tokenHandler = new();
            var decodedToken = tokenHandler.ReadJwtToken(token);
            var claims = decodedToken.Claims;
            string userId = (claims.First(claim => claim.Type == "userId").Value);


            try
            {
                HttpResponseMessage response = client.GetAsync($"api/User/{userId}").Result;
                if (!response.IsSuccessStatusCode) throw new Exception("An error ocurred upon listing");
                var myUser = await response.Content.ReadAsAsync<User>();
                return View(myUser);
            }
            catch (Exception ex)
            {
                return View("_Error", ex);
            }
        }

        public IActionResult EditUser(EditedUser myUser)
        {
            string token;
            if (Request.Cookies["AuthToken"] == null) return RedirectToAction("login", "Auth");
            token = Request.Cookies["AuthToken"];

            client.BaseAddress = new Uri("https://champscoreapi.azurewebsites.net/");
            client.DefaultRequestHeaders.Accept.Add(new
            MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            try
            {
                string json = JsonConvert.SerializeObject(myUser);
                HttpContent content = new StringContent(json, Encoding.Unicode, "application/json");
                HttpResponseMessage response = client.PutAsync($"api/User/Update", content).Result;
                if (!response.IsSuccessStatusCode) throw new Exception("An error ocurred upon listing");
                return RedirectToAction("MyProfile");
            }
            catch (Exception ex)
            {
                return View("_Error", ex);
            }
        }

        [HttpGet]
        [Route("Referees")]
        public async Task<IActionResult> RefereeList(string? idMatch = null)
        {
            if (Request.Cookies["AuthToken"] == null) return RedirectToAction("login", "Auth");
            string token = Request.Cookies["AuthToken"];

            client.BaseAddress = new Uri("https://champscoreapi.azurewebsites.net/");
            client.DefaultRequestHeaders.Accept.Add(new
                MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            try
            {
                HttpResponseMessage response = client.GetAsync($"getByType/{Enums.UserEnum.Referee}").Result;
                if (!response.IsSuccessStatusCode) throw new Exception("An error ocurred upon listing");
                if (idMatch != null) ViewBag.Id = idMatch;
                var listReferee = await response.Content.ReadAsAsync<User[]>();
                return View(listReferee.ToList());
            }
            catch (Exception ex)
            {
                return View("_Error", ex);
            }
        }

        [Route("NewReferee")]
        public IActionResult AddReferee()
        {
            return View();
        }


        public async Task<IActionResult> SubmitReferee(NewUser user)
        {
            try
            {
                client.BaseAddress = new Uri("https://champscoreapi.azurewebsites.net/");
                client.DefaultRequestHeaders.Accept.Add(new
                    MediaTypeWithQualityHeaderValue("application/json"));

                user.UserType = Enums.UserEnum.Referee;
                string json = JsonConvert.SerializeObject(user);

                HttpContent content = new StringContent(json, Encoding.Unicode, "application/json");

                var response = await client.PostAsync("api/user/register", content);
                if (!response.IsSuccessStatusCode)
                {
                    string error = $"{response.StatusCode} - {response.ReasonPhrase}";
                    throw new Exception(error);
                }
                return RedirectToAction("RefereeList");
            }
            catch (Exception ex)
            {
                return View("_Error", ex);
            }
        }

       

        public async Task<IActionResult> EnlistReferee(RefereeToMatch referee_match)
        {
            if (Request.Cookies["AuthToken"] == null) return RedirectToAction("login", "Auth");
            string token = Request.Cookies["AuthToken"];
            try
            {

                client.BaseAddress = new Uri("https://champscoreapi.azurewebsites.net/");
                client.DefaultRequestHeaders.Accept.Add(new
                    MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                string json = JsonConvert.SerializeObject(referee_match);
                HttpContent content = new StringContent(json, Encoding.Unicode, "application/json");
                HttpResponseMessage response = client.PutAsync($"atributteReferee", content).Result;
                if (!response.IsSuccessStatusCode)
                {
                    string error = $"{response.StatusCode} - {response.ReasonPhrase}";
                    throw new Exception(error);
                }
                return RedirectToAction("RefereeList");
            }
            catch (Exception ex)
            {
                return View("_Error", ex);
            }
        }

        public async Task<IActionResult> DeleteUser(string idReferee)
        {
            if (Request.Cookies["AuthToken"] == null) return RedirectToAction("login", "Auth");
            string token = Request.Cookies["AuthToken"];
            try
            {
                client.BaseAddress = new Uri("https://champscoreapi.azurewebsites.net/");
                client.DefaultRequestHeaders.Accept.Add(new
                    MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await client.DeleteAsync($"api/user/{idReferee}");
                if (response.IsSuccessStatusCode) return RedirectToAction("RefereeList");
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
