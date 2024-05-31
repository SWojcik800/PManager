using System.Collections;
using System.IO;

using Microsoft.Extensions.Options;

using ProcessManager.App.Wpf.Contracts.Services;
using ProcessManager.App.Wpf.Core.Contracts.Services;
using ProcessManager.App.Wpf.Models;
using System.Linq;
using ProcessManager.App.Wpf.Core.Data;
using Newtonsoft.Json;

namespace ProcessManager.App.Wpf.Services;

public class PersistAndRestoreService : IPersistAndRestoreService
{
    private readonly IFileService _fileService;
    private readonly AppConfig _appConfig;
    private readonly string _localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

    public PersistAndRestoreService(IFileService fileService, IOptions<AppConfig> appConfig)
    {
        _fileService = fileService;
        _appConfig = appConfig.Value;
    }

    public void PersistData()
    {
        if (App.Current.Properties != null)
        {
            var folderPath = Path.Combine(_localAppData, _appConfig.ConfigurationsFolder);
            var fileName = _appConfig.AppPropertiesFileName;
            _fileService.Save(folderPath, fileName, App.Current.Properties);            
        }
    }

    public void RestoreData()
    {
        var folderPath = Path.Combine(_localAppData, _appConfig.ConfigurationsFolder);
        var fileName = _appConfig.AppPropertiesFileName;
        var properties = _fileService.Read<IDictionary>(folderPath, fileName);
        if (properties != null)
        {
            foreach (DictionaryEntry property in properties)
            {
                if(App.Current.Properties.Contains(property.Key))
                {
                    App.Current.Properties[property.Key] = property.Value;
                }
                else
                {
                    App.Current.Properties.Add(property.Key, property.Value);                    
                }
            }
        }
    }

    public void SaveSuccessfullLogin(string userName)
    {
        App.Current.Properties["LastLoginUsername"] = userName;
        PersistData();
    }

    public string GetLastSuccessfullLogin()
        => App.Current.Properties["LastLoginUsername"] is null ? "" : App.Current.Properties["LastLoginUsername"].ToString();

    public void SetConnectionCreds(DbConnectionCreds connectionCreds)
    {
        App.Current.Properties["ConnectionString"] = connectionCreds;
    }
    public DbConnectionCreds GetDbConnectionCreds()
    {
        var value = App.Current.Properties["ConnectionString"];

        try
        {
            if (value != null)
                return JsonConvert.DeserializeObject<DbConnectionCreds>(value.ToString());

        }
        catch (Exception)
        {
            return new DbConnectionCreds();
        }

        return new DbConnectionCreds();
    }




}
