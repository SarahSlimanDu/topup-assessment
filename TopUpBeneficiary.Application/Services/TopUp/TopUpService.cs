using Commons.Errors;
using MapsterMapper;
using Microsoft.Extensions.Options;
using TopUpBeneficiary.Application.Dtos.Request;
using TopUpBeneficiary.Application.Dtos.Response;
using TopUpBeneficiary.Application.Services.TopUp.Handlers.Concrete;
using TopUpBeneficiary.Application.Services.TopUp.Handlers.Interface;
using TopUpBeneficiary.Application.SyncDataService.WebService.Client;
using TopUpBeneficiary.Domain.BeneficiaryAggregate.ValueObjects;
using TopUpBeneficiary.Domain.Commons.Constants;
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
        private readonly IAccountClient _accountClient;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly AppConstants _appConstants;

        public TopUpService(IUserRepository userRepository,
                            ITopUpOptionsRepository topUpOptionsRepository,
                            ITopUpTransactionRepository topUpTransactionRepository,
                            IBeneficiaryRepository beneficiaryRepository,
                            IAccountClient accountClient,
                            IUnitOfWork unitOfWork,
                            IMapper mapper,
                            IOptions<AppConstants> constants)
        {
            _userRepository = userRepository;
            _topUpOptionsRepository = topUpOptionsRepository;
            _topUpTransactionRepository = topUpTransactionRepository;
            _beneficiaryRepository = beneficiaryRepository;
            _accountClient = accountClient;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _appConstants = constants.Value;

            //build the chain
            var firstHandler = new CheckBeneficiaryStatusHandler();
            firstHandler.SetNext(new CheckTopUpLimitsHandler(_topUpTransactionRepository, constants))
                        .SetNext(new CheckUserBalanceHandler(_accountClient))
                        .SetNext(new DebitUserBalanceHandler(_accountClient));
            _handler = firstHandler;
        }

        public async Task<Result> TopUpBeneficiary(TopUpDto topUpRequest)
        {
            var topUpOption = await _topUpOptionsRepository.GetById(TopUpOptionId.Create(topUpRequest.topUpOptionId));
            if (topUpOption is null)
                return Result.Failure(TopUpOptionsErrors.NotFoundById());

            var user = await _userRepository.GetById(UserId.Create(topUpRequest.UserId));
            if (user is null)
                return Result.Failure(UserErrors.NotFoundById());

            var beneficiary = await _beneficiaryRepository.GetById(BeneficiaryId.Create(topUpRequest.BeneficiaryId));
            if (beneficiary is null)
                return Result.Failure(BeneficiaryErrors.NotFoundById());

            var topUpTransaction = _topUpTransactionRepository
                                  .Add(TopUpTransaction
                                  .Create(user.Id, beneficiary.Id, topUpOption.Id, _appConstants.Charge, TopUpTransactionStatus.Pending.ToString()));
            await _unitOfWork.Save();


            var chainResult = await _handler.HandleAsync(user,
                                             beneficiary,
                                             topUpOption.Amount,
                                             _appConstants.Charge);

            if (chainResult.IsSuccess)
            {
               topUpTransaction.UpdateStatus(TopUpTransactionStatus.Success.ToString());    
            }
            else
            {
                topUpTransaction.UpdateStatus(TopUpTransactionStatus.Failed.ToString());
            }
            _topUpTransactionRepository.Update(topUpTransaction);
            await _unitOfWork.Save(); 

            return chainResult;
        }

        public async Task<Result<IList<TopUpOptionsDto>>> GetTopUpOptions()
        {
           var topUpOptions =  await _topUpOptionsRepository.GetAll();
            if(topUpOptions.Count() == 0)
            {
                return Result.Failure<IList<TopUpOptionsDto>>(TopUpOptionsErrors.NoOptionsFound());
            }

           var result =  _mapper.Map<IList<TopUpOptionsDto>>(topUpOptions.ToList());
            return Result.Success(result);
        }


    }
}
