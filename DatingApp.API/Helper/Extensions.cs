using System;
using Microsoft.AspNetCore.Http;

namespace DatingApp.API.Helper
{
    public static class Extensions
    {
        public static void ApplicationError(this HttpResponse response , string message) {
            response.Headers.Add("Application-Error",message);
            response.Headers.Add("Access-Control-Expose-Header","Application-Error");
            response.Headers.Add("Access-Control-Allow-Origin","*");
        }

        public static int CalculateAge(this DateTime thisDateTime)
        {
            var age =DateTime.Now.Year - thisDateTime.Year;
            if (thisDateTime.AddYears(age)> DateTime.Today)
            age--;

            return age;
        }
    }
}