using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetalSpace.Helpers
{
    public class HashType
    {
        #region Properties

        private string _strID;
        public string StrID
        {
            get { return _strID; }
        }

        private int _id;
        public int ID
        {
            get { return _id; }
        }

        #endregion

        #region Constructor

        public HashType(string id)
        {
            _strID = id;
            _id = _strID.GetHashCode();
        }

        #endregion

        #region Comparison Operators

        public static bool operator ==(HashType x, HashType y)
        {
            return x.ID == y.ID ? true : false;
        }

        public static bool operator !=(HashType x, HashType y)
        {
            return x.ID != y.ID ? true : false;
        }

        public static bool operator <(HashType x, HashType y)
        {
            return x.ID < y.ID ? true : false;
        }

        public static bool operator >(HashType x, HashType y)
        {
            return x.ID > y.ID ? true : false;
        }

        #endregion
    }
}
