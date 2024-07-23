using Microsoft.Extensions.Options;
using TopUpBeneficiary.Application.Dtos.Response;
using TopUpBeneficiary.Application.Services.TopUp;
using TopUpBeneficiary.Application.Services.TopUp.Handlers.Interface;
using TopUpBeneficiary.Application.SyncDataService.WebService.Client;
using TopUpBeneficiary.Domain.Commons.Constants;
using TopUpBeneficiary.Domain.Persistence.Interfaces.Commons;
using TopUpBeneficiary.Domain.Persistence.Interfaces.Repository;
using TopUpBeneficiary.Domain.TopUpOptionsAggregate;
using TopUpBeneficiaryService.UnitTests.Fixtures;

namespace TopUpBeneficiaryService.UnitTests.TopUp
{
    public class GetTopUpOptionsTests
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

        public GetTopUpOptionsTests()
        {
            _handlerMock = new Mock<IHandler>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _topUpOptionsRepositoryMock = new Mock<ITopUpOptionsRepository>();
            _topUpTransactionRepositoryMock = new Mock<ITopUpTransactionRepository>();
            _beneficiaryRepositoryMock = new Mock<IBeneficiaryRepository>();
            _accountClientMock = new Mock<IAccountClient>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _appConstantsMock = new Mock<IOptions<AppConstants>>();

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
        }

        [Fact]
        public async Task GetTopUpOptions_ShouldReturnNotFound_WhenNoOptionsExist()
        {
            // Arrange
            _topUpOptionsRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(new List<TopUpOption>());

            // Act
            var result = await _topUpService.GetTopUpOptions();

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(TopUpOptionsErrors.NoOptionsFound());
        }

        [Fact]
        public async Task GetTopUpOptions_ShouldReturnOptions_WhenOptionsExist()
        {
            // Arrange
            var topUpOptions = TopUpOptionsFixtures.GetTopUpOptions();
            _topUpOptionsRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(topUpOptions);
            _mapperMock.Setup(mapper => mapper.Map<IList<TopUpOptionsDto>>(It.IsAny<IList<TopUpOption>>())).Returns(TopUpOptionsFixtures.GetTopUpOptionsDtos(topUpOptions));

            // Act
            var result = await _topUpService.GetTopUpOptions();

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.Should().HaveCount(2);
            result.Value.Should().BeEquivalentTo(TopUpOptionsFixtures.GetTopUpOptionsDtos(topUpOptions));
        }
    }
}
