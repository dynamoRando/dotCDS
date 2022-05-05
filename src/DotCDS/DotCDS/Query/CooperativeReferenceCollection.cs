using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace DotCDS.Query
{
    /// <summary>
    /// A collection of <see cref="CooperativeReference"/>s in a SQL statement
    /// </summary>
    internal class CooperativeReferenceCollection : IEnumerable<CooperativeReference>
    {
        #region Private Fields
        private List<CooperativeReference> _CooperativeReferences;
        private string _databaseName;
        #endregion

        #region Public Properties
        public int Count => _CooperativeReferences.Count;
        public bool IsReadOnly => false;
        public CooperativeReference CurrentReference { get; set; }
        #endregion

        #region Constructors
        public CooperativeReferenceCollection()
        {
            _CooperativeReferences = new List<CooperativeReference>();
        }

        public CooperativeReferenceCollection(string databaseName)
        {
            _databaseName = databaseName;
            _CooperativeReferences = new List<CooperativeReference>();
        }

        public CooperativeReferenceCollection(int length)
        {
            _CooperativeReferences = new List<CooperativeReference>(length);
        }
        #endregion

        #region Public Methods
        public CooperativeReference this[int index]
        {
            get { return _CooperativeReferences[index]; }
            set { _CooperativeReferences[index] = value; }
        }

        public CooperativeReference? GetCooperativeReferenceByDatabaseAndTable(string databaseName, string tableName)
        {
            foreach (var referece in _CooperativeReferences)
            {
                if (string.Equals(referece.DatabaseName, databaseName, StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(referece.TableName, tableName, StringComparison.OrdinalIgnoreCase))
                {
                    return referece;
                }
            }

            return null;
        }

        public void Add(CooperativeReference item)
        {
            if (!Contains(item))
            {
                _CooperativeReferences.Add(item);
            }
            else
            {
                throw new InvalidOperationException(
                  $"There is already a plan with database {item.DatabaseName} and table {item.TableName} and same columns");
            }
        }

        public void Clear()
        {
            _CooperativeReferences.Clear();
        }

        public bool ContainsDatabaseAndTable(CooperativeReference item)
        {
            foreach (var reference in _CooperativeReferences)
            {
                if (string.Equals(item.DatabaseName, reference.DatabaseName, StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(item.TableName, reference.TableName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        public bool Contains(CooperativeReference item)
        {
            foreach (var reference in _CooperativeReferences)
            {
                if (reference == item)
                {
                    return true;
                }
            }

            return false;
        }


        public void CopyTo(CooperativeReference[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException("The array cannot be null.");
            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException("The starting array index cannot be negative.");
            if (Count > array.Length - arrayIndex + 1)
                throw new ArgumentException("The destination array has fewer elements than the collection.");

            for (int i = 0; i < _CooperativeReferences.Count; i++)
            {
                array[i + arrayIndex] = _CooperativeReferences[i];
            }
        }

        public IEnumerator<CooperativeReference> GetEnumerator()
        {
            return new CooperativeReferenceEnumerator(this);
        }

        public bool Remove(CooperativeReference item)
        {
            bool result = false;

            // Iterate the inner collection to
            // find the box to be removed.
            for (int i = 0; i < _CooperativeReferences.Count; i++)
            {
                CooperativeReference curReference = _CooperativeReferences[i];

                if (curReference == item)
                {
                    _CooperativeReferences.RemoveAt(i);
                    result = true;
                    break;
                }
            }
            return result;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new CooperativeReferenceEnumerator(this);
        }
        #endregion

        #region Private Methods
        #endregion
    }
}
