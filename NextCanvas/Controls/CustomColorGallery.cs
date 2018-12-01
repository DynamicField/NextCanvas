using Fluent;
using NextCanvas.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FluentMenuItem = Fluent.MenuItem;
using MenuItem = System.Windows.Controls.MenuItem;

namespace NextCanvas.Controls
{
    public class CustomColorGallery : ColorGallery
    {
        public CustomColorGallery()
        {
            Loaded += OnLoaded;
        }
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (!(GetTemplateChild("PART_MoreColors") is FluentMenuItem moreColors)) return;
            EventManager.RegisterClassHandler(moreColors.GetType(), MenuItem.ClickEvent, new RoutedEventHandler((s, args) =>
            {
                if (moreColors != s) return;
                var colorChoose = new ColorChooserWindow { Owner = Window.GetWindow(this) };
                colorChoose.ActionComplete += (o, eventArgs) =>
                {
                    var color = eventArgs.Color;
                    if (RecentColors.Contains(color))
                    {
                        RecentColors.Remove(color);
                    }

                    RecentColors.Insert(0, color);
                    if (GetTemplateChild("PART_RecentColorsListBox") is ListBox recentColors)
                    {
                        recentColors.SelectedIndex = 0;
                    }
                };
                colorChoose.ShowDialog();
                args.Handled = true;
            }));
        }
    }
}