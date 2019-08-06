using AutoMapper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace Research.Enum
{
    public class EnumConverter

    {

        /// <summary>
        /// Converts a an Enum value to it's string.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return StringValue(value as System.Enum);
        }

        /// <summary>
        /// Converts an Enum's string value to the Enum value.
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return EnumValue(value as string, targetType);
        }

        private static string StringValue(System.Enum value)
        {

            if (value == null)
                return "";

            FieldInfo fi = value.GetType().GetField(value.ToString());
            EnumMemberAttribute[] attributes = (EnumMemberAttribute[])fi.GetCustomAttributes(typeof(EnumMemberAttribute), false);
            if (attributes.Length > 0)
            {
                return attributes[0].Value;
            }
            else
            {
                return value.ToString();
            }
        }

        private static object EnumValue(string value, Type enumType)
        {
            string[] names = System.Enum.GetNames(enumType);
            foreach (string name in names)
            {
                if (StringValue((System.Enum)System.Enum.Parse(enumType, name)).Equals(value))
                {
                    return System.Enum.Parse(enumType, name);
                }
            }

            throw new ArgumentException("The string is not a description or value of the specified enum.");

        }

    }

}
