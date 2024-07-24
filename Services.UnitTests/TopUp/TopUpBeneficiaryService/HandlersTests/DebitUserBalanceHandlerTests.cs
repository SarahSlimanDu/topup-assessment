using Commons.Errors;
using TopUpBeneficiary.Application.Dtos.Request;
using TopUpBeneficiary.Application.Services.TopUp.Handlers.Concrete;
using TopUpBeneficiary.Application.SyncDataService.WebService.Client;
using TopUpBeneficiaryService.UnitTests.Fixtures;

namespace TopUpBeneficiaryService.UnitTests.TopUp.TopUpBeneficiaryService.HandlersTests
{
    public class DebitUserBalanceHandlerTests
    {
        private readonly Mock<IAccountClient> _accountClientMock;
        private readonly DebitUserBalanceHandler _handler;

        public DebitUserBalanceHandlerTests()
        {
            _accountClientMock = new Mock<IAccountClient>();
            _handler = new DebitUserBalanceHandler(_accountClientMock.Object);
        }

        [Fact]
        public async Task HandleAsync_ShouldReturnSuccess_WhenDebitIsSuccessful()
        {
            // Arrange
            var user = UserFixtures.User;
            var beneficiary = BeneficiaryFixtures.ActiveBeneficiary;
            int topUpAmount = 100;
            int charge = 1;

            _accountClientMock.Setup(client => client.DebitBalance(It.Is<DebitBalanceDto>(d => d.accountIban == user.AccountId && d.debitAmount == topUpAmount + charge)))
                .ReturnsAsync(Result.Success());

            // Act
            var result = await _handler.HandleAsync(user, beneficiary, topUpAmount, charge);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }
    }


}
