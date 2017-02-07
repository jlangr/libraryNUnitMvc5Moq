using System.Collections.Generic;

namespace Library.Util
{
    public class MultiMap<TK,TV>
    {
        private readonly IDictionary<TK,IList<TV>> keys = new Dictionary<TK,IList<TV>>();

        public bool IsEmpty()
        {
            return 0 == Count();
        }

        // NEED TEST!
        public void Clear()
        {
            keys.Clear();
        }

        public int Count()
        {
            return keys.Count;
        }

        public void Add(TK key, TV value)
        {
            if (key == null)
                throw new IllegalKeyException();

            IList<TV> valuesForKey;
            if (!keys.ContainsKey(key))
            {
                valuesForKey = new List<TV>();
                keys[key] = valuesForKey;
            }
            else
                valuesForKey = keys[key];
            valuesForKey.Add(value);
        }

        public IList<TV> this[TK key]
        {
            get
            {
                if (!keys.ContainsKey(key))
                    return new List<TV>();
                return keys[key];
            }
            set
            {
                keys[key] = value;
            }
        }

        public int Count(TK key)
        {
            return this[key].Count;
        }

        public IList<TV> Values()
        {
            IList<TV> results = new List<TV>();

            ICollection<IList<TV>> lists = keys.Values;
            foreach (IList<TV> list in lists)
                foreach (TV item in list)
                    results.Add(item);
            return results;
        }
    }
}
