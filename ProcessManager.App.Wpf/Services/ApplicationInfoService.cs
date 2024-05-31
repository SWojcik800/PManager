using System.Diagnostics;
using System.Reflection;

using ProcessManager.App.Wpf.Contracts.Services;

namespace ProcessManager.App.Wpf.Services;

public class ApplicationInfoService : IApplicationInfoService
{
    public ApplicationInfoService()
    {
    }

    public Version GetVersion()
    {
        // Set the app version in ProcessManager.App.Wpf > Properties > Package > PackageVersion
        string assemblyLocation = Assembly.GetExecutingAssembly().Location;
        var version = FileVersionInfo.GetVersionInfo(assemblyLocation).FileVersion;
        return new Version(version);
    }
}
