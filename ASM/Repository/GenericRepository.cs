using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using ASM.IRepository;
using ASM.Models;

namespace ASM.Repostitory
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected DMMContext db { get; set; }
        protected DbSet<T> table = null;

        public GenericRepository()
        {
            db = new DMMContext();
            table = db.Set<T>();
        }

        public GenericRepository(DMMContext db)
        {
            this.db = db;
            table = db.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            return table.ToList();
        }

        public List<T> GetAllList()
        {
            return table.ToList();
        }




        public T GetByID(long id)
        {
            return table.Find(id);
        }

        public int Create(T item)
        {
            table.Add(item);
            return db.SaveChanges();
        }


        public int Update(T item)
        {
            table.Attach(item);
            db.Entry(item).State = EntityState.Modified;
            return db.SaveChanges();
        }

        public int Delete(long id)
        {
            table.Remove(table.Find(id));
            return db.SaveChanges();
        }

        public int Delete(List<T> item)
        {
            try
            {
                table.RemoveRange(item);
                return 1;
            }
            catch
            {
                return 0;
            }

        }
    }
}
