using ComparableLibrary.Utils;

namespace ComparableLibrary
{
    /// <summary>
    /// General class for getting hash from significant properties
    /// </summary>
    public class GeneralComparable : IGeneralComparable
    {
        private string _hashSum;

        public string HashSum
        {
            get
            {
                if (String.IsNullOrEmpty(_hashSum))
                    _hashSum = this.GetHashSum();

                return _hashSum;
            }
        }
    }
}
