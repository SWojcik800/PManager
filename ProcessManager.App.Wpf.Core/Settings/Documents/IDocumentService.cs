using ProcessManager.App.Wpf.Core.Shared.Results;

namespace ProcessManager.App.Wpf.Core.Settings.Documents
{
    public interface IDocumentService
    {
        Task<Result> AddTemplate(DocumentTemplate template);
        Task<Result<byte[]>> GenerateDocument(Dictionary<string, string> fields, int documentTemplateId);
        Task<List<DocumentTemplate>> GetAll();
        Task<DocumentTemplate> GetById(int id);
        Task<Result> UpdateTemplate(DocumentTemplate template);
    }
}