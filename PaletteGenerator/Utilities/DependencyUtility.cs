using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace PaletteGenerator.Utilities
{

    static class DependencyUtility
    {

        static IEnumerable<T> FindVisualChildren<T>(this DependencyObject depObj) where T : DependencyObject
        {

            if (depObj == null)
                yield break;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {

                DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                if (child != null && child is T)
                    yield return (T)child;

                foreach (T childOfChild in FindVisualChildren<T>(child))
                    yield return childOfChild;

            }

        }

    }

}
