using System;
using System.Threading;

namespace iLynx.Common.Threading
{
    public class WriterLock : IDisposable
    {
        private readonly ReaderWriterLockSlim rwl;

        public WriterLock(ReaderWriterLockSlim rwl)
        {
            this.rwl = rwl;
            this.rwl.EnterWriteLock();
        }

        public void Dispose()
        {
            rwl.ExitWriteLock();
        }
    }
}