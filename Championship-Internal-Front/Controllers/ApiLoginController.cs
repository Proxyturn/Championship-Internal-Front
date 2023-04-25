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

    }
}
