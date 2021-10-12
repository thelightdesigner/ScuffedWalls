using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ScuffedWalls
{
    class CacheableScanner<T> : IEnumerator<T>
    {
        private IEnumerable<T> _parameters;
        private IEnumerator<T> _enumerator;
        private List<T> _cache;
        public bool AnyCached => _cache.Count > 0;
        public T Current => _enumerator.Current;
        object IEnumerator.Current => _enumerator.Current;
        public bool MoveNext() => _enumerator.MoveNext();
        public void Reset() => _enumerator.Reset();
        public void Dispose() => _enumerator.Dispose();
        public List<T> Cache => _cache;

        public CacheableScanner(IEnumerable<T> items)
        {
            _parameters = items;
            _enumerator = _parameters.GetEnumerator();
            ResetCache();
        }
        
        public void ResetCache()
        {
            _cache = new List<T>();
        }
        public void AddToCache()
        {
            _cache.Add(_enumerator.Current);
        }
        public List<T> GetAndResetCache()
        {
            var cache = _cache;
            ResetCache();
            return cache;
        }

    }
}
