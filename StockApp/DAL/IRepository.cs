using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);

        void Update(TEntity entity);
        void UpdateRange(IEnumerable<TEntity> entities);

        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);

        int Count();
         
        TEntity GetSingle(Expression<Func<TEntity, bool>> predicate);
        TEntity GetById(int id);
        IEnumerable<TEntity> GetAll();


        // --------------------------------------
        Task<TEntity> GetAsync(int id);
        TEntity GetSingleIncluding(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties);

        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate);

        IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate);
        Task<ICollection<TEntity>> FindByAsyn(Expression<Func<TEntity, bool>> predicate);

        Task<ICollection<TEntity>> GetAllAsyn();

        IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] includeProperties);


        IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>> match);
        Task<ICollection<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> match);



        void Delete(TEntity entity);
        Task<int> DeleteAsyn(TEntity entity);
        void DeleteWhere(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> AddAsyn(TEntity entity);

        Task<int> CountAsync();
        void Dispose();

    }

}
