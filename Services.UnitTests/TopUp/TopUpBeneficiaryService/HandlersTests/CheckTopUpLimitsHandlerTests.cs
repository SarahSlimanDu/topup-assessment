using TopUpBeneficiary.Application.Services.TopUp.Handlers.Concrete;
using TopUpBeneficiary.Domain.Persistence.Interfaces.Repository;
using Commons.Errors;
using TopUpBeneficiary.Application.Services.TopUp.Handlers.Base;
using TopUpBeneficiaryService.UnitTests.Fixtures;

namespace TopUpBeneficiaryService.UnitTests.TopUp.TopUpBeneficiaryService.HandlersTests
{
    public class CheckTopUpLimitsHandlerTests
    {
        private readonly Mock<ITopUpTransactionRepository> _topUpTransactionRepositoryMock;
        private readonly CheckTopUpLimitsHandler _handler;

        public CheckTopUpLimitsHandlerTests()
        {
            _topUpTransactionRepositoryMock = new Mock<ITopUpTransactionRepository>();
            _handler = new CheckTopUpLimitsHandler(_topUpTransactionRepositoryMock.Object);
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
                .ReturnsAsync(500);

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
                .ReturnsAsync(1000);

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
                .ReturnsAsync(3000);

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
