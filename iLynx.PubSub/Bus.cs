using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace iLynx.PubSub
{
    public class Bus<T> : KeyedSubscriptionManager<Type, Delegate>, IBus<T>
    {
        public void Subscribe<TMessage>(SubscriberCallback<TMessage> subscriber) where TMessage : T
        {
            Subscribe(typeof(TMessage), subscriber);
        }

        public virtual void Unsubscribe<TMessage>(SubscriberCallback<TMessage> subscriber) where TMessage : T
        {
            Unsubscribe(typeof(TMessage), subscriber);
        }

        public virtual void Publish<TMessage>(TMessage message) where TMessage : T
        {
            Publish(typeof(TMessage), message);
        }

        protected virtual void Publish(Type messageType, object message)
        {
            var subscribers = GetSubscribers(messageType);
            if (null == subscribers) return;
            foreach (var subscriber in subscribers.Where(x => null != x))
            {
                try
                {
                    subscriber.DynamicInvoke(message);
                }
                catch (Exception e)
                {
                    Trace.WriteLine(
                        $"Unable to send {message} to {subscriber}, got Exception {e}", "Error");
                }
            }
        }

        private static void Publish<TMessage>(TMessage message, IEnumerable<SubscriberCallback<TMessage>> subscribers)
        {
            foreach (var subscriber in subscribers)
            {
                try
                {
                    subscriber.Invoke(message);
                }
                catch (Exception e)
                {
                    Trace.WriteLine($"Unable to send {message} to {subscriber}, got Exception {e}", "Error");
                }
            }
        }

        public virtual async Task PublishAsync<TMessage>(TMessage message) where TMessage : T
        {
            var type = typeof(TMessage);
            var subscribers = await GetSubscribersAsync(type);
            await Task.Run(() => Publish(message, subscribers.Cast<SubscriberCallback<TMessage>>()));
        }
    }
}
