using System.Linq.Expressions;
using Monopoly.Models;

namespace Monopoly.Repositories;

public interface IBaseRepository<TEntity> where TEntity : class
{
    Task<IEnumerable<TEntity>> GetAll();
    Task<IEnumerable<TEntity>> GetList(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity> GetById(Guid id);
    Task Add(TEntity model);
    Task Update(TEntity model);
    Task<bool> Remove(Guid id);
    Task Save();
}