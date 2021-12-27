using System;
using System.Threading;
using HarrisAlgorithm;

namespace LockFreeSkipList.Atomic
{
    public class AtomicReference<T> where T : class
    {
        public AtomicReference(T initialValue)
        {
            _value = initialValue;
        }

        private volatile T _value;
        
        public T Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public void LazySet(T newValue)
        {
            Interlocked.Exchange(ref _value, newValue);
        }

        public bool CompareAndSet(T expected, T newValue)
        {
            return Interlocked.CompareExchange(ref _value, newValue, expected) == expected;
        }
        
        public T GetAndSet(T newValue)
        {
            return Interlocked.Exchange(ref _value, newValue);
        }
        
        public override string ToString()
        {
            return $"AtomicReference<{typeof(T)}>";
        }
    }
}