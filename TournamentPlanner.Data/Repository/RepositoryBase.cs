using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TournamentPlanner.API.Data;
using TournamentPlanner.Data.IRepository;

namespace Repository;

public class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    private readonly TournamentPlannerDbContext _db;

    public RepositoryBase(TournamentPlannerDbContext db)
    {
        _db = db;
    }

    public IQueryable<T> GetAll()
    {
        return _db.Set<T>();
    }
    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
    {
        return _db.Set<T>().Where(expression);
    }


    public async Task Create(T entity)
    {
        _db.Set<T>().Add(entity);
        await _db.SaveChangesAsync();
    }

    public async Task Update(T entity)
    {
        _db.Set<T>().Update(entity);
        await _db.SaveChangesAsync();
    }

    public async Task Delete(T entity)
    {
        _db.Set<T>().Remove(entity);
        await _db.SaveChangesAsync();
    }
}
