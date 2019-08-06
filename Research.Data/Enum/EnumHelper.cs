using System;
using System.Linq;
using System.Runtime.Serialization;

namespace Research.Enum
{
    public static class EnumHelper
    {
        /// <summary>
        /// Gets the type of the attribute of.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumVal">The enum value.</param>
        /// <returns></returns>
        public static T GetAttributeOfType<T>(this System.Enum enumVal) where T : System.Attribute
        {
            var type = enumVal.GetType();
            var memInfo = type.GetMember(enumVal.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
            return (attributes.Length > 0) ? (T)attributes[0] : null;
        }

        public static string ToEnumString<T>(T type)
        {
            var enumType = typeof(T);
            var name = System.Enum.GetName(enumType, type);
            var enumMemberAttribute = ((EnumMemberAttribute[])enumType.GetField(name).GetCustomAttributes(typeof(EnumMemberAttribute), true)).Single();
            return enumMemberAttribute.Value;
        }

        public static T ToEnum<T>(string str)
        {
            var enumType = typeof(T);
            foreach (var name in System.Enum.GetNames(enumType))
            {
                var enumMemberAttribute = ((EnumMemberAttribute[])enumType.GetField(name).GetCustomAttributes(typeof(EnumMemberAttribute), true)).Single();
                if (enumMemberAttribute.Value == str) return (T)System.Enum.Parse(enumType, name);
            }
            //throw exception or whatever handling you want or
            return default(T);
        }
    }
}
