using TopUpBeneficiary.Application.Services.TopUp.Handlers.Concrete;
using TopUpBeneficiary.Domain.Persistence.Interfaces.Repository;
using Commons.Errors;
using TopUpBeneficiary.Application.Services.TopUp.Handlers.Base;
using TopUpBeneficiaryService.UnitTests.Fixtures;
using Microsoft.Extensions.Options;
using TopUpBeneficiary.Domain.Commons.Constants;

namespace TopUpBeneficiaryService.UnitTests.TopUp.TopUpBeneficiaryService.HandlersTests
{
    public class CheckTopUpLimitsHandlerTests
    {
        private readonly Mock<ITopUpTransactionRepository> _topUpTransactionRepositoryMock;
        private readonly CheckTopUpLimitsHandler _handler;
        private readonly Mock<IOptions<AppConstants>> _appConstantsMock;
        private readonly AppConstants _appConstants;

        public CheckTopUpLimitsHandlerTests()
        {
            _topUpTransactionRepositoryMock = new Mock<ITopUpTransactionRepository>();
            _appConstantsMock = new Mock<IOptions<AppConstants>>();

            _appConstants = new AppConstants
            {
                MonthlyLimit = 3000,
                MonthlyLimitPerBeneficiary_UnVerifiedUser = 1000,
                MonthlyLimitPerBeneficiary_VerifiedUser = 500,
                Charge = 1
            };
            _appConstantsMock.Setup(a => a.Value).Returns(_appConstants);
            _handler = new CheckTopUpLimitsHandler(_topUpTransactionRepositoryMock.Object, _appConstantsMock.Object);
            
        }
        [Fact]
        public async Task HandleAsync_ShouldReturnSuccess_WhenUserIsVerifiedAndSumOfTopUpAmountAndMonthlyTopUpsPerBeneficiaryWithinLimit()
        {
            // Arrange
            var user = UserFixtures.User; //Verified user
            var beneficiary = BeneficiaryFixtures.ActiveBeneficiary;
            int topUpAmount = 100;
            int charge = 1;
         
            _topUpTransactionRepositoryMock.Setup(repo => repo.SumTopUpsInCurrentMonthPerBeneficiary(user.Id, beneficiary.Id))
                .ReturnsAsync(400); // Within limit (100 + 400 = 500)

            var nextHandlerMock = new Mock<Handler>();
            nextHandlerMock.Setup(h => h.HandleAsync(user, beneficiary, topUpAmount, charge))
                .ReturnsAsync(Result.Success());
            _handler.SetNext(nextHandlerMock.Object);

            // Act
            var result = await _handler.HandleAsync(user, beneficiary, topUpAmount, charge);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task HandleAsync_ShouldReturnFailure_WhenUserIsVerifiedAndExceedsBeneficiaryLimit()
        {
            // Arrange
            var user = UserFixtures.User;
            var beneficiary = BeneficiaryFixtures.ActiveBeneficiary;
            int topUpAmount = 101; 
            int charge = 1;

            _topUpTransactionRepositoryMock.Setup(repo => repo.SumTopUpsInCurrentMonthPerBeneficiary(user.Id, beneficiary.Id))
                .ReturnsAsync(400); // 400 + 101 = 501

            // Act
            var result = await _handler.HandleAsync(user, beneficiary, topUpAmount, charge);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(TopUpTransactionErrors.ExceedBeneficiaryLimit());
        }

        [Fact]
        public async Task HandleAsync_ShouldReturnSuccess_WhenUserIsUnverifiedAndSumOfTopUpAmountAndMonthlyTopUpsPerBeneficiaryWithinLimit()
        {
            // Arrange
            var user = UserFixtures.NotVerifiedUser;
            var beneficiary = BeneficiaryFixtures.ActiveBeneficiary;
            int topUpAmount = 100;
            int charge = 1;

            _topUpTransactionRepositoryMock.Setup(repo => repo.SumTopUpsInCurrentMonthPerBeneficiary(user.Id, beneficiary.Id))
                .ReturnsAsync(900); //within limit 900 + 100 = 1000

            var nextHandlerMock = new Mock<Handler>();
            nextHandlerMock.Setup(h => h.HandleAsync(user, beneficiary, topUpAmount, charge))
                .ReturnsAsync(Result.Success());
            _handler.SetNext(nextHandlerMock.Object);

            // Act
            var result = await _handler.HandleAsync(user, beneficiary, topUpAmount, charge);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task HandleAsync_ShouldReturnFailure_WhenUserIsUnverifiedAndExceedsBeneficiaryLimit()
        {
            // Arrange
            var user = UserFixtures.NotVerifiedUser;
            var beneficiary = BeneficiaryFixtures.ActiveBeneficiary;
            int topUpAmount = 101; 
            int charge = 1;

            _topUpTransactionRepositoryMock.Setup(repo => repo.SumTopUpsInCurrentMonthPerBeneficiary(user.Id, beneficiary.Id))
                .ReturnsAsync(900); // 900 + 101 = 1001

            // Act
            var result = await _handler.HandleAsync(user, beneficiary, topUpAmount, charge);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(TopUpTransactionErrors.ExceedBeneficiaryLimit());
        }

        [Fact]
        public async Task HandleAsync_ShouldReturnSuccess_WhenSumOfTopUpAmountAndAllMonthlyTopUpsWithinLimit()
        {
            // Arrange
            var user = UserFixtures.User; // Can be verified or unverified
            var beneficiary = BeneficiaryFixtures.ActiveBeneficiary;
            int topUpAmount = 100;
            int charge = 1;

            _topUpTransactionRepositoryMock.Setup(repo => repo.SumTopUpsInCurrentMonthForAllBeneficiaries(user.Id))
                .ReturnsAsync(2900); // 100 + 2900 = 3000

            var nextHandlerMock = new Mock<Handler>();
            nextHandlerMock.Setup(h => h.HandleAsync(user, beneficiary, topUpAmount, charge))
                .ReturnsAsync(Result.Success());
            _handler.SetNext(nextHandlerMock.Object);
            // Act
            var result = await _handler.HandleAsync(user, beneficiary, topUpAmount, charge);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task HandleAsync_ShouldReturnFailure_WhenSumOfTopUpAmountAndAllMonthlyTopUpsExceedLimit()
        {
            // Arrange
            var user = UserFixtures.User; // Can be verified or unverified
            var beneficiary = BeneficiaryFixtures.ActiveBeneficiary;
            int topUpAmount = 101;
            int charge = 1;

            _topUpTransactionRepositoryMock.Setup(repo => repo.SumTopUpsInCurrentMonthForAllBeneficiaries(user.Id))
                .ReturnsAsync(2900); // Exceed overall monthly limit 101 + 2900 = 3001
      
            // Act
            var result = await _handler.HandleAsync(user, beneficiary, topUpAmount, charge);

            // Assert
            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public async Task HandleAsync_ShouldCallNextHandler_WhenLimitsAreNotExceeded()
        {
            // Arrange
            var user = UserFixtures.User;
            var beneficiary = BeneficiaryFixtures.ActiveBeneficiary;
            int topUpAmount = 100;
            int charge = 1;

            var nextHandlerMock = new Mock<Handler>();
            nextHandlerMock.Setup(h => h.HandleAsync(user, beneficiary, topUpAmount, charge))
                .ReturnsAsync(Result.Success());
            _handler.SetNext(nextHandlerMock.Object);

            // Act
            var result = await _handler.HandleAsync(user, beneficiary, topUpAmount, charge);

            // Assert
            result.IsSuccess.Should().BeTrue();
            nextHandlerMock.Verify(h => h.HandleAsync(user, beneficiary, topUpAmount, charge), Times.Once);
        }
    }
}
