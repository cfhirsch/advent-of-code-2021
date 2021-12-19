using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2021
{
    public class Deque<T>
    {
        private List<T> items;

        public Deque()
        {
            this.items = new List<T>();
        }

        public bool Empty {  get { return this.items.Count == 0; } }

        public T PopFront()
        {
            T item = items[0];
            items.RemoveAt(0);
            return item;
        }

        public void PushBack(T item)
        {
            this.items.Add(item);
        }


    }
}
