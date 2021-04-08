namespace ProductRest.Responses
{
    public class TokenResponse
    {
        public string Email { get; set; }
        public string Access { get; set; }

        public TokenResponse(string email, string access)
        {
            Email = email;
            Access = access;
        }
    }
}