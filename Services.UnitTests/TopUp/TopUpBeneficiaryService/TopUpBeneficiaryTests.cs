using TopUpBeneficiary.Application.Services.TopUp.Handlers.Interface;
using TopUpBeneficiary.Application.SyncDataService.WebService.Client;
using TopUpBeneficiary.Domain.Persistence.Interfaces.Repository;
using TopUpBeneficiary.Domain.Persistence.Interfaces.Commons;
using TopUpBeneficiary.Domain.UserAggregate.ValueObjects;
using TopUpBeneficiary.Domain.BeneficiaryAggregate.ValueObjects;
using TopUpBeneficiary.Domain.TopUpOptionsAggregate.ValueObjects;
using TopUpBeneficiary.Application.Dtos.Request;
using TopUpBeneficiary.Domain.TopUpOptionsAggregate;
using TopUpBeneficiary.Domain.BeneficiaryAggregate;
using TopUpBeneficiary.Application.Services.TopUp.Handlers.Concrete;
using TopUpBeneficiary.Application.Services.TopUp;
using TopUpBeneficiaryService.UnitTests.Fixtures;
using Commons.Errors;
using TopUpBeneficiary.Application.SyncDataService.WebService.Response;
using TopUpBeneficiary.Domain.Commons.Enums;
using TopUpBeneficiary.Domain.TopUpTransactionAggregate;
using TopUpBeneficiary.Domain.UserAggregate;
using Microsoft.Extensions.Options;
using TopUpBeneficiary.Domain.Commons.Constants;

namespace TopUpBeneficiaryService.UnitTests.TopUp.TopUpBeneficiaryService
{
    public class TopUpBeneficiaryTests
    {
        private readonly Mock<IHandler> _handlerMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<ITopUpOptionsRepository> _topUpOptionsRepositoryMock;
        private readonly Mock<ITopUpTransactionRepository> _topUpTransactionRepositoryMock;
        private readonly Mock<IBeneficiaryRepository> _beneficiaryRepositoryMock;
        private readonly Mock<IAccountClient> _accountClientMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly TopUpService _topUpService;
        private readonly Mock<IOptions<AppConstants>> _appConstantsMock;
        private readonly AppConstants _appConstants;
        public TopUpBeneficiaryTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _topUpOptionsRepositoryMock = new Mock<ITopUpOptionsRepository>();
            _topUpTransactionRepositoryMock = new Mock<ITopUpTransactionRepository>();
            _beneficiaryRepositoryMock = new Mock<IBeneficiaryRepository>();
            _accountClientMock = new Mock<IAccountClient>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _handlerMock = new Mock<IHandler>();
            _appConstantsMock = new Mock<IOptions<AppConstants>>();
            _appConstants = new AppConstants
            {
                MonthlyLimit = 3000,
                MonthlyLimitPerBeneficiary_UnVerifiedUser = 1000,
                MonthlyLimitPerBeneficiary_VerifiedUser = 500,
                Charge = 1
            };

            _appConstantsMock.Setup(a => a.Value).Returns(_appConstants);
            _topUpService = new TopUpService(
                _userRepositoryMock.Object,
                _topUpOptionsRepositoryMock.Object,
                _topUpTransactionRepositoryMock.Object,
                _beneficiaryRepositoryMock.Object,
                _accountClientMock.Object,
                _unitOfWorkMock.Object,
                _mapperMock.Object,
                _appConstantsMock.Object
            );

            
            //build the chain
            var firstHandler = new CheckBeneficiaryStatusHandler();
            firstHandler.SetNext(new CheckTopUpLimitsHandler(_topUpTransactionRepositoryMock.Object, _appConstantsMock.Object))
                        .SetNext(new CheckUserBalanceHandler(_accountClientMock.Object))
                        .SetNext(new DebitUserBalanceHandler(_accountClientMock.Object));
           // _handlerMock = firstHandler;
        }

        [Fact]
        public async Task TopUpBeneficiary_ShouldReturnNotFound_WhenTopUpOptionDoesNotExist()
        {
            //Arrange
            _topUpOptionsRepositoryMock.Setup(repo => repo.GetById(TopUpBeneficiaryFixtures.TopUpDto.topUpOptionId)).ReturnsAsync((TopUpOption)null);

            //Act
            var result = await _topUpService.TopUpBeneficiary(TopUpBeneficiaryFixtures.TopUpDto);

            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(TopUpOptionsErrors.NotFoundById());
        }

        [Fact]  
        public async Task TopUpBeneficiary_ShouldReturnNtoFound_WhenUserDoesNotExist()
        {
            //Arrange
            _topUpOptionsRepositoryMock.Setup(repo => repo.GetById(TopUpOptionId.Create(TopUpBeneficiaryFixtures.TopUpDto.topUpOptionId))).ReturnsAsync((TopUpOptionsFixtures.TopUpOption));
            _userRepositoryMock.Setup(repo => repo.GetById(UserId.Create(TopUpBeneficiaryFixtures.TopUpDto.UserId))).ReturnsAsync((User)null);

            //Act
            var result = await _topUpService.TopUpBeneficiary(TopUpBeneficiaryFixtures.TopUpDto);

            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(UserErrors.NotFoundById());
        }

