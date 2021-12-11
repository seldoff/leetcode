using System.Collections.Generic;

namespace Leetcode
{
    // https://leetcode.com/problems/lru-cache/
    public class LRUCache
    {
        class ListItem
        {
            public ListItem? Prev { get; set; }
            public ListItem? Next { get; set; }
            public int Key { get; set; }
            public int Value { get; set; }

            public override string ToString() => $"{Key}:{Value}";
        }

        private readonly int _capacity;
        private readonly Dictionary<int, ListItem> _map;
        private ListItem? _head;
        private ListItem? _tail;

        public LRUCache(int capacity)
        {
            _capacity = capacity;
            _map = new Dictionary<int, ListItem>();
        }

        public int Get(int key)
        {
            if (_map.TryGetValue(key, out var item))
            {
                Promote(item);
                return item.Value;
            }

            return -1;
        }

        public void Put(int key, int value)
        {
            if (_map.TryGetValue(key, out var item))
            {
                item.Value = value;
                Promote(item);
            }
            else
            {
                var newItem = new ListItem
                {
                    Key = key,
                    Value = value
                };
                Prepend(newItem);
                _map[key] = newItem;
            }

            if (_map.Count > _capacity)
            {
                EvictTail();
            }
        }

        private void Prepend(ListItem newItem)
        {
            if (_head != null)
            {
                _head.Next = newItem;
                newItem.Prev = _head;
            }
            _head = newItem;
            _tail ??= newItem;
        }

        private void Promote(ListItem item)
        {
            if (_head == null)
            {
                _head = _tail = item;
                return;
            }

            if (_head == item)
            {
                return;
            }

            if (item.Prev != null)
            {
                item.Prev.Next = item.Next;
            }
            else
            {
                _tail = item.Next;
            }

            if (item.Next != null)
            {
                item.Next.Prev = item.Prev;
            }

            item.Prev = _head;
            _head.Next = item;
            _head = item;
        }

        private void EvictTail()
        {
            if (_tail == null)
            {
                return;
            }

            _map.Remove(_tail.Key);
            if (_tail.Next != null)
            {
                _tail.Next.Prev = null;
            }

            _tail = _tail.Next;
        }
    }
}
