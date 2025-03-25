﻿using System.Collections;
using System.Data;
using System.Data.HashFunction.MurmurHash;
using System.Reflection;
using System.Text;

namespace ComparableLibrary
{
    /// <summary>
    /// General class for getting hash from significant properties
    /// </summary>
    public class GeneralComparable
    {
        private string _hashSum;

        public string HashSum
        {
            get
            {
                if (String.IsNullOrEmpty(_hashSum))
                    _hashSum = GetHashSum();

                return _hashSum;
            }
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
        private string GetComparablePair(Type propType, string propName, object propValue, ComparableCollectionType orderType = ComparableCollectionType.Ordered)
        {
            if(propValue == null)
                return $"{propName}:";

            if(typeof(bool) == propType)
                return $"{propName}:{((bool)propValue ? "1" : "0")}";

            if(propType.IsPrimitive || propType == typeof(string) || propType == typeof(decimal))
                return $"{propName}:{propValue.ToString()}";

            if (typeof(DateTime) == propType)
                return $"{propName}:{((DateTime)propValue).Ticks}";

            if (typeof(TimeSpan) == propType)
                return $"{propName}:{((TimeSpan)propValue).Ticks}";

            if (propType.IsClass && propType.IsSubclassOf(typeof(GeneralComparable)))
                return $"{propName}:{((GeneralComparable)propValue).GetHashSum()}";

            if (propValue != null && propType.IsArray)
            {
                var elemType = propType.GetElementType();
                var list = ((Array)propValue).Cast<object>().Select(x => GetComparablePair(elemType, String.Empty, x, orderType));

                if (orderType == ComparableCollectionType.Ordered)
                    return $"{propName}:[{string.Join(",", list)}]";
                else
                    return $"{propName}:{list.Aggregate(0, (acc, val) => acc ^= val.GetHashCode())}";
            }

            if(typeof(IEnumerable).IsAssignableFrom(propType) && propType.IsGenericType)
            {
                var genType = propType.GetGenericArguments().FirstOrDefault();
                var list = ((IEnumerable)propValue).Cast<object>().Select(x => GetComparablePair(genType, String.Empty, x, orderType));

                if (orderType == ComparableCollectionType.Ordered)
                    return $"{propName}:[{string.Join(",", list)}]";
                else
                    return $"{propName}:{list.Aggregate(0, (acc, val) => acc ^= val.GetHashCode())}";
            }

            throw new InvalidOperationException($"Unsupported property type: {propType.FullName} in {propName}");
        }

        /// <summary>
        /// Get instance's hash sum
        /// </summary>
        /// <returns></returns>
        public string GetHashSum()
        {
            string hashDataLine = String.Empty;

            //Getting public or instance properties of class with comperable attribute
            var props = (this).GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                        .Where(x => x.CustomAttributes.Any(y => y.AttributeType == typeof(ComparablePropertyAttribute)))
                                        .OrderBy(x => x.GetCustomAttribute<ComparablePropertyAttribute>().Order);

            StringBuilder sb = new StringBuilder();
            if (props.Any())
            {
                //Processing of each property
                foreach (var prop in props)
                {
                    var propValue = prop.GetValue(this);
                    var propType = prop.PropertyType;
                    var attr = prop.GetCustomAttribute<ComparablePropertyAttribute>();
                    var propName = !String.IsNullOrEmpty(attr.Name) ? attr.Name : prop.Name;
                    var orderType = attr.Type;

                    sb.Append(GetComparablePair(propType, propName, propValue, orderType));
                }
            }

            var dataStr = sb.ToString();

            //Create hash sum by comperable properties using MurmurHash3
            if (!String.IsNullOrEmpty(dataStr))
            {
                var murmurHash = MurmurHash3Factory.Instance.Create();
                byte[] hashBytes = murmurHash.ComputeHash(Encoding.UTF8.GetBytes(dataStr)).Hash;

                return BitConverter.ToString(hashBytes).Replace("-", "");
            }
            else
                return dataStr;
        }
    }

    /// <summary>
    /// Attribute for comperable properties
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ComparablePropertyAttribute : Attribute
    {
        public string Name { get; set; }

        public int Order { get; set; }

        public ComparableCollectionType Type { get; set; }

        public ComparablePropertyAttribute(int order = int.MaxValue, string name = null, ComparableCollectionType type = ComparableCollectionType.Ordered)
        {
            Name = name;
            Order = order;
            Type = type;
        }
    }

    /// <summary>
    /// Comperable type for collection, collection should be compare as ordered or as unordered 
    /// </summary>
    public enum ComparableCollectionType
    {
        Ordered = 0,
        Unordered = 1
    }
}
