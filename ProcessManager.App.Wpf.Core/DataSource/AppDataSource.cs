using Microsoft.EntityFrameworkCore;
using ProcessManager.App.Wpf.Core.Data;

namespace ProcessManager.App.Wpf.Core.DataSource
{
    public abstract class AppDataSource<T> where T : class, IEntity
    {
        protected AppDataSource(ApplicationDbContext context)
        {
            DbContext = context;
        }

        protected readonly ApplicationDbContext DbContext;
        public bool NextPageExists { get; private set; }
        public virtual AppDataSourceQueryParams QueryParams { get; set; } = new AppDataSourceQueryParams();
        public PaginatedResult<T> Data { get; protected set; }

        protected DbSet<T> DbSet
        {
            get { return DbContext.Set<T>(); }
        }

        public async Task FetchData()
        {
            var totalCount = DbSet.Count();
            var items = GetBaseFetchDataQuery().Take(QueryParams.MaxRecords).ToList();
            var data = new PaginatedResult<T>(items, totalCount);
            Data = data;

        }

        protected virtual IQueryable<T> GetBaseFetchDataQuery()
        {
            return DbSet.AsQueryable<T>();
        }

        public async Task Save(T entity, bool saveChanges = true)
        {
            var isNewEntity = entity.Id == 0;

            if (isNewEntity)
                DbSet.Add(entity);
            else
                DbSet.Update(entity);

            if (saveChanges)
                await DbContext.SaveChangesAsync();

        }

        public async Task<int> SaveChanges()
            => await DbContext.SaveChangesAsync();

    }

    public interface IEntity
    {
        public int Id { get; set; }
    }

    public class AppDataSourceQueryParams
    {
        public int MaxRecords { get; set; } = 25;
    }

    public class PaginatedResult<T> where T : class
    {
        public PaginatedResult(List<T> items, int totalCount)
        {
            Items = items;
            TotalCount = totalCount;
        }

        public PaginatedResult(List<T> items)
        {
            Items = items;
            TotalCount = items.Count;
        }

        public List<T> Items { get; private set; }
        public int TotalCount { get; private set; }
        public bool NextPageExists
        {
            get { return Items.Count < TotalCount; }
        }
    }


}
