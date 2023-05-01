using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Championship_Internal_Front.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Net.Http.Headers;
using Championship_Internal_Front.Models;
using Microsoft.AspNetCore.Authorization;
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


        //[Route("login")]
        //public IActionResult Login()
        //{
        //    return View();
        //}

        //[Route("signup")]
        //public IActionResult RegisterAccount()
        //{
        //    return View();
        //}


        [Route("login")]
        public IActionResult Login()
        {
            return View();
        }

        [Route("signup")]
        public IActionResult RegisterAccount()
        {
            return View();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginAuth(LoginModel login)
        {


            string authSigningKey = _configuration["TokenConfigurations:JwtKey"];
            client.BaseAddress = new Uri("https://champscoreapi.azurewebsites.net/");
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
                    JwtSecurityTokenHandler tokenHandler = new();
                    var token = tokenHandler.ReadJwtToken(status_token.token);
                    var claims = token.Claims;
                    string userId = (claims.First(claim => claim.Type == "userId").Value);
                    Guid id;
                    Guid.TryParse(userId, out id);

                    //UserServices userService = new(status_token.token);
                    //userService.GetByUserId(id);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    Response status_message = await response.Content.ReadAsAsync<Response>();
                    if (status_message.Status == "404") throw new Exception("Usuário não encontrado");
                    throw new Exception("Ocorreu um erro com o Login, favor voltar e tentar novamente");
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

            client.BaseAddress = new Uri("https://champscoreapi.azurewebsites.net/");
            client.DefaultRequestHeaders.Accept.Add(new
            MediaTypeWithQualityHeaderValue("application/json"));
            user.UserType = 0;
            string data = JsonConvert.SerializeObject(user);
            StringContent httpContent = new StringContent(data, System.Text.Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await client.PostAsync("api/user/register", httpContent);

                if (response.IsSuccessStatusCode)
                {
                    
                    return RedirectToAction("Index", "Home");
                }
                //else
                //{
                //    throw new Exception("An error ocurred upon listing");
                //}
            }
            catch (Exception ex)
            {
                return View("_Error", ex);
            }

            return RedirectToAction("login", "Auth");
        }

        public IActionResult Logout()
        {
            if (Request.Cookies["AuthToken"] != null) Response.Cookies.Delete("AuthToken");
            return RedirectToAction("login", "Auth");
        }

        public bool TokenExists()
        {
            if (Request.Cookies["AuthToken"] != null) return true;
            else return false;
        }
        //[HttpPost]
        //public async Task<IActionResult> Register([FromBody] RegisterModel model)
        //{
        //    var userExists = await _userManager.FindByNameAsync(model.Email);
        //    if (userExists != null)
        //        return StatusCode(
        //            StatusCodes.Status500InternalServerError,
        //            new Response { Status = "Error", Message = "Este usuário já existe" });

        //    IdentityUser user = new()
        //    {
        //        Email = model.Email,
        //        SecurityStamp = Guid.NewGuid().ToString(),
        //        UserName = model.Username
        //    };
        //    var result = await _userManager.CreateAsync(user, model.Password);
        //    if (!result.Succeeded)
        //        return StatusCode(
        //            StatusCodes.Status500InternalServerError,
        //            new Response { Status = "Error", Message = "Ocorreu um erro na criação do usuário" });

        //    return Ok(new Response { Status = "Success", Message = "Usuário criado com sucesso!" });
        //}
    }
}
