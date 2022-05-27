using System.Collections.Generic;

namespace Assets.Scripts.Utils
{
    public class UniqueQueue<T> : Queue<T>
    {
        private readonly HashSet<T> set = new HashSet<T>();

        public new void Enqueue(T t)
        {
            if (set.Add(t))
            {
                base.Enqueue(t);
            }
        }

        public new T Dequeue()
        {
            T ret = base.Dequeue();
            set.Remove(ret);
            return ret;
        }

        public new bool Contains(T t)
        {
            return set.Contains(t);
        }
    }
}
