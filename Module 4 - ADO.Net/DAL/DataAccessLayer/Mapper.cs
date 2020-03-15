using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using DataAccessLayer.Attributes;

namespace DataAccessLayer
{
    public static class Mapper
    {
        public static T Map<T>(DataRow row)
        {
            T item = Activator.CreateInstance<T>();


            var properties = typeof(T).GetProperties()
                .Where(p => p.GetCustomAttributes(typeof(NotMapAttribute), false).Length == 0
                            &&  !p.PropertyType.IsInterface);

            foreach (var prop in properties)
            {
                var value = row[prop.Name];

                if (value == DBNull.Value)
                {
                    value = null;
                }

                prop.SetValue(item, value);
            }

            return item;
        }
    }
}