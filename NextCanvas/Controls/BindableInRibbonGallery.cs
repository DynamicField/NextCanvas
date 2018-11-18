using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using Fluent;

namespace NextCanvas.Controls
{
    public class BindableInRibbonGallery : InRibbonGallery
    {
        public BindableInRibbonGallery()
        {
            
        }

        public static readonly DependencyProperty MaxItemsInRowFixProperty = DependencyProperty.Register(
            "MaxItemsInRowFix", typeof(int), typeof(BindableInRibbonGallery), new PropertyMetadata(8, MaxItemsChanged));

        private static void MaxItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var gallery = (BindableInRibbonGallery) d;
            var panel = gallery.GetTemplateChild("PART_GalleryPanel") as GalleryPanel;
            if (panel == null) return;
            gallery.MaxItemsInRow = (int) e.NewValue;
            panel.MaxItemsInRow = (int) e.NewValue;
        }

        public int MaxItemsInRowFix
        {
            get { return (int) GetValue(MaxItemsInRowFixProperty); }
            set { SetValue(MaxItemsInRowFixProperty, value); }
        }
    }
}
