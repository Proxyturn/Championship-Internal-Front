using Azure.Core;
using Championship_Internal_Front.Controllers;
using Championship_Internal_Front.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using NuGet.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Championship_Internal_Front.Services
{
    public class UserServices
    {

        private string _token;

        public UserServices(string token)
        {
            _token = token;
        }

        public UserServices() { }
        

        public HttpClient ApiConnection(HttpClient client)
        {
            client.BaseAddress = new Uri("https://champscoreapi.azurewebsites.net/");
            client.DefaultRequestHeaders.Accept.Add(new
            MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }

        
        //public User GetByUserId(Guid userId)
        //{


        //    ApiUserController userController = new();

        //    return  user;
        //}

        //public UserServices()
        //{
        //    AuthController auth = new();
        //    
        //}


        //public UserServices()
        //{ 
        //    _configuration = configuration;
        //    client = factory.CreateClient();
        //}

        //public void EndpointRequest(string method)
        //{
        //    if (Request.Cookies["AuthToken"] != null)
        //    {
        //        token = Request.Cookies["AuthToken"];
        //    }
        //    else
        //    {
        //        return RedirectToAction("login", "ApiLogin");
        //    }

        //    client.BaseAddress = new Uri("https://champscoreapi.azurewebsites.net/");
        //    client.DefaultRequestHeaders.Accept.Add(new
        //    MediaTypeWithQualityHeaderValue("application/json"));
        //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


        //    JwtSecurityTokenHandler tokenHandler = new();
        //    var decodedToken = tokenHandler.ReadJwtToken(token);
        //    var claims = decodedToken.Claims;
        //    string id = (claims.First(claim => claim.Type == "userId").Value);

        //    HttpResponseMessage response = await client.GetAsync($"api/user/{userId}");
        //}
    }
}
