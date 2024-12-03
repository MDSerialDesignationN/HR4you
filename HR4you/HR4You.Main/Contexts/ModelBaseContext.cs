using HR4You.Model.Base;
using Microsoft.EntityFrameworkCore;

namespace HR4You.Contexts;

public class ModelBaseContext<T> : DbContext where T : ModelBase
{
    private readonly ILogger<ModelBaseContext<T>> _logger;
    protected DbSet<T> Entities => Set<T>();

    public ModelBaseContext(DbContextOptions<ModelBaseContext<T>> options, ILogger<ModelBaseContext<T>> logger) :
        base(options)
    {
        _logger = logger;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<T>().ToTable($"hr4you_{typeof(T).Name}");
        modelBuilder.Entity<T>().Property(e => e.Id).ValueGeneratedOnAdd();
    }

    public async Task<ModelResult<T>> Create(T data)
    {
        data.Id = default;
        data.CreationDateTime = DateTime.Now;
        data.LastModifiedAt = null;
        var e = Entities.Add(data);
        await SaveChangesAsync();

        return new ModelResult<T>
        {
            Entity = e.Entity,
            Error = MasterDataError.None
        };
    }

    public async Task<ModelResult<T>> Edit(int id, T data)
    {
        var result = await Get(id);
        if (result.Error == MasterDataError.NotFound)
        {
            return result;
        }

        result.Entity!.Set(data);
        Entry(result.Entity).State = EntityState.Modified;
        _ = await SaveChangesAsync();
        return result;
    }

    public async Task<ModelResult<T>> SetDelete(int id, bool deleted)
    {
        var result = await Get(id);
        if (result.Error == MasterDataError.NotFound)
        {
            return result;
        }

        if (deleted)
        {
            result.Entity!.LastModifiedAt = DateTime.Now;
        }

        result.Entity!.Deleted = deleted;
        await SaveChangesAsync();
        return result;
    }

    public async Task<ModelResult<T>> Get(int id, bool addDeleted = true)
    {
        var linq = Entities.AsQueryable();
        if (!addDeleted)
        {
            linq = linq.Where(e => e.Deleted != true);
        }

        var entity = await linq.FirstOrDefaultAsync(j => j.Id == id);
        if (entity == null)
        {
            _logger.LogError("Entity with ID {Id} was not found", id);
            return ModelResult<T>.NotFound();
        }

        return ModelResult<T>.Ok(entity);
    }

    public async Task<ModelResult<List<T>>> GetAll(bool addDeleted = true)
    {
        var linq = Entities.AsQueryable();
        if (!addDeleted)
        {
            linq = linq.Where(e => e.Deleted != true);
        }

        var result = await linq.OrderByDescending(x => x.Id).ToListAsync();
        return ModelResult<List<T>>.Ok(result);
    }
}