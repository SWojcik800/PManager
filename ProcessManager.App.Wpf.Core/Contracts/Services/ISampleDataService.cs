using ProcessManager.App.Wpf.Core.Models;

namespace ProcessManager.App.Wpf.Core.Contracts.Services;

public interface ISampleDataService
{
  

    Task<IEnumerable<SampleOrder>> GetGridDataAsync();

    Task<IEnumerable<SampleOrder>> GetContentGridDataAsync();
}
