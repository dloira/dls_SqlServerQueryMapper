using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace dls_SqlServerQueryMapper.Common.Utils
{
    /// <summary>
    /// Mapping data utils
    /// </summary>
    public static class DataMapUtil
    {
        /// <summary>
        /// Returns a generic list of objects from DataTable
        /// </summary>
        /// <param name="table">data from databasae</param>
        /// <returns></returns>
        public static List<T> ToList<T>(DataTable table) where T : class, new()
        {
            try
            {
                List<T> list = new List<T>();

                foreach (var row in table.AsEnumerable())
                {
                    T obj = new T();

                    foreach (var prop in obj.GetType().GetProperties())
                    {
                        try
                        {

                            PropertyInfo propertyInfo = obj.GetType().GetProperty(prop.Name);
                            propertyInfo.SetValue(obj, Convert.ChangeType(row[prop.Name], propertyInfo.PropertyType), null);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    list.Add(obj);
                }

                return list;
            }
            catch
            {
                return new List<T>();
            }
        }
    }
}
