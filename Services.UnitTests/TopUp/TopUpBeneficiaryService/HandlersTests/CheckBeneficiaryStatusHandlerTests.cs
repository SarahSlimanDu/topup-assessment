using Commons.Errors;
using TopUpBeneficiary.Application.Services.TopUp.Handlers.Base;
using TopUpBeneficiary.Application.Services.TopUp.Handlers.Concrete;
using TopUpBeneficiaryService.UnitTests.Fixtures;

namespace TopUpBeneficiaryService.UnitTests.TopUp.TopUpBeneficiaryService.HandlersTests
{
    public class CheckBeneficiaryStatusHandlerTests
    {
        private readonly CheckBeneficiaryStatusHandler _handler;

        public CheckBeneficiaryStatusHandlerTests()
        {
            _handler = new CheckBeneficiaryStatusHandler();
        }

        [Fact]
        public async Task HandleAsync_ShouldReturnFailure_WhenBeneficiaryIsNotActive()
        {
            // Arrange
            var user = Fixtures.UserFixtures.User;
            var beneficiary = BeneficiaryFixtures.UnActiveBeneficiary;
            int topUpAmount = 100;
            int charge = 1;

            // Act
            var result = await _handler.HandleAsync(user, beneficiary, topUpAmount, charge);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(BeneficiaryErrors.NotActive());
        }
        [Fact]
        public async Task HandleAsync_ShouldCallNextHandler_WhenBeneficiaryIsActive()
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
