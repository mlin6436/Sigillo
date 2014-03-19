using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using MahApps.Metro.Controls;

namespace Sigillo.App.Common
{
    public static class ControlHelper
    {
        public static void HideTabItem<T>(this T target) where T : TabItem
        {
            if (target != null)
            {
                target.IsSelected = false;
                target.Visibility = Visibility.Collapsed;
            }
        }

        public static void ShowTabItem<T>(this T target) where T : TabItem
        {
            if (target != null)
            {
                target.IsSelected = true;
                target.Visibility = Visibility.Visible;
            }
        }

        public static void HideAllTabItems<T>(this T target) where T : TabControl
        {
            if (target != null)
            {
                foreach (TabItem item in target.Items)
                {
                    item.HideTabItem();
                }
            }
        }

        public static void ChangeFlyoutStatus<T>(this T target) where T : Flyout
        {
            if (target != null)
            {
                target.IsOpen = !target.IsOpen;
            }
        }

        public static void HideFlyout<T>(this T target) where T : Flyout
        {
            if (target != null)
            {
                target.IsOpen = false;
            }
        }

        public static void ShowFlyout<T>(this T target) where T : Flyout
        {
            if (target != null)
            {
                target.IsOpen = true;
            }
        }

        public static void ShowPanel<T>(this T target) where T : Panel
        {
            if (target != null)
            {
                target.Visibility = Visibility.Visible;
            }
        }

        public static void HidePanel<T>(this T target) where T : Panel
        {
            if (target != null)
            {
                target.Visibility = Visibility.Collapsed;
            }
        }

        public static void FadeInPanel<T>(this T target, double opacity, int milliseconds = 0) where T : Panel
        {
            if (target != null)
            {
                var storyboard = new Storyboard();
                var duration = new TimeSpan(0, 0, 0, 0, milliseconds);
                var animation = new DoubleAnimation { From = 0.0, To = opacity, Duration = new Duration(duration) };
                Storyboard.SetTargetName(animation, target.Name);
                Storyboard.SetTargetProperty(animation, new PropertyPath("Opacity", opacity));
                storyboard.Children.Add(animation);
                storyboard.FillBehavior = FillBehavior.HoldEnd;
                storyboard.Completed += (sender, args) => target.Visibility = Visibility.Visible;
                storyboard.Begin(target);
            }
        }

        public static void FadeOutPanel<T>(this T target, int milliseconds = 500) where T : Panel
        {
            if (target != null)
            {
                var storyboard = new Storyboard();
                var duration = new TimeSpan(0, 0, 0, 0, milliseconds);
                var animation = new DoubleAnimation {From = 1.0, To = 0.0, Duration = new Duration(duration)};
                Storyboard.SetTargetName(animation, target.Name);
                Storyboard.SetTargetProperty(animation, new PropertyPath("Opacity", 0));
                storyboard.Children.Add(animation);
                storyboard.FillBehavior = FillBehavior.HoldEnd;
                storyboard.Completed += (sender, args) => target.Visibility = Visibility.Collapsed;
                storyboard.Begin(target);
            }
        }
    }
}
