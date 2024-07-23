using TopUpBeneficiary.Application.Services.TopUp.Handlers.Concrete;
using TopUpBeneficiary.Domain.Persistence.Interfaces.Repository;
using Commons.Errors;
using TopUpBeneficiary.Application.Services.TopUp.Handlers.Base;
using TopUpBeneficiaryService.UnitTests.Fixtures;
using Microsoft.Extensions.Options;
using TopUpBeneficiary.Domain.Commons.Constants;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

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
        public async Task HandleAsync_ShouldReturnFailure_WhenUserIsVerifiedAndExceedsBeneficiaryLimit()
        {
            // Arrange
            var user = UserFixtures.User;
            var beneficiary = BeneficiaryFixtures.ActiveBeneficiary;
            int topUpAmount = 100;
            int charge = 1;

            _topUpTransactionRepositoryMock.Setup(repo => repo.SumTopUpsInCurrentMonthPerBeneficiary(user.Id, beneficiary.Id))
                .ReturnsAsync(_appConstants.MonthlyLimitPerBeneficiary_VerifiedUser);

            // Act
            var result = await _handler.HandleAsync(user, beneficiary, topUpAmount, charge);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(TopUpTransactionErrors.ExceedBeneficiaryLimit());
        }

        [Fact]
        public async Task HandleAsync_ShouldReturnFailure_WhenUserIsNotVerifiedAndExceedsBeneficiaryLimit()
        {
            // Arrange
            var user = UserFixtures.NotVerifiedUser;
            var beneficiary = BeneficiaryFixtures.ActiveBeneficiary;
            int topUpAmount = 100;
            int charge = 1;

            _topUpTransactionRepositoryMock.Setup(repo => repo.SumTopUpsInCurrentMonthPerBeneficiary(user.Id, beneficiary.Id))
                .ReturnsAsync(_appConstants.MonthlyLimitPerBeneficiary_UnVerifiedUser);

            // Act
            var result = await _handler.HandleAsync(user, beneficiary, topUpAmount, charge);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(TopUpTransactionErrors.ExceedBeneficiaryLimit());
        }

        [Fact]
        public async Task HandleAsync_ShouldReturnFailure_WhenExceedsMonthlyLimit()
        {
            // Arrange
            var user = UserFixtures.User;
            var beneficiary = BeneficiaryFixtures.ActiveBeneficiary;
            int topUpAmount = 100;
            int charge = 1;

            _topUpTransactionRepositoryMock.Setup(repo => repo.SumTopUpsInCurrentMonthForAllBeneficiaries(user.Id))
                .ReturnsAsync(_appConstants.MonthlyLimit);

            // Act
            var result = await _handler.HandleAsync(user, beneficiary, topUpAmount, charge);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(TopUpTransactionErrors.ExceedMonthlyLimit());
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