        [Fact]
        public async Task TopUpBeneficiary_ShouldReturnNtoFound_WhenBeneficiaryDoesNotExist()
        {
            //Arrange
            _topUpOptionsRepositoryMock.Setup(repo => repo.GetById(TopUpOptionId.Create(TopUpBeneficiaryFixtures.TopUpDto.topUpOptionId))).ReturnsAsync((TopUpOptionsFixtures.TopUpOption));
            _userRepositoryMock.Setup(repo => repo.GetById(UserId.Create(TopUpBeneficiaryFixtures.TopUpDto.UserId))).ReturnsAsync(UserFixtures.User);
            _beneficiaryRepositoryMock.Setup(repo => repo.GetById(BeneficiaryId.Create(TopUpBeneficiaryFixtures.TopUpDto.BeneficiaryId))).ReturnsAsync((Beneficiary)null);

            //Act
            var result = await _topUpService.TopUpBeneficiary(TopUpBeneficiaryFixtures.TopUpDto);

            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(BeneficiaryErrors.NotFoundById());
        }

        [Fact]
        public async Task TopUpBeneficiary_ShouldUpdateTransactionStatusToSuccess_WhenChainResultIsSuccess()
        {
            // Arrange
            _accountClientMock.Setup(client => client.GetBalance(It.IsAny<AccountId>())).ReturnsAsync(Result.Success(new GetBalanceResponse { Balance = 100 }));
            _accountClientMock.Setup(client => client.DebitBalance(It.IsAny<DebitBalanceDto>())).ReturnsAsync(Result.Success());
            _userRepositoryMock.Setup(repo => repo.GetById(It.IsAny<UserId>())).ReturnsAsync(UserFixtures.User);
            _beneficiaryRepositoryMock.Setup(repo => repo.GetById(It.IsAny<BeneficiaryId>())).ReturnsAsync(BeneficiaryFixtures.ActiveBeneficiary);
            _topUpOptionsRepositoryMock.Setup(repo => repo.GetById(It.IsAny<TopUpOptionId>())).ReturnsAsync(TopUpOptionsFixtures.TopUpOption);
            _topUpTransactionRepositoryMock.Setup(repo => repo.Add(It.IsAny<TopUpTransaction>())).Returns(TopUpBeneficiaryFixtures.TopUpTransaction);
            _unitOfWorkMock.Setup(uow => uow.Save()).Returns(Task.CompletedTask);
            // Mock the handler chain to return success
            _handlerMock.Setup(handler => handler.HandleAsync(UserFixtures.User, BeneficiaryFixtures.ActiveBeneficiary, TopUpOptionsFixtures.TopUpOption.Amount, _appConstants.Charge))
                        .ReturnsAsync(Result.Success());

            // Act
            var result = await _topUpService.TopUpBeneficiary(TopUpBeneficiaryFixtures.TopUpDto);


            // Assert
            result.IsSuccess.Should().BeTrue();
            _topUpTransactionRepositoryMock.Verify(repo => repo.Update(It.Is<TopUpTransaction>(t => t.Status == TopUpTransactionStatus.Success.ToString())), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.Save(), Times.Exactly(2));
            TopUpBeneficiaryFixtures.TopUpTransaction.Status.Should().Be(TopUpTransactionStatus.Success.ToString());
        }

        [Fact]
        public async Task TopUpBeneficiary_ShouldUpdateTransactionStatusToFailed_WhenChainResultIsFailed()
        {
            // Arrange
            _accountClientMock.Setup(client => client.GetBalance(It.IsAny<AccountId>())).ReturnsAsync(Result.Success(new GetBalanceResponse { Balance = 0 }));
            _accountClientMock.Setup(client => client.DebitBalance(It.IsAny<DebitBalanceDto>())).ReturnsAsync(Result.Success());
            _userRepositoryMock.Setup(repo => repo.GetById(It.IsAny<UserId>())).ReturnsAsync(UserFixtures.User);
            _beneficiaryRepositoryMock.Setup(repo => repo.GetById(It.IsAny<BeneficiaryId>())).ReturnsAsync(BeneficiaryFixtures.ActiveBeneficiary);
            _topUpOptionsRepositoryMock.Setup(repo => repo.GetById(It.IsAny<TopUpOptionId>())).ReturnsAsync(TopUpOptionsFixtures.TopUpOption);
            _topUpTransactionRepositoryMock.Setup(repo => repo.Add(It.IsAny<TopUpTransaction>())).Returns(TopUpBeneficiaryFixtures.TopUpTransaction);
            _unitOfWorkMock.Setup(uow => uow.Save()).Returns(Task.CompletedTask);
            // Mock the handler chain to return success
            _handlerMock.Setup(handler => handler.HandleAsync(UserFixtures.User, BeneficiaryFixtures.ActiveBeneficiary, TopUpOptionsFixtures.TopUpOption.Amount, _appConstants.Charge))
                        .ReturnsAsync(Result.Failure(UserErrors.NoEnoughBalance()));

            // Act
            var result = await _topUpService.TopUpBeneficiary(TopUpBeneficiaryFixtures.TopUpDto);

            // Assert
            result.IsFailure.Should().BeTrue();
            _topUpTransactionRepositoryMock.Verify(repo => repo.Update(It.Is<TopUpTransaction>(t => t.Status == TopUpTransactionStatus.Failed.ToString())), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.Save(), Times.Exactly(2));
            TopUpBeneficiaryFixtures.TopUpTransaction.Status.Should().Be(TopUpTransactionStatus.Failed.ToString());
        }
    }
}

