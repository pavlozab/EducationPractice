namespace ProductRest.Responses
{
    public class ErrorResponse
    {
        public bool Ok { get; set; }
        public int Status { get; set; }
        public string Title { get; set; }

        public ErrorResponse(int status, string title)
        {
            Ok = false;
            Status = status;
            Title = title;
        }
    }
}