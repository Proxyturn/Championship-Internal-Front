using Azure.Core;
using Championship_Internal_Front.Models;
using Championship_Internal_Front.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using NuGet.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
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
                return RedirectToAction("login", "ApiLogin");
                
            } 
                

            //UserServices userService = new(token);
            //userService.GetByUserId(userId);

            client.BaseAddress = new Uri("https://localhost:44334/");
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
                client.BaseAddress = new Uri("https://champscoreapi.azurewebsites.net/");
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
        public async Task<IActionResult> EditUser(User editedUser)
        {
            if (Request.Cookies["AuthToken"] == null) return RedirectToAction("login", "ApiLogin");
            string token = Request.Cookies["AuthToken"];
            try
            {
                client.BaseAddress = new Uri("https://champscoreapi.azurewebsites.net/");
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
        public async Task<IActionResult> DeleteUser(User user)
        {
            if (Request.Cookies["AuthToken"] == null) return RedirectToAction("login", "ApiLogin");
            string token = Request.Cookies["AuthToken"];
            try
            {
                client.BaseAddress = new Uri("https://champscoreapi.azurewebsites.net/");
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
