using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Confab.Modules.Users.Core.DTO;
using Confab.Shared.Abstractions.Auth;

namespace Confab.Tests.EndToEnd.Common
{
    internal static class UsersExtensions
    {
        private const string Path = "users-module";

        internal static Requests Users(this HttpClient client) => new(client);

        internal class Requests
        {
            private readonly HttpClient _client;

            public Requests(HttpClient client)
            {
                _client = client;
            }

            public Task<HttpResponseMessage> SignUpAsync(string email = "user1@confab.io", string password = "secret",
                string role = "user", IEnumerable<string> permissions = null)
                => _client.PostAsJsonAsync($"{Path}/account/sign-up", new SignUpDto
                {
                    Email = email,
                    Password = password,
                    Role = role,
                    Claims = permissions is null
                        ? null
                        : new Dictionary<string, IEnumerable<string>>
                        {
                            ["claims"] = permissions
                        }
                });

            public async Task<JsonWebToken> SignInAsync(string email = "user1@confab.io", string password = "secret")
            {
                var response = await _client.PostAsJsonAsync($"{Path}/account/sign-in", new SignInDto
                {
                    Email = email,
                    Password = password
                });

                return response.IsSuccessStatusCode
                    ? await response.Content.ReadFromJsonAsync<JsonWebToken>()
                    : default;
            }
        }
    }
}