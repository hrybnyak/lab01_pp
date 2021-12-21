using System.Threading;

namespace CasSynchronization
{
    public static class Monitor
    {
        private static int _locked = 0;
        private static Thread _lockThread;
        private static bool _notify = false;
        
        public static void Lock()
        {
            while (Interlocked.CompareExchange(ref _locked, 1, 0) != 0)
            {
                Thread.Yield();
            }
            _lockThread = Thread.CurrentThread;
        }

        public static void Unlock()
        {
            _locked = 0;
            _lockThread = null;
        }

        public static void Wait()
        {
            var currentThread = Thread.CurrentThread;
            if (currentThread != _lockThread)
            {
                throw new SynchronizationLockException();
            }

            Unlock();
            while (!_notify)
            {
                Thread.Yield();
            }

            Lock();
            _notify = false;
        }

        public static void Notify()
        {
            var currentThread = Thread.CurrentThread;
            if (currentThread != _lockThread)
            {
                throw new SynchronizationLockException();
            }

            _notify = true;
        }
    }
}