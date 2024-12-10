using School.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace School.Services
{
    public interface IApiService
    {
        Task<Response> GetTokenAsync(string urlBase, string controller, TokenRequest request);

        Task<Response> GetSingleResultAsync<T>(string urlBase, string servicePrefix, string controller, string tokenType, string accessToken);

        Task<Response> GetMultipleResultsAsync<T>(string urlBase, string servicePrefix, string controller, string tokenType, string accessToken);

        Task<Response> GetSingleResultAsync<T>(string urlBase, string servicePrefix, string controller);

        Task<Response> GetMultipleResultsAsync<T>(string urlBase, string servicePrefix, string controller);

        Task<Response> PostCitiesAsync<T>(string urlBase, string servicePrefix, string controller, CityRequest cityRequest);
    }
}
