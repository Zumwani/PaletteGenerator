﻿using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace PaletteGenerator
{

    public static class ListUtility
    {

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> list, Action<T> action)
        {
            foreach (var item in list)
                action?.Invoke(item);
            return list;
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

        public static void AddRange<T>(this IList<T> list, IEnumerable<T> items)
        {
            foreach (var item in items)
                list.Add(item);
        }

        public static void AddRange<T>(this BindingList<T> list, IEnumerable<T> items)
        {
            list.RaiseListChangedEvents = false;
            foreach (var item in items)
                list.Add(item);
            list.RaiseListChangedEvents = true;
            list.ResetBindings();
        }

    }

}
