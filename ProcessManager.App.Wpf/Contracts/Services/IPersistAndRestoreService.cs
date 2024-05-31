using ProcessManager.App.Wpf.Core.Data;

namespace ProcessManager.App.Wpf.Contracts.Services;

public interface IPersistAndRestoreService
{
    void RestoreData();

    void PersistData();
    void SetConnectionCreds(DbConnectionCreds connectionCreds);
    DbConnectionCreds GetDbConnectionCreds();
    string GetLastSuccessfullLogin();
    void SaveSuccessfullLogin(string userName);
}
