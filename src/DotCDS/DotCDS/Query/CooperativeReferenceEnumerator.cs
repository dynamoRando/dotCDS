using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace DotCDS.Query
{

    class CooperativeReferenceEnumerator : IEnumerator<CooperativeReference>
    {
        private CooperativeReferenceCollection _reference;
        private int _index;
        private CooperativeReference _current;

        public CooperativeReference Current => _current;
        object IEnumerator.Current => Current;

        public CooperativeReferenceEnumerator(CooperativeReferenceCollection collection)
        {
            _reference = collection;
            _index = -1;
            _current = default(CooperativeReference);
        }

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            //Avoids going beyond the end of the collection.
            if (++_index >= _reference.Count)
            {
                return false;
            }
            else
            {
                // Set current box to next item in collection.
                _current = _reference[_index];
            }
            return true;
        }

        public void Reset()
        {
            _index = -1;
        }

        public IEnumerator<CooperativeReference> GetEnumerator()
        {
            throw new NotImplementedException();
        }

    }

}
