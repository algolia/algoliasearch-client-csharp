using System;
using System.Collections.Generic;
using System.Linq;

namespace Algolia.Search.Utils
{
    public class ArrayUtils<T>
    {
        Random _rand;
        public ArrayUtils()
        {
            _rand = new Random();
        }

        public IEnumerable<T> Shuffle(IEnumerable<T> toBeShuffled)
        {
            var randomList = new List<KeyValuePair<int, T>>();

            foreach(var el in toBeShuffled)
            {
                randomList.Add(new KeyValuePair<int, T>(_rand.Next(), el));
            }

            T[] result = new T[toBeShuffled.Count()];
            int index = 0;
            foreach (KeyValuePair<int, T> pair in randomList.OrderBy(x => x.Key))
            {
                result[index] = pair.Value;
                index++;
            }
            return result;
        }
    }
}
