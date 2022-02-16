using TietoEvryUserList.Models;


namespace TietoEvryUserList.Services
{
    public class UserHttpClient
    {
        private readonly HttpClient _httpClient;


        public UserHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<IEnumerable<User>?> GetAll() => await _httpClient.GetFromJsonAsync<IEnumerable<User>>("/users");
    }
}
