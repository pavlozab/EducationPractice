using AutoMapper;
using Entities;
using WebFramework.CustomMapping;

namespace WebFramework.Api
{
    public class BaseDto<TDto, TEntity> : IHaveCustomMapping  // FIXME
        where TDto : class, new()
        where TEntity : BaseEntity, new()
    {
        public void CreateMappings(Profile profile)
        {
            throw new System.NotImplementedException();
        }
    }
}