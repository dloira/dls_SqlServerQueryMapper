using dls_SqlServerQueryMapper.Common.Enum;
using dls_SqlServerQueryMapper.Model;
using dls_SqlServerQueryMapper.Model.Parameter;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace dls_SqlServerQueryMapper.Common.Utils
{
    /// <summary>
    /// Mapping utils
    /// </summary>
    public static class MapUtil
    {
        /// <summary>
        /// Universal date format
        /// </summary>
        public const string DATE_FORMAT = "yyyy-MM-dd";

        /// <summary>
        /// Universal datetime format
        /// </summary>
        public const string DATE_TIME_FORMAT = "yyyy-MM-dd HH:mm:ss.FFFFFF";

        /// <summary>
        /// Returns the DA data type enum for specified class type  
        /// </summary>
        /// <param name="type">class type</param>
        /// <returns></returns>
        public static DataType ToDAType(Type type)
        {
            DataType result = DataType.Unknow;

            // Types for integer
            var integerTypes = new List<Type>()
            {
                typeof(byte), typeof(sbyte), typeof(ushort),typeof(uint), typeof(ulong),
                typeof(short),typeof(int), typeof(long)
            };

            // Types for decimal
            var decimalTypes = new List<Type>()
            {
                typeof(decimal), typeof(float), typeof(double)
            };

            if (type != null)
            {
                if (type.Equals(typeof(string)))
                {
                    result = DataType.String;
                }
                else if (integerTypes.Contains(type))
                {
                    result = DataType.Integer;
                }
                else if (decimalTypes.Contains(type))
                {
                    result = DataType.Decimal;
                }
                else if (type.Equals(typeof(DateTime)))
                {
                    result = DataType.Timestamp;
                }
                else if (type.Equals(typeof(bool)))
                {
                    result = DataType.Boolean;
                }
            }
            return result;
        }

        /// <summary>
        /// Returns datatime value of specified date object. If it's a string, tries parse it using datetime format. Otherwise, try to cast directly.
        /// </summary>
        /// <param name="dateObject">object date to cast</param>
        /// <returns></returns>
        public static DateTime ToDateTime(object dateObject)
        {
            try
            {
                if (dateObject == null)
                    throw new Exception($"date object is null!!!");

                if (dateObject.GetType().Equals(typeof(string)))
                {   
                    // For strings, try parse for universal formats (date or datetime)
                    var dateString = (string)dateObject;
                    if (DateTime.TryParseExact(dateString, DATE_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
                        return result;

                    if (DateTime.TryParseExact(dateString, DATE_TIME_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
                        return result;

                    throw new Exception($"Couldn't parse '{dateObject}' string to datetime. You must use '{DATE_FORMAT}' or '{DATE_TIME_FORMAT}' format.");
                }

                // Try to parse directly
                return Convert.ToDateTime(dateObject);
            }
            catch (Exception e)
            {
                string description = $"Exception on {typeof(MapUtil).GetType()}.ToDateTime() method: {e.Message}";
                throw new Exception(description, e);
            }
        }

        /// <summary>
        /// Returns long value for specified object value.
        /// </summary>
        /// <param name="longObject">object value</param>
        /// <returns></returns>
        public static long ToLong(object longObject)
        {
            try
            {
                if (longObject == null)
                    throw new Exception("long object is null!!!");

                if (longObject.GetType().Equals(typeof(string)))
                {   
                    // When value is string, try to parse it 
                    var longString = (string)longObject;
                    if (long.TryParse(longString, NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out long result))
                        return result;

                    throw new Exception($"couldn't parse '{longObject}' string to long; you must to specified a number with optional leading sign and no more (no spaces, thousand or decimals points)");
                }

                // Try to cast directly
                return Convert.ToInt64(longObject);
            }
            catch (Exception e)
            {
                string description = $"Exception on {typeof(MapUtil).GetType().Name}.ToLong() method: {e.Message}";
                throw new Exception(description, e);
            }
        }

        /// <summary>
        /// Returns decimal value for specified object value.
        /// </summary>
        /// <param name="decimalObject">object value</param>
        /// <returns></returns>
        public static decimal ToDecimal(object decimalObject)
        {
            try
            {
                if (decimalObject == null)
                    throw new Exception("decimal object is null!!!");

                if (decimalObject.GetType().Equals(typeof(string)))
                {   
                    // When value is string, try to parse it 
                    var decimalString = (string)decimalObject;
                    if (decimal.TryParse(decimalString, NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal result))
                        return result;

                    throw new Exception($"couldn't parse '{decimalObject}' string to decimal; you must to specified a number with optional leading sign, decimal point and no more (no spaces or thousand points)");
                }

                // Try to cast directly
                return Convert.ToDecimal(decimalObject);
            }
            catch (Exception e)
            {
                string description = $"Exception on {typeof(MapUtil).GetType().Name}.ToDecimal() method: {e.Message}";
                throw new Exception(description, e);
            }
        }

        /// <summary>
        /// Returns long value for specified object value.
        /// </summary>
        /// <param name="boolObject">object value</param>
        /// <returns></returns>
        public static bool ToBool(object boolObject)
        {
            try
            {
                if (boolObject == null)
                    throw new Exception("bool object is null!!!");

                if (boolObject.GetType().Equals(typeof(string)))
                {   
                    // When value is string, try to parse it 
                    var booleanString = (string)boolObject;
                    if (bool.TryParse(booleanString, out bool result))
                        return result;

                    throw new Exception($"couldn't parse '{boolObject}' string to bool");
                }

                // Try to cast directly
                return Convert.ToBoolean(boolObject);
            }
            catch (Exception e)
            {
                string description = $"Exception on {typeof(MapUtil).GetType().Name}.ToBool() method: {e.Message}";
                throw new Exception(description, e);
            }
        }
    }
}
