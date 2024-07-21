using Commons.Errors;
using MapsterMapper;
using TopUpBeneficiary.Application.Dtos.Request;
using TopUpBeneficiary.Application.Dtos.Response;
using TopUpBeneficiary.Application.Services.TopUp.Handlers.Concrete;
using TopUpBeneficiary.Application.Services.TopUp.Handlers.Interface;
using TopUpBeneficiary.Application.SyncDataService.WebService.Client;
using TopUpBeneficiary.Domain.BeneficiaryAggregate.ValueObjects;
using TopUpBeneficiary.Domain.Commons.Enums;
using TopUpBeneficiary.Domain.Errors;
using TopUpBeneficiary.Domain.Persistence.Interfaces.Commons;
using TopUpBeneficiary.Domain.Persistence.Interfaces.Repository;
using TopUpBeneficiary.Domain.TopUpOptionsAggregate.ValueObjects;
using TopUpBeneficiary.Domain.TopUpTransactionAggregate;
using TopUpBeneficiary.Domain.UserAggregate.ValueObjects;

namespace TopUpBeneficiary.Application.Services.TopUp
{
    public class TopUpService : ITopUpService
    {
        private readonly IHandler _handler;
        private readonly IUserRepository _userRepository;
        private readonly ITopUpOptionsRepository _topUpOptionsRepository;
        private readonly ITopUpTransactionRepository _topUpTransactionRepository;
        private readonly IBeneficiaryRepository _beneficiaryRepository;
        private readonly IAccountExternalService _accountExternalService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public TopUpService(IUserRepository userRepository,
                            ITopUpOptionsRepository topUpOptionsRepository,
                            ITopUpTransactionRepository topUpTransactionRepository,
                            IBeneficiaryRepository beneficiaryRepository,
                            IAccountExternalService accountExternalService,
                            IUnitOfWork unitOfWork,
                            IMapper mapper)
        {
            _userRepository = userRepository;
            _topUpOptionsRepository = topUpOptionsRepository;
            _topUpTransactionRepository = topUpTransactionRepository;
            _beneficiaryRepository = beneficiaryRepository;
            _accountExternalService = accountExternalService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;

            //build the chain
            var firstHandler = new CheckBeneficiaryStatusHandler();
            firstHandler.SetNext(new CheckTopUpLimitsHandler(_topUpTransactionRepository))
                        .SetNext(new CheckUserBalanceHandler(_accountExternalService))
                        .SetNext(new DebitUserBalanceHandler());
            _handler = firstHandler;
        }

        public async Task<Result> TopUpBeneficiary(TopUpDto topUpRequest)
        {
            //test
            var topUpOption = await _topUpOptionsRepository.GetById(TopUpOptionId.Create(topUpRequest.topUpOptionId));
            if (topUpOption is null)
                return Result.Failure(TopUpOptionsErrors.NotFoundById());

            //test
            var user = await _userRepository.GetById(UserId.Create(topUpRequest.UserId));
            if (user is null)
                return Result.Failure(UserErrors.NotFoundById());

            //test
            var beneficiary = await _beneficiaryRepository.GetById(BeneficiaryId.Create(topUpRequest.BeneficiaryId));
            if (beneficiary is null)
                return Result.Failure(BeneficiaryErrors.NotFoundById());

            //insert row in top up transaction with initial status
            _topUpTransactionRepository.Add(TopUpTransaction.Create(user.Id, beneficiary.Id, topUpOption.Id, 1, TopUpTransactionStatus.Pending.ToString()));
            await _unitOfWork.Save();


            var chainResult = await _handler.HandleAsync(user,
                                             beneficiary,
                                             topUpOption.Amount);

            if (chainResult.IsSuccess)
            {
                //TODO:update the transaction to success
            }
            else
            {
                //TODO:update the transaction to failed
            }

            return chainResult;
        }

        public async Task<Result<IList<TopUpOptionsDto>>> GetTopUpOptions()
        {
           var topUpOptions =  await _topUpOptionsRepository.GetAll();
           var result =  _mapper.Map<IList<TopUpOptionsDto>>(topUpOptions.ToList());
            return Result.Success(result);
        }
    }
}
