using AutoMapper;
using QPD.AddressStandardizer.Services;

namespace QPD.AddressStandardizer.Controllers.Models
{
    public class AddressCleanProfile : Profile
    {
        public AddressCleanProfile()
        {
            CreateMap<AddressCleanRequest, AddressModel>();
        }
    }
}
