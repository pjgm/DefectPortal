using Application.Common.Interfaces;
using Application.Defects.Commands;
using Microsoft.EntityFrameworkCore;
using Moq;
using Shouldly;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Application.UnitTests
{
    public class CreateDefectTests
    {
        [Fact]
        public async Task CreateDefectCommandHandler_ValidRequest_SingleDefectCreated()
        {
            // Arrange
            var entriesWritten = 1;
            var personalNumber = "personalNr";
            var machineId = 1;
            var dbContext = Mock.Of<IApplicationDbContext>();
            var handler = new CreateDefectCommandHandler(dbContext);

            Mock.Get(dbContext)
                .Setup(r => r.Workers.AnyAsync(w => w.PersonalNumber == personalNumber, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            Mock.Get(dbContext)
                .Setup(r => r.Defects.AnyAsync(m => m.MachineId == machineId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            Mock.Get(dbContext)
                .Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(entriesWritten);

            // Act
            var result = await handler.Handle(new CreateDefectCommand(personalNumber, "test2", machineId), default);

            // Assert
            result.ShouldBe(entriesWritten);
        }
    }
}