using System;
using System.Collections;

namespace sabre.vobject
{
    public class Node : ICollection
    {
        /**
         * The root document.
         *
         * @var Component
         */
        protected Node root;
        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public int Count { get; }
        public bool IsSynchronized { get; }
        public object SyncRoot { get; }
    }
}