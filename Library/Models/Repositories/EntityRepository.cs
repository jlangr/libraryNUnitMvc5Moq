using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Library.Models.Repositories
{
    public class EntityRepository<T> : IRepository<T>
        where T: class, Identifiable
    {
        protected LibraryContext db = new LibraryContext();

        public IEnumerable<T> FindBy(Func<T, bool> predicate)
        {
            return dbSetFunc(db).Where(predicate);
        }

        protected Func<LibraryContext, DbSet<T>> dbSetFunc;

        public EntityRepository(Func<LibraryContext,DbSet<T>> dbSetFunc)
        {
            this.dbSetFunc = dbSetFunc;
        }

        public LibraryContext Db {  get { return db;  } }

        public void Clear()
        {
            dbSetFunc(db).RemoveRange(dbSetFunc(db));
        }

        virtual public T GetByID(int id)
        {
            return dbSetFunc(db).FirstOrDefault(p => p.Id == id);
        }

        virtual public IEnumerable<T> GetAll()
        {
            return dbSetFunc(db).ToList();
        }

        public int Create(T entity)
        {
            dbSetFunc(db).Add(entity);
            db.SaveChanges();
            return entity.Id;
        }

        public void Delete(int id)
        {
            var entity = GetByID(id);
            dbSetFunc(db).Remove(entity);
            db.SaveChanges();
        }

        public int Save()
        {
            return db.SaveChanges();
        }

        public void MarkModified(T entity)
        {
            db.Entry(entity).State = EntityState.Modified;
        }

        public void Save(T entity)
        {
            MarkModified(entity);
            Save();
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}