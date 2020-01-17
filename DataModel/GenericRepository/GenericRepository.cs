using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.GenericRepository
{
    public class GenericRepository<TEntity> where TEntity:class
    {
        internal WebApiDbEntities Context;
        internal DbSet<TEntity> DbSet;

        public GenericRepository(WebApiDbEntities context)
        {
            this.Context = context;
            this.DbSet = context.Set<TEntity>();
        }

        public virtual IEnumerable<TEntity> Get() => DbSet.ToList();
        //public virtual IEnumerable<TEntity> Get()
        //{
        //    IQueryable<TEntity> query = DbSet;
        //    return query.ToList();
        //}

        public virtual TEntity GetByID(object id) => DbSet.Find(id);

        public virtual void Insert(TEntity entity) => DbSet.Add(entity);

        public virtual void Delete(object id) =>
            Delete(DbSet.Find(id));
        public virtual void Delete(TEntity entity)
        {
            if (Context.Entry(entity).State == EntityState.Detached)
                DbSet.Attach(entity);
            DbSet.Remove(entity);
        }

        public virtual void Update (TEntity entity)
        {
            DbSet.Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;
        }

        public virtual IEnumerable<TEntity> GetMany(Func<TEntity, bool> where) => 
            DbSet.Where(where).ToList();

        public virtual IQueryable<TEntity> GetManyQueryable(Func<TEntity, bool> where) =>
            DbSet.Where(where).AsQueryable();

        public TEntity Get(Func<TEntity, bool> where) =>
            DbSet.Where(where).FirstOrDefault<TEntity>();

        /// <summary>
        /// generic delete method , deletes data for the entities on the basis of condition.
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public void Delete(Func<TEntity, Boolean> where)
        {
            IQueryable<TEntity> objects = DbSet.Where<TEntity>(where).AsQueryable();
            foreach (TEntity obj in objects)
                DbSet.Remove(obj);
        }

        /// <summary>
        /// generic method to fetch all the records from db
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> GetAll()
        {
            return DbSet.ToList();
        }

        /// <summary>
        /// Inclue multiple
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        public IQueryable<TEntity> GetWithInclude(
            System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate, params
            string[] include)
        {
            IQueryable<TEntity> query = this.DbSet;
            query = include.Aggregate(query, (current, inc) => current.Include(inc));
            return query.Where(predicate);
        }
        /// <summary>
        /// Generic method to check if entity exists
        /// </summary>
        /// <param name="primaryKey"></param>
        /// <returns></returns>
        public bool Exists(object primaryKey)
        {
            return DbSet.Find(primaryKey) != null;
        }
        /// <summary>
        /// Gets a single record by the specified criteria (usually the unique identifier)
        /// </summary>
        /// <param name="predicate">Criteria to match on</param>
        /// <returns>A single record that matches the specified criteria</returns>
        public TEntity GetSingle(Func<TEntity, bool> predicate)
        {
            return DbSet.Single<TEntity>(predicate);
        }
        /// <summary>
        /// The first record matching the specified criteria
        /// </summary>
        /// <param name="predicate">Criteria to match on</param>
        /// <returns>A single record containing the first record matching the specified criteria</returns>
        public TEntity GetFirst(Func<TEntity, bool> predicate) =>
            DbSet.First<TEntity>(predicate);
    }
}
