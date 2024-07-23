using TopUpBeneficiary.Application.Dtos.Request;
using TopUpBeneficiary.Application.Services.Beneficiaries;
using TopUpBeneficiary.Domain.Persistence.Interfaces.Commons;
using TopUpBeneficiary.Domain.Persistence.Interfaces.Repository;
using TopUpBeneficiary.Domain.UserAggregate;
using TopUpBeneficiaryService.UnitTests.Fixtures;

namespace Services.UnitTests.Beneficiaries
{
    public class AddBeneficiaryTests
    {
        private readonly Mock<IBeneficiaryRepository> _beneficiaryRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly BeneficiaryService _beneficiaryService;

        public AddBeneficiaryTests()
        {
            _beneficiaryRepositoryMock = new Mock<IBeneficiaryRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _beneficiaryService = new BeneficiaryService(
                _beneficiaryRepositoryMock.Object,
                _userRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _mapperMock.Object
            );
        }

        [Fact]
        public async Task AddBeneficiary_ShouldReturnError_WhenUserHasFiveActiveBeneficiaries()
        {
            // Arrange
            var request = new AddBeneficiaryDto { UserId = UserFixtures.User.Id.Value, PhoneNumber = "1234567890", NickName = "TestNick" };

            _userRepositoryMock.Setup(repo => repo.GetById(UserFixtures.User.Id)).ReturnsAsync(UserFixtures.User);
            _beneficiaryRepositoryMock.Setup(repo => repo.GetActiveBeneficiariesByUserId(UserFixtures.User.Id)).ReturnsAsync(BeneficiaryFixtures.GetBeneficiaries());

            // Act
            var result = await _beneficiaryService.AddBeneficiary(request);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(BeneficiaryErrors.CountLimitReached());
        }

        [Fact]
        public async Task AddBeneficiary_ShouldReturnError_WhenUserDoesNotExist()
        {
            // Arrange
            var request = new AddBeneficiaryDto { UserId = UserFixtures.User.Id.Value, PhoneNumber = "1234567890", NickName = "TestNick" };

            _userRepositoryMock.Setup(repo => repo.GetById(UserFixtures.User.Id)).ReturnsAsync((User)null);

            // Act
            var result = await _beneficiaryService.AddBeneficiary(request);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(UserErrors.NotFoundById());
        }

        [Fact]
        public async Task AddBeneficiary_ShouldReturnError_WhenDuplicateNickName()
        {
            // Arrange
            var request = new AddBeneficiaryDto { UserId = UserFixtures.User.Id.Value, PhoneNumber = "1234567890", NickName = "DuplicateNick" };
            _userRepositoryMock.Setup(repo => repo.GetById(UserFixtures.User.Id)).ReturnsAsync(UserFixtures.User);
            _beneficiaryRepositoryMock.Setup(repo => repo.GetActiveBeneficiariesByUserId(UserFixtures.User.Id)).ReturnsAsync(BeneficiaryFixtures.GetBeneficiaryWithSameNickName());

            // Act
            var result = await _beneficiaryService.AddBeneficiary(request);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(BeneficiaryErrors.DuplicateNickName());
        }

        [Fact]
        public async Task AddBeneficiary_ShouldReturnError_WhenDuplicatePhoneNumber()
        {
            // Arrange
            var request = new AddBeneficiaryDto { UserId = UserFixtures.User.Id.Value, PhoneNumber = "123456", NickName = "Nick1" };
            _userRepositoryMock.Setup(repo => repo.GetById(UserFixtures.User.Id)).ReturnsAsync(UserFixtures.User);
            _beneficiaryRepositoryMock.Setup(repo => repo.GetActiveBeneficiariesByUserId(UserFixtures.User.Id)).ReturnsAsync(BeneficiaryFixtures.GetBeneficiaryWithSamePhoneNumber());

            // Act
            var result = await _beneficiaryService.AddBeneficiary(request);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(BeneficiaryErrors.DuplicatePhoneNumber());
        }
    }
}
