using AutoMapper;

namespace Cofi;

sealed class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ValidationFailed, ValidationFailedResponse>();
        CreateMap<ValidationFailed.FailureObj, ValidationFailedResponse.FailureObj>();
    }
}