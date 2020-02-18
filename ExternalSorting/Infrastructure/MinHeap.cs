// https://github.com/Ra-Sedgwick/MinHeap/blob/master/MinHeap/MinHeap.cs

using System;
using System.Collections;
using System.Collections.Generic;

namespace ExternalSorting.Infrastructure
{
    public class MinHeap<T> : IEnumerable<T> where T : IComparable<T>
    {
        private List<T> _data;

        public MinHeap()
        {
            _data = new List<T>();
        }

        public void Enqueue(T item)
        {
            _data.Add(item);
            var child = _data.Count - 1;

            while (child > 0)
            {
                var parent = (child - 1) / 2;

                if (_data[child].CompareTo(_data[parent]) >= 0)
                    break;                                         

                Swap(ref _data, ref child, ref parent);
            }
        }

        public T Dequeue()
        {
            if (_data.Count == 0)
                throw new InvalidOperationException("Heap Empty");

            var last = _data.Count - 1;
            var first = _data[0];
            _data[0] = _data[last];
            _data.RemoveAt(last);
            var parent = 0;

            last--;

            while (true)
            {
                var left = parent * 2 + 1;               // Left child index of parent

                if (left > last)
                    break;

                var right = left + 1;                    // Right child index

                // If there is a right child, and it is smaller then the left child....
                if (right <= last && _data[right].CompareTo(_data[left]) < 0)
                    left = right;

                // If parent is <= to samllest child; done.
                if (_data[parent].CompareTo(_data[left]) <= 0)
                    break;

                Swap(ref _data, ref parent, ref left);
            }

            return first;
        }

        public T Peek()
        {
            if (_data.Count == 0)
                throw new InvalidOperationException("Heap Empty");

            return _data[0];
        }

        public void Clear()
        {
            _data.Clear();
        }

        public T[] ToArray()
        {
            return _data.ToArray();
        }

        public List<T> ToList()
        {
            return _data;
        }

        public int Count()
        {
            return _data.Count;
        }

        private void Swap(ref List<T> data, ref int x, ref int y)
        {
            var temp = data[x];
            data[x] = data[y];
            data[y] = temp;
            x = y;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}