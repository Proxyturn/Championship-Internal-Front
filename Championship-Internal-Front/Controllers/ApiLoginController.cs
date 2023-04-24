 using Championship_Internal_Front.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;


namespace Championship_Internal_Front.Controllers
{

    [ApiController]
    public class ApiLoginController : Controller
    {

        HttpClient client;


        private IConfiguration _config;
        public ApiLoginController(IConfiguration configuration, IHttpClientFactory factory)
        {
            _config = configuration;
            client = factory.CreateClient();
        }
        
     

        //private string GenerateToken( User users )
        //{
        //    var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["TokenConfigurations:JwtKey"]));
        //    var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);
        //    var token = new JwtSecurityToken(_config["TokenConfigurations:Issuer"], _config["TokenConfigurations:Audience"], null,
        //        expires: DateTime.UtcNow.AddHours(2),
        //        signingCredentials: credentials);
        //    return new JwtSecurityTokenHandler().WriteToken(token);
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

        //[AllowAnonymous]
        //[HttpPost]
        //public IActionResult Login(string email, string password)
        //{
        //    IActionResult response = Unauthorized();
        //    var user_ = AuthenticateUser(user);
        //    if (user_ != null)
        //    {
        //        var token = GenerateToken(user_);
        //        response = Ok(new { token = token });
        //    }
        //    return response;
        //}

        //HttpClient client;

        //public ApiLoginController(IHttpClientFactory factory)
        //{
        //    client = factory.CreateClient();
        //}

        //public IActionResult Login()
        //{
        //    return View();
        //}


        //public IActionResult AddAcount()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public async Task<IActionResult> RegisterAccount(string email, string password)
        //{
        //    try
        //    {
        //        client.BaseAddress = new Uri("https://champscoreapi.azurewebsites.net/");
        //        client.DefaultRequestHeaders.Accept.Add(new
        //            MediaTypeWithQualityHeaderValue("application/json"));

        //        string json = JsonConvert.SerializeObject(Login);

        //        HttpContent content = new StringContent(json, Encoding.Unicode, "application/json");

        //        var response = await client.PostAsync("api/login", content);
        //        if (response.IsSuccessStatusCode)
        //        {
        //            return RedirectToAction("Login");
        //        }
        //        else
        //        {
        //            string error = $"{response.StatusCode} - {response.ReasonPhrase}";
        //            throw new Exception(error);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return View("_Erro", ex);
        //    }
        //}
    }
}
