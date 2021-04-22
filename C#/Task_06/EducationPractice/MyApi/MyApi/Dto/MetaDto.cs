namespace Data.Dto
{
    public class MetaDto: QueryMetaDto
    {
        public long Count { get; set; }

        public MetaDto(QueryMetaDto query, long count) : base(query)
        {
            Count = count;
        }
    }
}