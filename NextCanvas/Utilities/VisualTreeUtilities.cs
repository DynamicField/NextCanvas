using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace NextCanvas.Utilities
{
    public static class VisualTreeUtilities
    {
        public static T FindVisualChild<T>(DependencyObject obj)
            where T : DependencyObject
        {
            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                var child = VisualTreeHelper.GetChild(obj, i);
                if (child is T variable)
                    return variable;
                var childOfChild = FindVisualChild<T>(child);
                if (childOfChild == null) continue;
                return childOfChild;
            }
            return null;
        }
    }
}
