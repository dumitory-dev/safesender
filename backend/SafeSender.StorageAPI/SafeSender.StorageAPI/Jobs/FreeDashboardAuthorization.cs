using Hangfire.Dashboard;

namespace SafeSender.StorageAPI.Jobs;

public class FreeDashboardAuthorization: IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        return true;
    }
}