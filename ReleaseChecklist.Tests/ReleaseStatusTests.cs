using ReleaseChecklist.Api.Constants;
using ReleaseChecklist.Api.Models;
using Xunit;

namespace ReleaseChecklist.Tests;

public class ReleaseStatusTests
{
    [Fact]
    public void CalculateStatus_WhenZeroStepsCompleted_ShouldReturnPLANNED()
    {
        // Arrange
        var completedSteps = new List<string>();

        // Act
        var status = ReleaseStepConfig.CalculateStatus(completedSteps);

        // Assert
        Assert.Equal(ReleaseStatus.PLANNED, status);
    }

    [Fact]
    public void CalculateStatus_WhenSomeStepsCompleted_ShouldReturnONGOING()
    {
        // Arrange
        var completedSteps = new List<string> { "DB_MIGRATION", "RUN_SMOKE_TESTS" };

        // Act
        var status = ReleaseStepConfig.CalculateStatus(completedSteps);

        // Assert
        Assert.Equal(ReleaseStatus.ONGOING, status);
    }

    [Fact]
    public void CalculateStatus_WhenAllStepsCompleted_ShouldReturnDONE()
    {
        // Arrange
        var completedSteps = ReleaseStepConfig.AllSteps.Select(s => s.Id).ToList();

        // Act
        var status = ReleaseStepConfig.CalculateStatus(completedSteps);

        // Assert
        Assert.Equal(ReleaseStatus.DONE, status);
    }
}