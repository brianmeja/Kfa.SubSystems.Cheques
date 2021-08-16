using Aura.UI.Controls;
using Aura.UI.Controls.Primitives;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Threading;
using JetBrains.Annotations;
using Kfa.SubSystems.Cheques.Contracts;
using System;

#nullable enable

namespace Kfa.SubSystems
{
    public static partial class ContentDialogService
    {
        public static void NewMessageDialog(this WindowBase owner,
                                           object title,
                                           object content,
                                           Action<object, RoutedEventArgs>? OnClosing, [CanBeNull] IImage icon = null) => NewMessageDialog<MessageDialog>(owner, title, content, OnClosing, icon);

        public static void NewMessageDialog<TMessageDialog>(this WindowBase owner,
                                            object title,
                                            object content,
                                            Action<object, RoutedEventArgs>? OnClosing, [CanBeNull] IImage icon = null)
            where TMessageDialog : MessageDialog, new()
        {
            var m = new TMessageDialog();
            m.SetOwner(owner);

            m.Content = content;
            m.Title = title;
            //if (icon != null)
            //    m.Icon = icon;

            if (OnClosing != null)
            {
                m.Closing += (s, e) =>
                {
                    OnClosing.Invoke(s, e);
                };
            }
            m.InvalidateVisual();
            m.Show();
        }

        internal static void ShowDialogOn(Control window, Control dialog)
        {
            Dispatcher.UIThread.Post(() =>
            {
                var layer = OverlayLayer.GetOverlayLayer(window);
                dialog.Width = layer.Bounds.Width;
                dialog.Height = layer.Bounds.Height;
                window.PropertyChanged += (s, e) =>
                {
                    dialog.Width = layer.Bounds.Width;
                    dialog.Height = layer.Bounds.Height;
                };
                layer.Children.Add(dialog);
            });
        }

        internal static void CloseDialogOn(Control window, Control dialog)
        {
            Dispatcher.UIThread.Post(() =>
            {
                var layer = OverlayLayer.GetOverlayLayer(window);
                if (layer.Children.Contains(dialog))
                {
                    layer.Children.Remove(dialog);
                }
            });
        }

        public static void NewContentDialog(this WindowBase owner,
                                         object content,
                                         Action<object, RoutedEventArgs>? OnOKButtonClick,
                                         Action<object, RoutedEventArgs>? OnCancelButtonClick,
                                         object? OkButtonContent,
                                         object? CancelButtonContent)
            => NewContentDialog<ContentDialog>(
                owner,
                content,
                OnOKButtonClick,
                OnCancelButtonClick,
                OkButtonContent,
                CancelButtonContent);

        public static void NewPageContentDialog(this WindowBase owner,
                                      object content,
                                      Action<object, RoutedEventArgs>? OnOKButtonClick,
                                      object? OkButtonContent = null)
         => NewPageContentDialog<PageContentDialog>(
             owner,
             content,
             OnOKButtonClick,
             OkButtonContent);

        public static void NewPageContentDialog(this WindowBase owner,
                                    IClosablePage content)
       => NewPageContentDialog<PageContentDialog>(owner, content);

        /// <summary>
        /// Creates a new ContentDialog, you must set the window owner and the content, the rest parameters are optional, you can set them as null
        /// </summary>
        /// <param name="owner">the window owner</param>
        /// <param name="content">the ContentDialog content</param>
        /// <param name="OnOKButtonClick">This action is invoked when the Ok Button is clicked</param>
        /// <param name="OnCancelButtonClick">This action is invoked when the Cancel Button is clicked</param>
        /// <param name="OkButtonContent">The OkButton content, when is null the default content is "Ok"</param>
        /// <param name="CancelButtonContent">The CancelButton content, when is null the default content is "Cancel"</param>
        public static void NewContentDialog<TContentDialog>(this WindowBase owner,
                                         object content,
                                         Action<object, RoutedEventArgs>? OnOKButtonClick,
                                         Action<object, RoutedEventArgs>? OnCancelButtonClick,
                                         object? OkButtonContent,
                                         object? CancelButtonContent) where TContentDialog : ContentDialog, new()
        {
            var dialog = new TContentDialog();
            dialog.FontSize = 32;
            dialog.Width = Declarations.AppWidth.Value;
            dialog.Content = content;
            dialog.SetOwner(owner);

