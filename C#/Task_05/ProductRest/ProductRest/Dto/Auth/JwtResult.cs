using System.Text.Json.Serialization;

namespace ProductRest.Dto.Auth
{
    public class JwtResult
    {
        [JsonPropertyName("Email")]
        public string Email { get; set; }
        
        [JsonPropertyName("AccessToken")]
        public string AccessToken { get; set; }
    }
}