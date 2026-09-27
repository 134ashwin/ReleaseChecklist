using ReleaseChecklist.Api.Models;
using System.Runtime.Intrinsics.X86;
using static HotChocolate.ErrorCodes;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.EntityFrameworkCore;
using ReleaseChecklist.Api.Constants;
using ReleaseChecklist.Api.Data;

namespace ReleaseChecklist.Api.GraphQL
{
    public class Query
    {
        //Query.cs contains C# functions that READ data from your PostgreSQL database and return it to the Angular frontend.
        //In standard REST APIs, you create Controllers with [HttpGet] endpoints.
        //In GraphQL(which we are using via Hot Chocolate), we use a Query class instead.
        public async Task<List<Release>> GetReleasesAsync([Service] AppDbContext dbContext)
        {
            var releases = await dbContext.Releases
                .AsNoTracking()
                .OrderByDescending(r => r.ReleaseDate)
                .ToListAsync();

            foreach (var release in releases)
            {
                release.Status = ReleaseStepConfig.CalculateStatus(release.CompletedStepIds);
            }

            return releases;
        }

        public async Task<Release?> GetReleaseAsync(Guid id, [Service] AppDbContext dbContext)
        {
            var release = await dbContext.Releases
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id);

            if (release != null)
            {
                release.Status = ReleaseStepConfig.CalculateStatus(release.CompletedStepIds);
            }

            return release;
        }

        public List<ReleaseStep> GetAvailableSteps() => ReleaseStepConfig.AllSteps;
    }
}
