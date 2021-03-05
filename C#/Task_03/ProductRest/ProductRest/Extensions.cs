using ProductRest.Dtos;

namespace ProductRest
{
    public static class Extensions
    {
        public static ProductDto AsDto(this ProductDto item)
        {
            return new ProductDto
            {
                Id = item.Id,
                AddressLine = item.AddressLine,
                PostalCode = item.PostalCode,
                Country = item.Country,
                City = item.City,
                FaxNumber = item.FaxNumber,
                PhoneNumber = item.PhoneNumber
            };
        }
    }
}