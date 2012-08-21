using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.Windows.Markup;

namespace MusicLibrary.View.Controls
{
    [ContentProperty("BubblesBrush")]
    public class MetroLoading : Control
    {
        static MetroLoading()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MetroLoading), new FrameworkPropertyMetadata(typeof(MetroLoading)));
        }

        public Brush BubblesBrush
        {
            get { return (Brush)GetValue(BubblesBrushProperty); }
            set { SetValue(BubblesBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BubblesBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BubblesBrushProperty =
            DependencyProperty.Register("BubblesBrush", typeof(Brush), typeof(MetroLoading), new UIPropertyMetadata(null));



        public bool Animating
        {
            get { return (bool)GetValue(AnimatingProperty); }
            set { SetValue(AnimatingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Animating.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AnimatingProperty =
            DependencyProperty.Register("Animating", typeof(bool), typeof(MetroLoading), new UIPropertyMetadata(false));



        public double BubbleSize
        {
            get { return (double)GetValue(BubbleSizeProperty); }
            set { SetValue(BubbleSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BubbleSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BubbleSizeProperty =
            DependencyProperty.Register("BubbleSize", typeof(double), typeof(MetroLoading), new UIPropertyMetadata(5d));
    }
}
