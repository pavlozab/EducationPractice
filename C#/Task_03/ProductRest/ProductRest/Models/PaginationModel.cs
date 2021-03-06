namespace ProductRest.Models
{
    public class PaginationModel
    {
        public int Offset { get; set; }
        public int Limit { get; set; }

        public PaginationModel()
        {
            Offset = 1;
            Limit = 10;
        }
        
        public PaginationModel(int offset, int limit)
        {
            Offset = offset < 1 ? 1 : offset;
            Limit = limit > 10 ? 10 : limit;
        }
    }
}