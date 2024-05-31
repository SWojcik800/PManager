using Microsoft.EntityFrameworkCore;
using ProcessManager.App.Wpf.Core.Data;
using ProcessManager.App.Wpf.Core.Shared.Results;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManager.App.Wpf.Core.Settings.Documents
{
    internal sealed class DocumentService : IDocumentService
    {
        private readonly ApplicationDbContext _context;

        public DocumentService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result> AddTemplate(DocumentTemplate template)
        {
            _context.Add(template);
            await _context.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result> UpdateTemplate(DocumentTemplate template)
        {
            _context.Update(template);
            await _context.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<DocumentTemplate> GetById(int id)
        {
            var result = await _context.DocumentTemplates.FindAsync(id);
            return result;
        }

        public async Task<List<DocumentTemplate>> GetAll()
        {
            var result = await _context.DocumentTemplates.ToListAsync();
            return result;
        }

        public async Task<Result<byte[]>> GenerateDocument(Dictionary<string, string> fields, int documentTemplateId)
        {
            var template = await _context.DocumentTemplates.FindAsync(documentTemplateId);
            string result = Encoding.UTF8.GetString(template.Content);

            foreach (var field in fields)
            {
                result.Replace($"%{field.Key}%", field.Value);
            }

            byte[] byteArray = Encoding.UTF8.GetBytes(result);
            return Result<byte[]>.Success(byteArray);
        }
    }
}
