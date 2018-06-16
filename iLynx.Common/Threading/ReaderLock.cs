using System;
using System.Threading;

namespace iLynx.Common.Threading
{
    public class ReaderLock : IDisposable
    {
        private readonly ReaderWriterLockSlim rwl;

        public ReaderLock(ReaderWriterLockSlim rwl)
        {
            this.rwl = rwl;
            this.rwl.EnterReadLock();
        }

        public void Dispose()
        {
            rwl.ExitReadLock();
        }
    }
}