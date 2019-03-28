using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetalSpace.Events
{
    /// <summary>
    /// The <c>HashType</c> class represents a hash that contains the type of
    /// an produced event, being unique.
    /// </summary>
    public class HashType
    {
        #region Fields

        /// <summary>
        /// Store for the ID property.
        /// </summary>
        private int _id;

        /// <summary>
        /// Store for the StrID property.
        /// </summary>
        private string _strID;
        
        #endregion

        #region Properties

        /// <summary>
        /// ID property
        /// </summary>
        /// <value>
        /// Get the ID of the event.
        /// </value>
        public int ID
        {
            get { return _id; }
        }

        /// <summary>
        /// StrID property
        /// </summary>
        /// <value>
        /// Get the string ID of the event.
        /// </value>
        public string StrID
        {
            get { return _strID; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of the <c>HashType</c> class.
        /// </summary>
        /// <param name="id"></param>
        public HashType(string id)
        {
            _strID = id;
            _id = _strID.GetHashCode();
        }

        #endregion

        #region Comparison Operators

        /// <summary>
        /// Equality operator.
        /// </summary>
        /// <param name="x">First <c>HashType</c> to compare.</param>
        /// <param name="y">Second <c>HashType</c> to compare.</param>
        /// <returns></returns>
        public static bool operator ==(HashType x, HashType y)
        {
            return x.ID == y.ID ? true : false;
        }

        /// <summary>
        /// Inequality operator.
        /// </summary>
        /// <param name="x">First <c>HashType</c> to compare.</param>
        /// <param name="y">Second <c>HashType</c> to compare.</param>
        /// <returns></returns>
        public static bool operator !=(HashType x, HashType y)
        {
            return x.ID != y.ID ? true : false;
        }

        /// <summary>
        /// Less operator.
        /// </summary>
        /// <param name="x">First <c>HashType</c> to compare.</param>
        /// <param name="y">Second <c>HashType</c> to compare.</param>
        /// <returns></returns>
        public static bool operator <(HashType x, HashType y)
        {
            return x.ID < y.ID ? true : false;
        }

        /// <summary>
        /// Greater operator.
        /// </summary>
        /// <param name="x">First <c>HashType</c> to compare.</param>
        /// <param name="y">Second <c>HashType</c> to compare.</param>
        /// <returns></returns>
        public static bool operator >(HashType x, HashType y)
        {
            return x.ID > y.ID ? true : false;
        }

        #endregion
    }
}
