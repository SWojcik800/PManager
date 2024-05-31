using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProcessManager.App.Wpf.Core.Data;
using ProcessManager.App.Wpf.Core.Services;

namespace ProcessManager.App.Wpf.Core.Settings.Dictionaries
{
    public sealed class DictionariesService : IDictionaryService
    {
        private readonly ApplicationDbContext _context;

        public DictionariesService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<DataDictionary> GetAllDictionaries()
        {
            var dictionaries = _context.Dictionaries.ToList();
            return dictionaries;
        }

        public async Task<List<DictionaryItem>> GetDictionaryItems(int dictionaryId)
        {
            var items = await _context.DictionaryItems.Where(x => x.DictionaryId == dictionaryId)
                .ToListAsync();

            return items;
        }

        public async Task SaveDictionary(DataDictionary item)
        {
            if(item.Id == 0)
            {
                _context.Dictionaries.Add(item);
            } else
            {
                _context.Dictionaries.Update(item);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<int> SaveItemChanges(List<DictionaryItem> items)
        {
            if (!items.Any())
                return 0;

            foreach (var item in items)
            {
                if(item.Id == 0)
                    _context.DictionaryItems.Add(item); 
                else
                    _context.DictionaryItems.Update(item);
            }

            return await _context.SaveChangesAsync();
        }
    }
}
