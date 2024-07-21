using Microsoft.AspNetCore.Mvc;
using Moq;
using TopUpBeneficiary.Api.Controllers;
using TopUpBeneficiary.Application.Dtos.Response;
using TopUpBeneficiary.Application.Services.Beneficiaries;

namespace TopUpBeneficiary.UnitTests;

public class BeneficiariesControllerTests
{
    //UnitOfWork_StateUnderTest_ExpectedBehavior()
    [Fact]
    public async Task GetBeneficiaries_WithUnExistingItems_ReturnsNotFound()
    {
        //Arrange
        var serviceStub = new Mock<IBeneficiaryService>();
      //  serviceStub.Setup(service => service.GetBeneficiaries(It.IsAny<Guid>())).ReturnsAsync((IList<BeneficiaryDto>)null);
        
        var controller = new BeneficiariesController(serviceStub.Object);

        //Act
        var result = await controller.GetBeneficiaries(Guid.NewGuid());

        //Assert
        Assert.IsType<NotFoundResult>(result);
    }
}