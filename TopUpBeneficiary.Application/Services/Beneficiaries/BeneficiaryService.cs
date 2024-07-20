using MapsterMapper;
using TopUpBeneficiary.Application.Dtos.Response;
using TopUpBeneficiary.Domain.Persistence.Interfaces.Commons;
using TopUpBeneficiary.Domain.Persistence.Interfaces.Repository;
using TopUpBeneficiary.Domain.UserAggregate.ValueObjects;

namespace TopUpBeneficiary.Application.Services.Beneficiaries
{
    public class BeneficiaryService(IBeneficiaryRepository beneficiaryRepository, IUnitOfWork unitOfWork, IMapper mapper) : IBeneficiaryService
    {
        private readonly IBeneficiaryRepository _beneficiaryRepository = beneficiaryRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public Task AddBeneficiary()
        {
            throw new NotImplementedException();
        }

        public async Task<IList<BeneficiaryDto>?> GetBeneficiaries(Guid userId)
        {
            //TODO : we should make sure if the user exist.//get user by Id
            var beneficiaries = await _beneficiaryRepository.GetByUserId(UserId.Create(userId));

            //map to the suitable response
            return _mapper.Map<IList<BeneficiaryDto>>(beneficiaries.ToList());




        }
    }
}
