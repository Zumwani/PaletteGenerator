using System;
using System.Collections.Generic;

namespace PaletteGenerator
{
    public static class ListUtility
    {

        public static void ForEach<T>(this IList<T> list, Action<T> action)
        {
            if (list == null) return;
            foreach (var item in list)
                action?.Invoke(item);
        }

        public static void ForEach(this int count, Action<int> action)
        {
            for (int i = 0; i < count; i++)
                action?.Invoke(i);
        }

        public static T[] Select<T>(this int count, Func<int, T> action)
        {
            if (action == null)
                return Array.Empty<T>();
            var l = new List<T>();
            for (int i = 0; i < count; i++)
                if (action.Invoke(i) is T t)
                    l.Add(t);
            return l.ToArray();
        }

        public static void AddRange<T>(this IList<T> list, params T[] items)
        {
            foreach (var item in items)
                list.Add(item);
        }

    }

}
