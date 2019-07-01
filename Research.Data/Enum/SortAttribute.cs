using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Research.Enum
{
    // <summary>
    /// Attribute for enums to specify member sort order.
    /// </summary>
    public class SortAttribute : Attribute
    {

        public int Order { get; set; }
        public SortAttribute(int order)
        {
            this.Order = order;
        }

        public static SortAttribute GetSortOrder(System.Enum e)
        {
            return (SortAttribute)Attribute.GetCustomAttribute(e.GetType().GetField(System.Enum.GetName(e.GetType(), e)), typeof(SortAttribute));
        }

        /// <summary>
        /// Sort an Enum whose members have the Sort attribute.
        /// </summary>
        /// <typeparam name="T">Enum type to sort.</typeparam>
        /// <returns>Sorted Enumerable of Enum values.</returns>
        public static IEnumerable<T> SortEnum<T>() where T : struct, IComparable, IConvertible
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("T must be an enum");

            Dictionary<int, T> orderHash = new Dictionary<int, T>();
            foreach (T value in System.Enum.GetValues(typeof(T)))
            {
                orderHash[SortAttribute.GetSortOrder(value as System.Enum).Order] = value;
            }

            return orderHash.OrderBy(pair => pair.Key).Select(pair => pair.Value);

        }

    }



}
