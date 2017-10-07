using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Repositories
{
    public class GenericRepository<TEntity> where TEntity : class
    {
        internal BookLibraryContext context;
        internal DbSet<TEntity> dbSet;
        public GenericRepository(BookLibraryContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }
        public virtual IQueryable<TEntity> GetAll()
        {
            return dbSet;
        }
        public virtual TEntity GetByID(object id)
        {
            return dbSet.Find(id);
        }
        public virtual void Delete(object id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entity)
        {
            if (context.Entry(entity).State == EntityState.Detached)
            {
                dbSet.Attach(entity);
            }
            dbSet.Remove(entity);
        }

        public virtual void Update(TEntity entity)
        {
            dbSet.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }
        public virtual void Add(TEntity entity)
        {
            dbSet.Add(entity);
        }
    }
}
