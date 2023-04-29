using Microsoft.AspNetCore.Mvc;



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
