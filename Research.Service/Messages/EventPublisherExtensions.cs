using Research.Core.Domain.Messages;
using Research.Data;
using Research.Services.Events;

namespace Research.Services.Messages
{
    /// <summary>
    /// Event publisher extensions
    /// </summary>
    public static class EventPublisherExtensions
    {


        /// <summary>
        /// Entity tokens added
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <typeparam name="U">Type</typeparam>
        /// <param name="eventPublisher">Event publisher</param>
        /// <param name="entity">Entity</param>
        /// <param name="tokens">Tokens</param>
        public static void EntityTokensAdded<T, U>(this IEventPublisher eventPublisher, T entity, System.Collections.Generic.IList<U> tokens) where T : BaseEntity
        {
            eventPublisher.Publish(new EntityTokensAddedEvent<T, U>(entity, tokens));
        }

        /// <summary>
        /// Message token added
        /// </summary>
        /// <typeparam name="U">Type</typeparam>
        /// <param name="eventPublisher">Event publisher</param>
        /// <param name="message">Message</param>
        /// <param name="tokens">Tokens</param>
        public static void MessageTokensAdded<U>(this IEventPublisher eventPublisher, MessageTemplate message, System.Collections.Generic.IList<U> tokens)
        {
            eventPublisher.Publish(new MessageTokensAddedEvent<U>(message, tokens));
        }
    }
}