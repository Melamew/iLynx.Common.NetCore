using System;
using System.Threading;

namespace iLynx.Common.Threading
{
    public class DisposableMonitor : IDisposable
    {
        private readonly object target;

        public DisposableMonitor(object target)
        {
            this.target = target;
            Monitor.Enter(this.target);
        }

        public void Dispose()
        {
            Monitor.Exit(target);
        }
    }
}