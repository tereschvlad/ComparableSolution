namespace ComparableLibrary
{
    /// <summary>
    /// Interface for realization geting hash sum from instance
    /// </summary>
    public interface IGeneralComparable
    {
    }

    /// <summary>
    /// Attribute for comparable properties
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ComparablePropertyAttribute : Attribute
    {
        /// <summary>
        /// Name of property
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Serial number of property
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Type of comparable collection (use only for collection ordered or unordered)
        /// </summary>
        public ComparableCollectionType Type { get; set; }

        public ComparablePropertyAttribute(int order = int.MaxValue, string name = null, ComparableCollectionType type = ComparableCollectionType.Ordered)
        {
            Name = name;
            Order = order;
            Type = type;
        }
    }

    /// <summary>
    /// Comparable type for collection, collection should be compare as ordered or as unordered 
    /// </summary>
    public enum ComparableCollectionType
    {
        Ordered = 0,
        Unordered = 1
    }
}
