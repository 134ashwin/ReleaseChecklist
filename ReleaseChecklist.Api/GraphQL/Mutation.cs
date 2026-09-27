using Microsoft.EntityFrameworkCore;
using ReleaseChecklist.Api.Constants;
using ReleaseChecklist.Api.Data;
using ReleaseChecklist.Api.Models;


//Define GraphQL mutation endpoints to create releases, update step completions, update additional info, and delete releases.
//Why: GraphQL mutations allow clients to create, modify, and delete data on the server.

namespace ReleaseChecklist.Api.GraphQL;

public record CreateReleaseInput(string Name, DateTimeOffset Date, string? AdditionalInfo);
public record UpdateReleaseStepsInput(Guid Id, List<string> CompletedStepIds);
public record UpdateReleaseInfoInput(Guid Id, string? AdditionalInfo);

public class Mutation
{
    public async Task<Release> CreateReleaseAsync(CreateReleaseInput input, [Service] AppDbContext dbContext)
    {
        var release = new Release
        {
            Id = Guid.NewGuid(),
            Name = input.Name,
            ReleaseDate = input.Date,
            AdditionalInfo = input.AdditionalInfo,
            CompletedStepIds = new List<string>(),
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };

        release.Status = ReleaseStepConfig.CalculateStatus(release.CompletedStepIds);

        dbContext.Releases.Add(release);
        await dbContext.SaveChangesAsync();

        return release;
    }

    public async Task<Release> UpdateReleaseStepsAsync(UpdateReleaseStepsInput input, [Service] AppDbContext dbContext)
    {
        var release = await dbContext.Releases.FirstOrDefaultAsync(r => r.Id == input.Id);
        if (release == null)
        {
            throw new Exception($"Release with ID '{input.Id}' not found.");
        }

        release.CompletedStepIds = input.CompletedStepIds ?? new List<string>();
        release.UpdatedAt = DateTimeOffset.UtcNow;
        release.Status = ReleaseStepConfig.CalculateStatus(release.CompletedStepIds);

        await dbContext.SaveChangesAsync();

        return release;
    }

    public async Task<Release> UpdateReleaseInfoAsync(UpdateReleaseInfoInput input, [Service] AppDbContext dbContext)
    {
        var release = await dbContext.Releases.FirstOrDefaultAsync(r => r.Id == input.Id);
        if (release == null)
        {
            throw new Exception($"Release with ID '{input.Id}' not found.");
        }

        release.AdditionalInfo = input.AdditionalInfo;
        release.UpdatedAt = DateTimeOffset.UtcNow;
        release.Status = ReleaseStepConfig.CalculateStatus(release.CompletedStepIds);

        await dbContext.SaveChangesAsync();

        return release;
    }

    public async Task<bool> DeleteReleaseAsync(Guid id, [Service] AppDbContext dbContext)
    {
        var release = await dbContext.Releases.FirstOrDefaultAsync(r => r.Id == id);
        if (release == null)
        {
            return false;
        }

        dbContext.Releases.Remove(release);
        await dbContext.SaveChangesAsync();

        return true;
    }
}