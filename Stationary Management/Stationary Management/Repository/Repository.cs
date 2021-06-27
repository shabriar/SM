using SCHM.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
//using System.Linq.Dynamic;

namespace SCHM.Repo
{
    public class BaseRepository<T> : IBaseRepository<T> where T : Entity
    {
        private DbContext _context;
        public BaseRepository(DbContext context)
        {
            _context = context;
        }
        public void Delete(int id)
        {
            T item = GetById(id);
            Delete(item);
        }
        public void Delete(T item)
        {
            item.IsDeleted = true;
        }
        public virtual void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }
        public virtual void Update(T entityToUpdate)
        {
            _context.Set<T>().Attach(entityToUpdate);
            _context.Entry(entityToUpdate).State = EntityState.Modified;
        }
        public virtual IEnumerable<T> GetAll(
          out int total, out int totalDisplay,
          Expression<Func<T, bool>> filter = null,
          Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
          string includeProperties = "", int pageIndex = 1, int pageSize = 10, bool isTrackingOff = false)
        {
            IQueryable<T> query = _context.Set<T>();
            total = query.Count();
            totalDisplay = query.Count();

            if (filter != null)
            {
                query = query.Where(filter);
                totalDisplay = query.Count();
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                var result = orderBy(query).Skip((pageIndex - 1) * pageSize).Take(pageSize);

                if (isTrackingOff)
                    return result.AsNoTracking().ToList();
                else
                    return result.ToList();
            }
            else
            {
                var result = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);

                if (isTrackingOff)
                    return result.AsNoTracking().ToList();
                else
                    return result.ToList();
            }
        }
        public virtual void DeletePermanent(object id)
        {
            T entityToDelete = _context.Set<T>().Find(id);
            DeletePermanent(entityToDelete);
        }

        public virtual IEnumerable<T> GetDynamic(out int total, out int totalDisplay, Expression<Func<T, bool>> filter = null,
            string orderBy = null, string includeProperties = "", int pageIndex = 1, int pageSize = 10, bool isTrackingOff = false)
        {
            IQueryable<T> query = _context.Set<T>();
            total = query.Count();
            totalDisplay = query.Count();

            if (filter != null)
            {
                query = query.Where(filter);
                totalDisplay = query.Count();
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                var result = query.OrderBy(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize);

                if (isTrackingOff)
                {
                    return result.AsNoTracking().ToList();
                }
                else
                {
                    return result.ToList();
                }
            }
            else
            {
                var result = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);

                if (isTrackingOff)
                    return result.AsNoTracking().ToList();
                else
                    return result.ToList();
            }
        }

        public virtual IEnumerable<T> GetDynamic(Expression<Func<T, bool>> filter = null, string orderBy = null,
            string includeProperties = "", bool isTrackingOff = false)
        {
            IQueryable<T> query = _context.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                if (isTrackingOff)
                    return query.OrderBy(orderBy).AsNoTracking().ToList();
                else
                    return query.OrderBy(orderBy).ToList();
            }
            else
            {
                if (isTrackingOff)
                    return query.AsNoTracking().ToList();
                else
                    return query.ToList();
            }
        }

        public virtual IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>,
            IOrderedQueryable<T>> orderBy = null, string includeProperties = "", bool isTrackingOff = false)
        {
            IQueryable<T> query = _context.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                if (isTrackingOff)
                    return orderBy(query).AsNoTracking().ToList();
                else
                    return orderBy(query).ToList();
            }
            else
            {
                if (isTrackingOff)
                    return query.AsNoTracking().ToList();
                else
                    return query.ToList();
            }
        }
        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().Where(x => !x.IsDeleted).ToList();
        }


        public virtual T GetById(object id, string includeProperties = "")
        {
            if (string.IsNullOrEmpty(includeProperties))
                return _context.Set<T>().Find(id);
            else
            {
                IQueryable<T> query = _context.Set<T>();
                //query = query.Where(x => x.ID == (Guid)id);

                foreach (var includeProperty in includeProperties.Split
                    (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }

                return query.SingleOrDefault();
            }
        }

        public T GetById(int id)
        {
            return _context.Set<T>().Find(id);
        }
        public void Disable(int id)
        {
            T item = GetById(id);
            Disable(item);
        }
        public void Enable(int id)
        {
            T item = GetById(id);
            Enable(item);
        }

        public void Disable(T item)
        {
            item.IsDeleted = true;
        }
        public void Enable(T item)
        {
            item.IsDeleted = false;
        }


        public virtual void DeleteFromDb(object id)
        {
            T entityToDelete = _context.Set<T>().Find(id);
            DeleteFromDb(entityToDelete);
        }

        public virtual void DeleteFromDb(T entityToDelete)
        {
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _context.Set<T>().Attach(entityToDelete);
            }
            _context.Set<T>().Remove(entityToDelete);
        }

        public void DeleteFromDb(Expression<Func<T, bool>> filter)
        {
            _context.Set<T>().RemoveRange(_context.Set<T>().Where(filter));
        }
        public virtual int GetCount(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = _context.Set<T>();
            int count = query.Count();

            if (filter != null)
            {
                query = query.Where(filter);
                count = query.Count();
            }
            return count;
        }
    }
    public class Repository<T> : BaseRepository<T>, IRepository<T> where T : AuditableEntity
    {
        private DbContext _context;

        public Repository(DbContext context) : base(context)
        {
            _context = context;
        }

        public int GetCount(DateTime date)
        {
            return _context.Set<T>().Count(e => e.CreatedAt.Value.Year == date.Year && e.CreatedAt.Value.Month == date.Month && e.CreatedAt.Value.Day == date.Day && !e.IsDeleted);
        }
        public int GetCount(int year)
        {
            return _context.Set<T>().Count(e => e.CreatedAt.Value.Year == year && !e.IsDeleted);
        }

        //public string GenerateRequisitionNo(int? formId)
        //{
        //    string fmt = "000.##";
        //    return string.Format("{0:yyMMdd}", DateTime.Now) + formId +
        //           (GetCount(DateTime.Now.Year) + 1).ToString(fmt);
        //}
    }
}
