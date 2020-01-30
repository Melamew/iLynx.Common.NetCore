using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using iLynx.Common.Threading;

namespace iLynx.PubSub
{
    public class QueuedBus<T> : Bus<T>, IDisposable
    {
        private class QueuedMessage
        {
            public QueuedMessage(Type type, object message)
            {
                Type = type;
                Message = message;
            }

            public Type Type { get; }
            public object Message { get; }
        }

        private readonly IWorker worker;
        private bool isRunning = true;
        private readonly Queue<QueuedMessage> messageQueue = new Queue<QueuedMessage>();

        public QueuedBus(IThreadManager threadManager)
        {
            var manager = threadManager ?? throw new ArgumentNullException(nameof(threadManager));
            worker = manager.StartNew(MessagePump);
        }

        private void MessagePump()
        {
            while (isRunning)
            {
                if (messageQueue.Count < 1)
                {
                    Thread.CurrentThread.Join(5);
                    continue;
                }
                var item = messageQueue.Dequeue();
                Publish(item.Type, item.Message);
            }
            PopLast();
        }

        private void PopLast()
        {
            while (messageQueue.Count > 1)
            {
                var item = messageQueue.Dequeue();
                Publish(item.Type, item.Message);
            }
        }

        public override void Publish<TMessage>(TMessage message)
        {
            messageQueue.Enqueue(new QueuedMessage(typeof(TMessage), message));
        }

        public override async Task PublishAsync<TMessage>(TMessage message)
        {
            await Task.Run(() => Publish(message));
        }

        public void Dispose()
        {
            isRunning = false;
            worker?.Wait();
        }
    }
}