using System.Collections;
using System.Data.HashFunction.MurmurHash;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace ComparableLibrary.Utils
{
    /// <summary>
    /// Extensions for IGeneralComparable interface for get hash sum from instance
    /// </summary>
    public static class GeneralComparableExtensions
    {
        /// <summary>
        /// Get instance's hash sum
        /// </summary>
        /// <param name="generalComparable">Instance which inheritance IGeneralComparable</param>
        /// <returns></returns>
        public static string GetHashSum(this IGeneralComparable generalComparable)
        {
            string hashDataLine = String.Empty;

            //Getting public or instance properties of class with comparable attribute
            var props = (generalComparable).GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                        .Where(x => x.CustomAttributes.Any(y => y.AttributeType == typeof(ComparablePropertyAttribute)))
                                        .OrderBy(x => x.GetCustomAttribute<ComparablePropertyAttribute>().Order);

            StringBuilder sb = new StringBuilder();
            if (props.Any())
            {
                //Processing of each property
                foreach (var prop in props)
                {
                    var propValue = prop.GetValue(generalComparable);
                    var propType = prop.PropertyType;
                    var attr = prop.GetCustomAttribute<ComparablePropertyAttribute>();
                    var propName = !String.IsNullOrEmpty(attr.Name) ? attr.Name : prop.Name;
                    var orderType = attr.Type;

                    //Get main type from nullable type
                    propType = Nullable.GetUnderlyingType(propType) ?? propType;

                    //Construct string for hashe from each props
                    sb.Append(GetComparablePair(propType, propName, propValue, orderType));
                }
            }

            var dataStr = sb.ToString();

            //Create hash sum by comparable properties using MurmurHash3
            if (!String.IsNullOrEmpty(dataStr))
            {
                var murmurHash = MurmurHash3Factory.Instance.Create();
                byte[] hashBytes = murmurHash.ComputeHash(Encoding.UTF8.GetBytes(dataStr)).Hash;

                return BitConverter.ToString(hashBytes).Replace("-", "");
            }
            else
                return null;
        }

        /// <summary>
        /// Get pair, name of property and value, for constructing hash
        /// </summary>
        /// <param name="propType">Property type</param>
        /// <param name="propName">Name of property</param>
        /// <param name="propValue">Value of property</param>
        /// <param name="orderType">Type of comparing (using only for collections)</param>
        /// <returns>Return pair name-value as string</returns>
        /// <exception cref="Exception"></exception>
        private static string GetComparablePair(Type propType, string propName, object propValue, ComparableCollectionType orderType = ComparableCollectionType.Ordered)
        {
            if (propValue == null)
                return $"{propName}";

            if (propType == typeof(bool))
                return $"{propName}:{((bool)propValue ? "1" : "0")}";

            if (propType.IsPrimitive || propType == typeof(string) || propType == typeof(decimal))
            {
                if (propType == typeof(double))
                    return $"{propName}:{((double)propValue).ToString("R", CultureInfo.InvariantCulture)}";
                else if (propType == typeof(float))
                    return $"{propName}:{((float)propValue).ToString("R", CultureInfo.InvariantCulture)}";
                else if (propType == typeof(decimal))
                    return $"{propName}:{((decimal)propValue).ToString(CultureInfo.InvariantCulture)}";
                else
                    return $"{propName}:{propValue.ToString()}";
            }

            if (typeof(DateTime) == propType)
                return $"{propName}:{((DateTime)propValue).Ticks}";

            if (typeof(TimeSpan) == propType)
                return $"{propName}:{((TimeSpan)propValue).Ticks}";

            if (propType.IsClass && propType.IsSubclassOf(typeof(GeneralComparable)))
                return $"{propName}:{((GeneralComparable)propValue).GetHashSum()}";

            if (propType.GetInterface((typeof(IGeneralComparable)).FullName) != null)
                return $"{propName}:{((IGeneralComparable)propValue).GetHashSum()}";

            if (propValue != null && propType.IsArray)
            {
                var elemType = propType.GetElementType();
                var list = ((Array)propValue).Cast<object>().Select(x => GetComparablePair(elemType, String.Empty, x, orderType));

                if (orderType == ComparableCollectionType.Unordered)
                    list = list.OrderBy(x => x);

                return $"{propName}:[{String.Join(",", list)}]";
            }

            if (typeof(IEnumerable).IsAssignableFrom(propType) && propType.IsGenericType)
            {
                var genType = propType.GetGenericArguments().FirstOrDefault();
                var list = ((IEnumerable)propValue).Cast<object>().Select(x => GetComparablePair(genType, String.Empty, x, orderType));

                if (orderType == ComparableCollectionType.Unordered)
                    list = list.OrderBy(x => x);

                return $"{propName}:[{String.Join(",", list)}]";
            }

            throw new InvalidOperationException($"Unsupported property type: {propType.FullName} in {propName}");
        }
    }
}
