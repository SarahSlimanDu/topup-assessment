using Mapster;
using TopUpBeneficiary.Application.Dtos.Response;
using TopUpBeneficiary.Domain.TopUpOptionsAggregate;

namespace TopUpBeneficiary.Api.Commons.MappingConfig
{
    public class TopUpMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<TopUpOption, TopUpOptionsDto>().Map(dest => dest.Id, src => src.Id.Value);
        }
    }
}
