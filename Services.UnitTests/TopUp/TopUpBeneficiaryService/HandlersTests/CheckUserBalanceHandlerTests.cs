using Commons.Errors;
using TopUpBeneficiary.Application.Services.TopUp.Handlers.Base;
using TopUpBeneficiary.Application.Services.TopUp.Handlers.Concrete;
using TopUpBeneficiary.Application.SyncDataService.WebService.Client;
using TopUpBeneficiary.Application.SyncDataService.WebService.Response;
using TopUpBeneficiaryService.UnitTests.Fixtures;

namespace TopUpBeneficiaryService.UnitTests.TopUp.TopUpBeneficiaryService.HandlersTests
{
    public class CheckUserBalanceHandlerTests
    {
        private readonly Mock<IAccountClient> _accountClientMock;
        private readonly CheckUserBalanceHandler _handler;

        public CheckUserBalanceHandlerTests()
        {
            _accountClientMock = new Mock<IAccountClient>();
            _handler = new CheckUserBalanceHandler(_accountClientMock.Object);
        }

        [Fact]
        public async Task HandleAsync_ShouldReturnFailure_WhenBalanceIsInsufficient()
        {
            // Arrange
            var user = UserFixtures.User;
            var beneficiary = BeneficiaryFixtures.ActiveBeneficiary;
            int topUpAmount = 100;
            int charge = 1;

            _accountClientMock.Setup(client => client.GetBalance(user.AccountId))
                .ReturnsAsync(Result.Success(new GetBalanceResponse {AccountId = user.AccountId.Value, Balance = 50 }));

            // Act
            var result = await _handler.HandleAsync(user, beneficiary, topUpAmount, charge);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(UserErrors.NoEnoughBalance());
        }

        [Fact]
        public async Task HandleAsync_ShouldCallNextHandler_WhenBalanceIsSufficient()
        {
            // Arrange
            var user = UserFixtures.User;
            var beneficiary = BeneficiaryFixtures.ActiveBeneficiary;
            int topUpAmount = 100;
            int charge = 1;

            _accountClientMock.Setup(client => client.GetBalance(user.AccountId))
                .ReturnsAsync(Result.Success(new GetBalanceResponse { AccountId = user.AccountId.Value, Balance = 101 }));

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
