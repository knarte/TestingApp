using System.Threading.Tasks;
using BeSafe.Core.Models;
using BeSafe.Core.Models.Internal.Requests;
using BeSafe.Core.Models.Internal.Responses;

namespace BeSafe.Core.Interfaces
{
    public interface IApiService
    {
        Task<Response> APIRequestPOST<T>(string urlBase, string prefix, string controller, object data, bool IsList);
        Task<Response> DeleteAsync(string urlBase, string servicePrefix, string controller, int id, string tokenType, string accessToken);
        Task<Response> GetListAsync<T>(string urlBase, string servicePrefix, string controller);
        Task<Response> GetListAsync<T>(string urlBase, string servicePrefix, string controller, string tokenType, string accessToken);
        Task<Response> GetTokenAsync(string urlBase, string servicePrefix, string controller, TokenRequest request);
        Task<Response> PostAsync<T>(string urlBase, string servicePrefix, string controller, T model, string tokenType, string accessToken);
        Task<Response> PutAsync<T>(string urlBase, string servicePrefix, string controller, int id, T model, string tokenType, string accessToken);
        Task<Response> PutAsync<T>(string urlBase, string servicePrefix, string controller, T model, string tokenType, string accessToken);
    }
}