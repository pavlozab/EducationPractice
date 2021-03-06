namespace ProductRest.Models
{
    public class QueryParametersModel
    {
        public string SortBy { get; set; }
        public string SortType { get; set; }
        public int Offset { get; set; }
        public int Limit { get; set; }

        public QueryParametersModel()
        {
            Offset = 1;
            Limit = 10;
        }
        
        public QueryParametersModel(string sortBy, string sortType,  int offset, int limit)
        {
            if (sortBy is null)
            {
                SortBy = "addressLine";
                SortType = "1";
            }
            SortBy = sortBy;
            SortType = sortType;
            Offset = offset < 1 ? 1 : offset;
            Limit = limit > 10 ? 10 : limit;
        }
    }
}