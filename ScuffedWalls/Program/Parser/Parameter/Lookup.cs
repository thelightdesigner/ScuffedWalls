using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ScuffedWalls
{
    public class Lookup<T> : IEnumerable<T>
    {
        public Func<T, string> Exposer => _exposer;

        private readonly Func<T, string> _exposer;
        private readonly Dictionary<string, T> _dict;

        public Lookup(Func<T, string> Exposer)
        {
            _dict = new Dictionary<string, T>();
            _exposer = Exposer;
        }
        public Lookup(IEnumerable<T> collection, Func<T, string> Exposer)
        {
            _dict = collection.ToDictionary(Exposer);
            _exposer = Exposer;
        }
        public void Clear()
        {
            _dict.Clear();
        }
        public void Add(T value)
        {
            _dict.Add(_exposer(value), value);
        }
        public void Remove(string key)
        {
            _dict.Remove(key);
        }
        public void Remove(T Item)
        {
            _dict.Remove(_exposer(Item));
        }
        public T Get(string key) => _dict[key];

        public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)_dict).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();


    }
}
