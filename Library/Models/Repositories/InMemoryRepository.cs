using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Library.Models.Repositories
{
    public class InMemoryRepository<T> : IRepository<T>
        where T : Identifiable
    {
        private IDictionary<int, T> entities = new Dictionary<int, T>();
        private ISet<T> modifiedEntities = new HashSet<T>();

        public ISet<T> ModifiedEntities { get { return modifiedEntities; } }

        public int Create(T entity)
        {
            entity.Id = NextId();
            entities[entity.Id] = DeepClone(entity);
            return entity.Id;
        }

        // See http://stackoverflow.com/questions/17065264/create-copy-of-object
        private T DeepClone(T original)
        {
            if (!typeof(T).IsSerializable)
                throw new ArgumentException("The type must be serializable.", "original");
            if (ReferenceEquals(original, null))
                return default(T);
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter
                { Context = new StreamingContext(StreamingContextStates.Clone) };
                formatter.Serialize(stream, original);
                stream.Position = 0;
                return (T)formatter.Deserialize(stream);
            }
        }

        private int NextId()
        {
            if (entities.Keys.Count == 0)
                return 1;
            return entities.Keys.Max() + 1;
        }

        public void Clear()
        {
            entities.Clear();
            modifiedEntities.Clear();
        }

        public void Delete(int id)
        {
            entities.Remove(id);
            // remove from modified entities?
        }

        public void Dispose()
        {
            entities.Clear();
            modifiedEntities.Clear();
        }

        public T Get(Func<T, bool> predicate)
        {
            return entities.Values.Where(predicate).First();
        }

        public IEnumerable<T> GetAll()
        {
            return entities.Values;
        }

        public T GetByID(int id)
        {
            if (!entities.ContainsKey(id))
                return default(T);
            return entities[id];
        }

        public IEnumerable<T> FindBy(Func<T, bool> predicate)
        {
            return entities.Values.Where(predicate);
        }

        public void MarkModified(T entity)
        {
            modifiedEntities.Add(entity);
        }

        public void Save(T entity)
        {
            entities[entity.Id] = DeepClone(entity);
        }

        public int Save()
        {
            foreach (var entity in modifiedEntities)
                Save(entity);
            modifiedEntities.Clear();
            return 0;
        }
    }
}