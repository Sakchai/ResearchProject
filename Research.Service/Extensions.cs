using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Research.Core;
using Research.Data;
using Research.Enum;
using Research.Infrastructure;

namespace Research.Services
{
    /// <summary>
    /// Extensions
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Convert to select list
        /// </summary>
        /// <typeparam name="TEnum">Enum type</typeparam>
        /// <param name="enumObj">Enum</param>
        /// <param name="markCurrentAsSelected">Mark current value as selected</param>
        /// <param name="valuesToExclude">Values to exclude</param>
        /// <param name="useLocalization">Localize</param>
        /// <returns>SelectList</returns>
        public static SelectList ToSelectList<TEnum>(this TEnum enumObj,
           bool markCurrentAsSelected = true, int[] valuesToExclude = null, bool useLocalization = true) where TEnum : struct
        {
            if (!typeof(TEnum).IsEnum) throw new ArgumentException("An Enumeration type is required.", "enumObj");

 
            var values = from TEnum enumValue in System.Enum.GetValues(typeof(TEnum))
                         where valuesToExclude == null || !valuesToExclude.Contains(Convert.ToInt32(enumValue))
                         select new { ID = Convert.ToInt32(enumValue),
                                      Name = StringValue((System.Enum)System.Enum.Parse(typeof(TEnum), enumValue.ToString())) };


            object selectedValue = null;
            if (markCurrentAsSelected)
                selectedValue = Convert.ToInt32(enumObj);
            return new SelectList(values, "ID", "Name", selectedValue);
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

        /// <summary>
        /// Convert to select list
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="objList">List of objects</param>
        /// <param name="selector">Selector for name</param>
        /// <returns>SelectList</returns>
        public static SelectList ToSelectList<T>(this T objList, Func<BaseEntity, string> selector) where T : IEnumerable<BaseEntity>
        {
            return new SelectList(objList.Select(p => new { ID = p.Id, Name = selector(p) }), "ID", "Name");
        }
    }
}
