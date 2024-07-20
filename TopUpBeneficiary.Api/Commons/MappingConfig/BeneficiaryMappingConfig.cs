using Mapster;
using TopUpBeneficiary.Application.Dtos.Response;
using TopUpBeneficiary.Domain.BeneficiaryAggregate;

namespace TopUpBeneficiary.Api.Commons.MappingConfig
{
    public class BeneficiaryMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Beneficiary, BeneficiaryDto>()
                .Map(dest => dest.BeneficiaryId, src => src.Id.Value);
        }
    }
}
