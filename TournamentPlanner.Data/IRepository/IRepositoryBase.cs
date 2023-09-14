using System.Linq.Expressions;

namespace TournamentPlanner.Data.IRepository;

public interface IRepositoryBase<T>
{
    IQueryable<T> GetAll();
    IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
    Task Create(T entity);
    Task Update(T entity);
    Task Delete(T entity);
}