            // returns Ok when OkButton content is false or returns the custom content
            dialog.OkButtonContent = OkButtonContent == null ? "Ok" : OkButtonContent;

            // the same but with CancelButton content
            dialog.CancelButtonContent = CancelButtonContent == null ? "Cancel" : CancelButtonContent;

            // sets the actions and the closing
            dialog.OkButtonClick += (s, e) =>
            {
                if (OnOKButtonClick != null)
                {
                    OnOKButtonClick.Invoke(s, e);
                }
                dialog.Close();
            };
            dialog.CancelButtonClick += (s, e) =>
            {
                if (OnCancelButtonClick != null)
                {
                    OnCancelButtonClick.Invoke(s, e);
                }
                dialog.Close();
            };
            dialog.InvalidateVisual();
            dialog.Show();
        }

        public static void NewPageContentDialog<TContentDialog>(this WindowBase owner,
                                       object content,
                                       Action<object, RoutedEventArgs>? OnOKButtonClick,
                                        object? OkButtonContent)
                                       where TContentDialog : PageContentDialog, new()
        {
            var dialog = new TContentDialog
            {
                FontSize = 8,
                Width = Declarations.AppWidth.Value,
                Content = content
            };

            if (content is Control ctrl)
            {
                dialog.Padding = new Avalonia.Thickness(0);
                dialog.Margin = new Avalonia.Thickness(0);
                ctrl.Margin = new Avalonia.Thickness(0);
            }

            dialog.Height = Math.Max(100, Declarations.AppHeight.Value - 100);
            dialog.Width = Math.Max(100, Declarations.AppWidth.Value - 100);
            dialog.SetOwner(owner);
            //var btn = dialog.FindControl<Button>("PART_CancelButton");
            //if (btn != null)
            //    btn.IsVisible = false;

            // returns Ok when OkButton content is false or returns the custom content
            dialog.OkButtonContent = OkButtonContent == null ? "Ok" : OkButtonContent;

            // the same but with CancelButton content
            //dialog.CancelButtonContent = CancelButtonContent == null ? "Cancel" : CancelButtonContent;

            // sets the actions and the closing
            dialog.OkButtonClick += (s, e) =>
            {
                if (OnOKButtonClick != null)
                {
                    OnOKButtonClick.Invoke(s, e);
                }
                dialog.Content = null;
                dialog.Close();
            };
            dialog.InvalidateVisual();
            dialog.Show();
        }

        public static void NewPageContentDialog<TContentDialog>(this WindowBase owner,
                                     IClosablePage content)
                                     where TContentDialog : PageContentDialog, new()
        {
            var dialog = new TContentDialog
            {
                FontSize = 8,
                Width = Declarations.AppWidth.Value,
                Content = content,
                OkButtonIsVisible = false
            };

            if (content is Control ctrl)
            {
                dialog.Padding = new Avalonia.Thickness(0);
                dialog.Margin = new Avalonia.Thickness(0);
                ctrl.Margin = new Avalonia.Thickness(0);
                ctrl.MinHeight = Math.Max(100, Declarations.AppHeight.Value - 100);
                ctrl.MinWidth = Math.Max(100, Declarations.AppWidth.Value - 300);
            }

            content.Close += tt =>
            {
                dialog.Content = null;
                dialog.Close();
            };

            dialog.SetOwner(owner);
            dialog.InvalidateVisual();
            dialog.Show();
        }

        public static void NewCustomContentDialog<TContentDialogBase>(this WindowBase owner,
                                                  object content,
                                                  Action<object, RoutedEventArgs>? OnShowing,
                                                  Action<object, RoutedEventArgs>? OnClosing)
                                                  where TContentDialogBase : ContentDialogBase, new()
        {
            var c = new TContentDialogBase();
            c.SetOwner(owner);
            c.Content = content;

            if (OnShowing != null)
            {
                c.Showing += (s, e) =>
                {
                    OnShowing.Invoke(s, e);
                };
            }

            if (OnClosing != null)
            {
                c.Closing += (s, e) =>
                {
                    OnClosing.Invoke(s, e);
                };
            }
            c.InvalidateVisual();
            c.Show();
        }
    }
}