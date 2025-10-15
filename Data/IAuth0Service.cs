using Auth0.ManagementApi;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace QuizManager.Data
{
    public interface IAuth0Service
    {
        Task<Auth0UserDetails> GetUserDetailsAsync(string name, string userId = null);
        Task<Auth0UserDetails> GetUserDetailsByEmailAsync(string name);
        Task<string> GetUserBrowserInfoAsync(string userId);
    }

    public class Auth0Service : IAuth0Service
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public Auth0Service(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<Auth0UserDetails> GetUserDetailsAsync(string name, string userId = null)
        {
            try
            {
                var token = await GetManagementApiTokenAsync();
                Console.WriteLine($"[GetUserDetailsAsync] Token acquired: {(string.IsNullOrEmpty(token) ? "NO TOKEN" : "TOKEN OK")}");

                var managementDomain = _configuration["Auth0:Management:Domain"];
                var httpClient = _httpClientFactory.CreateClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                Console.WriteLine($"Attempting to find user with email: {name} or ID: {userId}");

                // Helper function to properly escape any user ID format
                string EscapeUserId(string id)
                {
                    // First URL encode the entire string
                    var encoded = Uri.EscapeDataString(id);
                    // Then ensure the pipe character is double-encoded if present
                    return encoded.Replace("|", "%7C");
                }

                if (!string.IsNullOrEmpty(userId))
                {
                    try
                    {
                        // Get user details
                        var userUrl = $"https://{managementDomain}/api/v2/users/{EscapeUserId(userId)}";
                        var userResponse = await httpClient.GetAsync(userUrl);

                        if (!userResponse.IsSuccessStatusCode)
                        {
                            Console.WriteLine($"User with ID {userId} not found, falling back to email lookup.");
                            throw new Exception("User not found");
                        }

                        var user = await userResponse.Content.ReadFromJsonAsync<Auth0User>();
                        Console.WriteLine($"Found user by ID: {userId}");

                        // Get logs with properly escaped user ID
                        var logsUrl = $"https://{managementDomain}/api/v2/users/{EscapeUserId(userId)}/logs?per_page=1";
                        var logsResponse = await httpClient.GetAsync(logsUrl);

                        Auth0LogEntry mostRecentLog = null;
                        if (logsResponse.IsSuccessStatusCode)
                        {
                            var logs = await logsResponse.Content.ReadFromJsonAsync<List<Auth0LogEntry>>();
                            mostRecentLog = logs?.FirstOrDefault();
                        }

                        return new Auth0UserDetails
                        {
                            CreatedAt = user.CreatedAt,
                            LastLogin = user.LastLogin,
                            LoginTimes = user.LoginsCount?.ToString(),
                            LastIp = mostRecentLog?.Ip ?? user.LastIpAddress,
                            IsEmailVerified = user.EmailVerified,
                            LoginBrowser = mostRecentLog?.UserAgent ?? "Unknown",
                            IsMobile = mostRecentLog?.IsMobile ?? false,
                            LocationInfo = mostRecentLog?.LocationInfo
                        };
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error getting user by ID {userId}: {ex.Message}");
                        // Fall through to email lookup
                    }
                }

                try
                {
                    var users = await httpClient.GetFromJsonAsync<List<Auth0User>>(
                        $"https://{managementDomain}/api/v2/users-by-email?email={Uri.EscapeDataString(name)}");

                    if (users == null || !users.Any())
                    {
                        Console.WriteLine($"No users found with email: {name}");
                        return null;
                    }

                    var selectedUser = users.OrderByDescending(u => u.LastLogin ?? u.CreatedAt).First();
                    var selectedUserId = selectedUser.UserId;

                    // Get logs with properly escaped user ID
                    var logsUrl = $"https://{managementDomain}/api/v2/users/{EscapeUserId(selectedUserId)}/logs?per_page=1";
                    var logsResponse = await httpClient.GetAsync(logsUrl);

                    Auth0LogEntry mostRecentLog = null;
                    if (logsResponse.IsSuccessStatusCode)
                    {
                        var logs = await logsResponse.Content.ReadFromJsonAsync<List<Auth0LogEntry>>();
                        mostRecentLog = logs?.FirstOrDefault();
                    }

                    return new Auth0UserDetails
                    {
                        CreatedAt = selectedUser.CreatedAt,
                        LastLogin = selectedUser.LastLogin,
                        LoginTimes = selectedUser.LoginsCount?.ToString(),
                        LastIp = mostRecentLog?.Ip ?? selectedUser.LastIpAddress,
                        IsEmailVerified = selectedUser.EmailVerified,
                        LoginBrowser = mostRecentLog?.UserAgent ?? "Unknown",
                        IsMobile = mostRecentLog?.IsMobile ?? false,
                        LocationInfo = mostRecentLog?.LocationInfo
                    };
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error searching by email {name}: {ex}");
                    throw;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetUserDetailsAsync for email {name}: {ex.Message}");
                return null;
            }
        }

        public async Task<string> GetUserBrowserInfoAsync(string userId)
        {
            try
            {
                var token = await GetManagementApiTokenAsync();
                var managementDomain = _configuration["Auth0:Management:Domain"];

                var httpClient = _httpClientFactory.CreateClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Helper function to properly escape any user ID format
                string EscapeUserId(string id)
                {
                    var encoded = Uri.EscapeDataString(id);
                    return encoded.Replace("|", "%7C");
                }

                var url = $"https://{managementDomain}/api/v2/users/{EscapeUserId(userId)}/logs?per_page=1";

                var response = await httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Auth0 API Error: {response.StatusCode} - {errorContent}");
                    return "Unknown";
                }

                var logs = await response.Content.ReadFromJsonAsync<List<Auth0LogEntry>>(new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                var mostRecentLog = logs?.FirstOrDefault();

                if (mostRecentLog == null) return "Unknown";

                return mostRecentLog.UserAgent ?? "Unknown";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting browser info for user {userId}: {ex.Message}");
                return "Unknown";
            }
        }

        public async Task<Auth0UserDetails> GetUserDetailsByEmailAsync(string name)
        {
            try
            {
                var token = await GetManagementApiTokenAsync();
                Console.WriteLine($"[GetUserDetailsByEmailAsync] Token acquired: {(string.IsNullOrEmpty(token) ? "NO TOKEN" : "TOKEN OK")}");

                var managementDomain = _configuration["Auth0:Management:Domain"];
                var httpClient = _httpClientFactory.CreateClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var users = await httpClient.GetFromJsonAsync<List<Auth0User>>(
                    $"https://{managementDomain}/api/v2/users-by-email?email={Uri.EscapeDataString(name)}");

                var user = users?.FirstOrDefault();

                if (user == null)
                {
                    Console.WriteLine($"No user found with email: {name}");
                    return null;
                }

                var browserInfo = await GetUserBrowserInfoAsync(user.UserId);

                return new Auth0UserDetails
                {
                    CreatedAt = user.CreatedAt,
                    LastLogin = user.LastLogin,
                    LoginTimes = user.LoginsCount?.ToString(),
                    LastIp = user.LastIpAddress,
                    IsEmailVerified = user.EmailVerified,
                    LoginBrowser = browserInfo
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetUserDetailsByEmailAsync: {ex}");
                return null;
            }
        }

        private async Task<string> GetManagementApiTokenAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var managementDomain = _configuration["Auth0:Management:Domain"];

            Console.WriteLine($"Requesting token from: https://{managementDomain}/oauth/token");
            Console.WriteLine($"Using client ID: {_configuration["Auth0:Management:ClientId"]}");

            var request = new HttpRequestMessage(HttpMethod.Post, $"https://{managementDomain}/oauth/token")
            {
                Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    ["grant_type"] = "client_credentials",
                    ["client_id"] = _configuration["Auth0:Management:ClientId"],
                    ["client_secret"] = _configuration["Auth0:Management:ClientSecret"],
                    ["audience"] = $"https://{managementDomain}/api/v2/",
                    ["scope"] = "read:users read:user_idp_tokens read:logs"
                })
            };

            var response = await client.SendAsync(request);
            var responseBody = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"Token response status: {response.StatusCode}");
            Console.WriteLine($"Token response body: {responseBody}");

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Token request failed: {response.StatusCode} | {responseBody}");

            var tokenResponse = JsonSerializer.Deserialize<Auth0TokenResponse>(responseBody);
            Console.WriteLine($"Access Token (first 20 chars): {tokenResponse?.AccessToken?.Substring(0, 20)}...");

            return tokenResponse?.AccessToken;
        }
    }

    public class Auth0UserDetails
    {
        public DateTime? CreatedAt { get; set; }
        public DateTime? LastLogin { get; set; }
        public string LoginTimes { get; set; }
        public string LastIp { get; set; }
        public bool? IsEmailVerified { get; set; }
        public string LoginBrowser { get; set; }
        public bool IsMobile { get; set; }
        public Models.LocationInfo LocationInfo { get; set; }
    }

    public class Auth0TokenResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }
    }

    public class Auth0LogEntry
    {
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("user_agent")]
        public string UserAgent { get; set; }

        [JsonPropertyName("ip")]
        public string Ip { get; set; }

        [JsonPropertyName("isMobile")]
        public bool IsMobile { get; set; }

        [JsonPropertyName("location_info")]
        public Models.LocationInfo LocationInfo { get; set; } 

    }

    public class Auth0User
    {
        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt { get; set; }

        [JsonPropertyName("last_login")]
        public DateTime? LastLogin { get; set; }

        [JsonPropertyName("logins_count")]
        public int? LoginsCount { get; set; }

        [JsonPropertyName("last_ip")]
        public string LastIpAddress { get; set; }

        [JsonPropertyName("email_verified")]
        public bool? EmailVerified { get; set; }

        [JsonPropertyName("user_id")]
        public string UserId { get; set; }
    }
}