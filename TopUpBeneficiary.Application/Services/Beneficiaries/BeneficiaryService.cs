using Commons.Errors;
using MapsterMapper;
using TopUpBeneficiary.Application.Dtos.Response;
using TopUpBeneficiary.Domain.Errors;
using TopUpBeneficiary.Domain.Persistence.Interfaces.Commons;
using TopUpBeneficiary.Domain.Persistence.Interfaces.Repository;
using TopUpBeneficiary.Domain.UserAggregate.ValueObjects;

namespace TopUpBeneficiary.Application.Services.Beneficiaries
{
    public class BeneficiaryService(IBeneficiaryRepository beneficiaryRepository,
                                    IUserRepository userRepository, 
                                    IUnitOfWork unitOfWork, 
                                    IMapper mapper) : IBeneficiaryService
    {
        private readonly IBeneficiaryRepository _beneficiaryRepository = beneficiaryRepository;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public Task AddBeneficiary()
        {
            throw new NotImplementedException();
        }

        public async Task<Result<IList<BeneficiaryDto>?>> GetBeneficiaries(Guid userId)
        {
            var user = await _userRepository.GetById(UserId.Create(userId));
            if(user is null)
            {
                return Result.Failure<IList<BeneficiaryDto>?>(UserErrors.NotFoundById());
            }

            var beneficiaries = await _beneficiaryRepository.GetByUserId(UserId.Create(userId));

            var userBeneficiaries = _mapper.Map<IList<BeneficiaryDto>?>(beneficiaries.ToList());

            return Result.Success(userBeneficiaries);
        }
    }
}
