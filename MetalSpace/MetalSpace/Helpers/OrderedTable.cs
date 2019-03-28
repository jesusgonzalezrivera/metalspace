using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetalSpace.Events
{
    /// <summary>
    /// The <c>NameValuePair</c> class represents a pair of values that acts
    /// like a relation key-value.
    /// </summary>
    [System.Xml.Serialization.XmlRoot("NameValuePair", Namespace="", IsNullable=false)]
    public class NameValuePair
    {
        #region Fields

        /// <summary>
        /// Store for the Key property.
        /// </summary>
        private string _key;

        /// <summary>
        /// Store for the Value property.
        /// </summary>
        private string _value;

        #endregion

        #region Properties

        /// <summary>
        /// Key property
        /// </summary>
        /// <value>
        /// Key value.
        /// </value>
        [System.Xml.Serialization.XmlAttribute("Key", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Key
        {
            get { return _key; }
            set { _key = value; }
        }

        /// <summary>
        /// Value property
        /// </summary>
        /// <value>
        /// Value.
        /// </value>
        [System.Xml.Serialization.XmlAttribute("Value", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of the <c>NameValuePair</c> class.
        /// </summary>
        public NameValuePair() { }

        /// <summary>
        /// Constructor of the <c>NameValuePair</c> class.
        /// </summary>
        /// <param name="key">Key value.</param>
        /// <param name="value">Value.</param>
        public NameValuePair(string key, string value)
        {
            _key = key;
            _value = value;
        }

        #endregion
    }

    /// <summary>
    /// The <c>ListNameValuePair</c> class represents a list of <c>NameValuePair</c>
    /// that represents the list of subtitles used by the game.
    /// </summary>
    [Serializable]
    [System.Xml.Serialization.XmlRoot("ListNameValuePair", Namespace = "", IsNullable = false)]
    public class ListNameValuePair
    {
        #region Fields

        /// <summary>
        /// List of <c>NameValuePair</c> that contains the subtitles.
        /// </summary>
        [System.Xml.Serialization.XmlArray("Strings", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItem("NameValuePair", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public List<NameValuePair> Strings;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of the <c>ListNameValuePair</c> class.
        /// </summary>
        public ListNameValuePair() { }

        /// <summary>
        /// Constructor of the <c>ListNameValuePair</c> class.
        /// </summary>
        /// <param name="table"><c>Hashtable</c> that contains the subtitles.</param>
        public ListNameValuePair(Hashtable table)
        {
            Strings = new List<NameValuePair>();
            
            IDictionaryEnumerator enumerator = table.GetEnumerator();
            while (enumerator.MoveNext())
                this.Add((string)enumerator.Key, (string)enumerator.Value);
        }

        #endregion

        #region Add Method

        /// <summary>
        /// Add a new <c>NameValuePair</c> to the list.
        /// </summary>
        /// <param name="key">Key value.</param>
        /// <param name="value">Value.</param>
        public void Add(string key, string value)
        {
            Strings.Add(new NameValuePair(key, value));
        }

        #endregion

        #region ConvertIntoHashTable Method

        /// <summary>
        /// Convert the <c>ListNameValuePair</c> into a HashTable.
        /// </summary>
        /// <returns></returns>
        public Hashtable ConvertIntoHashTable()
        {
            Hashtable table = new Hashtable();
            foreach (NameValuePair pair in Strings)
                if (!table.ContainsKey(pair.Key))
                    table.Add(pair.Key, pair.Value);
            return table;
        }

        #endregion
    }
}
