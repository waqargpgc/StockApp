using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace DAL.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext _context;
        protected readonly DbSet<TEntity> _entities;

        public Repository(DbContext context)
        {
            _context = context;
            _entities = context.Set<TEntity>();
        }

        public virtual void Add(TEntity entity)
        {
            _entities.Add(entity);
        }

        public virtual void AddRange(IEnumerable<TEntity> entities)
        {
            _entities.AddRange(entities);
        }


        public virtual void Update(TEntity entity)
        {

            _entities.Update(entity);

            //EntityEntry dbEntityEntry = _context.Entry<TEntity>(entity);
            //dbEntityEntry.State = EntityState.Modified;

            //_context.Set<TEntity>().AddOrUpdate(entity);
        }

        public virtual void UpdateRange(IEnumerable<TEntity> entities)
        {
            _entities.UpdateRange(entities);
        }



        public virtual void Remove(TEntity entity)
        {
            _entities.Remove(entity);
        }

        public virtual void RemoveRange(IEnumerable<TEntity> entities)
        {
            _entities.RemoveRange(entities);
        }


        public virtual int Count()
        {
            return _entities.Count();
        }

          
        public virtual TEntity GetSingle(Expression<Func<TEntity, bool>> predicate)
        {
            return _entities.SingleOrDefault(predicate);
        }

        public virtual TEntity GetById(int id)
        {
            return _entities.Find(id);
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return _entities.ToList();
        }

        public virtual async Task<TEntity> GetAsync(int id)
        {
            return await _entities.FindAsync(id);
        }

        // ===================================== //

        public TEntity GetSingleIncluding(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = _entities;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query.Where(predicate).FirstOrDefault();
        }


        public virtual async Task<ICollection<TEntity>> GetAllAsyn()
        {
            return await _entities.ToListAsync();
        }

        public IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> queryable = GetAll().AsQueryable();
            foreach (Expression<Func<TEntity, object>> includeProperty in includeProperties)
            {
                queryable = queryable.Include<TEntity, object>(includeProperty);
            }
            return queryable;
        }

        public virtual async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> match)
        {
            return await _entities.SingleOrDefaultAsync(match);
        }

        public IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>> match)
        {
            return _entities.Where(match);
        }

        public async Task<ICollection<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> match)
        {
            return await _entities.Where(match).ToListAsync();
        }

        public virtual IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate)
        {
            IQueryable<TEntity> query = _entities.Where(predicate);
            return query;
        }


        public virtual async Task<ICollection<TEntity>> FindByAsyn(Expression<Func<TEntity, bool>> predicate)
        {
            return await _entities.Where(predicate).ToListAsync();
        }

        public virtual async Task<TEntity> AddAsyn(TEntity entity)
        {
            _entities.Add(entity);
            await _context.SaveChangesAsync();
            return entity;

        }

        public virtual void Delete(TEntity entity)
        {
            EntityEntry dbEntityEntry = _context.Entry<TEntity>(entity);
            dbEntityEntry.State = EntityState.Deleted;
        }

        public virtual void DeleteWhere(Expression<Func<TEntity, bool>> predicate)
        {
            IEnumerable<TEntity> entities = _entities.Where(predicate);

            foreach (var entity in entities)
            {
                _context.Entry<TEntity>(entity).State = EntityState.Deleted;
            }
        }

        public virtual async Task<int> DeleteAsyn(TEntity entity)
        {
            _entities.Remove(entity);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> CountAsync()
        {
            return await _entities.CountAsync();
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
