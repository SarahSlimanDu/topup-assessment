

using System.Collections.Generic;
using System.Diagnostics;
using TopUpBeneficiary.Application.Dtos.Request;
using TopUpBeneficiary.Application.Services.TopUp.ChainOfResponsibilities;
using TopUpBeneficiary.Domain.Persistence.Interfaces.Repository;
using TopUpBeneficiary.Domain.UserAggregate;
using TopUpBeneficiary.Domain.UserAggregate.ValueObjects;

namespace TopUpBeneficiary.Application.Services.TopUp
{
    public class TopUpService : ITopUpService
    {
        private readonly IApplyTopUp _applyTopUp;
        private readonly IUserRepository _userRepository;
        private readonly ITopUpOptionsRepository _topUpOptionsRepository;
        private readonly ITopUpTransactionRepository _topUpTransactionRepository;
        public TopUpService(IApplyTopUp applyTopUp, 
                            IUserRepository userRepository,
                            ITopUpOptionsRepository topUpOptionsRepository,
                            ITopUpTransactionRepository topUpTransactionRepository)
        {
            _applyTopUp = applyTopUp;   
            _userRepository = userRepository;
            _topUpOptionsRepository = topUpOptionsRepository;  
            _topUpTransactionRepository = topUpTransactionRepository;   
        }
        public Task GetTopUpOptions()
        {
            throw new NotImplementedException();
        }

        public async Task TopUpBeneficiary(TopUpRequest topUpRequest)
        {
            var topUpOption = await _topUpOptionsRepository.GetById(topUpRequest.topUpOptionId);
            _applyTopUp.SetNext(new BeneficiaryStatus())
                       .SetNext(new CheckTopUpLimits(_topUpTransactionRepository))
                       .SetNext(new CheckUserBalance())
                       .SetNext(new DebitUserAccount());
            //Check Beneficiary Existence and Status (Exist and active)
            //Check Verification and Top-Up Limits for Beneficiary (user verified or not and related limits)
            //Check Monthly Top - Up Limit for All Beneficiaries(monthly limit)
            //Check User Balance 
            //Debit User Account
            throw new NotImplementedException();
        }
    }
}
