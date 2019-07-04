using SCHM.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SCHM.Repo
{
    public interface IBaseRepository<T> where T : Entity
    {
        void Add(T item);
        void Update(T entityToUpdate);

        IEnumerable<T> GetAll();
        IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "", bool isTrackingOff = false);
        IEnumerable<T> GetAll(out int total, out int totalDisplay, Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "", int pageIndex = 1, int pageSize = 10, bool isTrackingOff = false);

        int GetCount(Expression<Func<T, bool>> filter = null);

        T GetById(int id);
        T GetById(object id, string includeProperties = "");
        //IEnumerable<T> GetDynamic(Expression<Func<T, bool>> filter = null, string orderBy = null, string includeProperties = "", bool isTrackingOff = false);
        //IEnumerable<T> GetDynamic(out int total, out int totalDisplay, Expression<Func<T, bool>> filter = null, string orderBy = null, string includeProperties = "", int pageIndex = 1, int pageSize = 10, bool isTrackingOff = false);
        //void Add(T entity);


        void Disable(int id);
        void Disable(T item);

        void DeleteFromDb(Expression<Func<T, bool>> filter);
        void DeleteFromDb(object id);
        void DeleteFromDb(T entityToDelete);
    }
    public interface IRepository<T> : IBaseRepository<T> where T : AuditableEntity
    {

        int GetCount(DateTime date);
        int GetCount(int year);

    }
}
