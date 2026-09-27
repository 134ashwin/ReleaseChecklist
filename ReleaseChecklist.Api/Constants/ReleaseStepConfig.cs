using ReleaseChecklist.Api.Models;
using System.Net.NetworkInformation;
using System.Numerics;

namespace ReleaseChecklist.Api.Constants
{
    public record ReleaseStep(string Id, string Name, string Description, int Order);

    public static class ReleaseStepConfig
    {
        //A static helper class containing the fixed set of 7 checklist steps and the pure status calculation function.
        //Why we need it 
        // It Ensures consistent step IDs across backend and frontend.
        //Automatically computes release status (PLANNED, ONGOING, or DONE) based on checked step counts.


        public static readonly List<ReleaseStep> AllSteps = new()
    {
        new("DB_MIGRATION", "DB Migration", "Execute database schema updates & migrations", 1),
        new("RUN_SMOKE_TESTS", "Run Smoke Tests", "Run automated E2E and sanity verification tests", 2),
        new("UPDATE_DNS", "Update DNS", "Route web traffic to new infrastructure endpoints", 3),
        new("DEPLOY_API", "Deploy API", "Deploy compiled .NET API services to host clusters", 4),
        new("TAG_GIT_RELEASE", "Tag Git Release", "Publish release semver tag in version control", 5),
        new("NOTIFY_SLACK", "Notify Slack", "Trigger release notifications across team channels", 6),
        new("VERIFY_METRICS", "Verify Metrics", "Monitor latency, error rates, and telemetry", 7)
    };

        public static readonly int TotalSteps = AllSteps.Count;

        public static ReleaseStatus CalculateStatus(List<string>? completedStepIds)
        {
            if (completedStepIds == null || completedStepIds.Count == 0)
            {
                return ReleaseStatus.PLANNED;
            }

            if (completedStepIds.Count >= TotalSteps)
            {
                return ReleaseStatus.DONE;
            }

            return ReleaseStatus.ONGOING;
        }
    }
}
