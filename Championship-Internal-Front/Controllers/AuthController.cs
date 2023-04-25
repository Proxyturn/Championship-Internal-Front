using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Championship_Internal_Front.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using Championship_Internal_Front.Models;
using Newtonsoft.Json;

namespace Championship_Internal_Front.Controllers
{
    public class AuthController : Controller
    {

        HttpClient client;

        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthController(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration, IHttpClientFactory factory)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            client = factory.CreateClient();
        }

        //AuthController() { }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginAuth(LoginModel login)
        {


            string authSigningKey = _configuration["TokenConfigurations:JwtKey"];
            client.BaseAddress = new Uri("https://localhost:44334/");
            client.DefaultRequestHeaders.Accept.Add(new
            MediaTypeWithQualityHeaderValue("application/json"));
                
            
            string data = JsonConvert.SerializeObject(login);

            //var tokenHandler = new JwtSecurityTokenHandler();
            //var token = new System.IdentityModel.Tokens.JwtSecurityToken(authSigningKey);
            StringContent httpContent = new StringContent(data, System.Text.Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await client.PostAsync("api/user/login", httpContent);

                if (response.IsSuccessStatusCode)
                {
                    User user = new();
                    Response status_token = await response.Content.ReadAsAsync<Response>();
                    
                    Response.Cookies.Append("AuthToken", status_token.token, new CookieOptions() 
                    { 
                        HttpOnly = true, 
                        SameSite = SameSiteMode.Strict, 
                        Expires = DateTime.Now.AddHours(2) 
                    });
                    JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                    var token = tokenHandler.ReadJwtToken(status_token.token);
                    var claims = token.Claims;
                    string userId = (claims.First(claim => claim.Type == "userId").Value);
                    Guid id;
                    Guid.TryParse(userId, out id);

                    return RedirectToAction("Index", "Home");
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

        [HttpPost]
        [Route("signup")]
        public async Task<IActionResult> SignUp(NewUser user)
        {

            client.BaseAddress = new Uri("https://localhost:44334/");
            client.DefaultRequestHeaders.Accept.Add(new
            MediaTypeWithQualityHeaderValue("application/json"));

            string data = JsonConvert.SerializeObject(user);
            StringContent httpContent = new StringContent(data, System.Text.Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await client.PostAsync("api/user/register", httpContent);

                if (response.IsSuccessStatusCode)
                {
                    Response status_token = await response.Content.ReadAsAsync<Response>();

                    Response.Cookies.Append("AuthToken", status_token.token, new CookieOptions()
                    {
                        HttpOnly = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTime.Now.AddHours(2)
                    });

                    return RedirectToAction("Index", "Home");
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

        public IActionResult Logout()
        {
            if (Request.Cookies["AuthToken"] != null) Response.Cookies.Delete("AuthToken");
            return RedirectToAction("login", "ApiLogin");
        }

        public bool TokenExists()
        {
            if (Request.Cookies["AuthToken"] != null) return true;
            else return false;
        }
    }
}
