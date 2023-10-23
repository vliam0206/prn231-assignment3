using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class BaseDAO<TEntity> where TEntity : class
{
    public static async Task<List<TEntity>> GetEntitiesAsync()
    {
        var list = new List<TEntity>();
        try
        {
            using (var context = new FucarRentingManagementContext())
            {
                list = await context.Set<TEntity>().ToListAsync();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return list;
    }
    public static async Task SaveEntityAsync(TEntity p)
    {
        try
        {
            using (var context = new FucarRentingManagementContext())
            {
                context.Set<TEntity>().Add(p);
                await context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public static async Task SaveRangeAsync(List<TEntity> ls)
    {
        try
        {
            using (var context = new FucarRentingManagementContext())
            {
                await context.Set<TEntity>().AddRangeAsync(ls);
                await context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public static async Task UpdateEntityAsync(TEntity p)
    {
        try
        {
            using (var context = new FucarRentingManagementContext())
            {
                context.Entry(p).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
