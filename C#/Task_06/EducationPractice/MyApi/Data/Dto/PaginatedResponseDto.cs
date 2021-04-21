using System.Collections.Generic;

namespace Data.Dto
{
    public class PaginatedResponseDto<TEntity>
    {
        public MetaDto Meta { get; set; }
        public IEnumerable<TEntity> Items { get; set; }
    }
}