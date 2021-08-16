using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Xaml.Interactivity;
using Kfa.SubSystems.Cheques.Contracts.Messaging;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kfa.SubSystems.Cheques.Contracts.Behaviors
{
    /// <summary>
    /// Toggles <see cref="TreeViewItem.IsExpanded"/> property of the associated <see cref="TreeViewItem"/> control on <see cref="InputElement.DoubleTapped"/> event.
    /// </summary>
    public sealed class TextBlockMessageBehavior : Behavior<TextBlock>
    {
        private SubscriptionToken MessageSubscriptionToken;

        /// <summary>
        /// Called after the behavior is attached to the <see cref="Behavior.AssociatedObject"/>.
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();
            if (AssociatedObject != null)
            {
                AssociatedObject.Tapped += AssociatedObject_Tapped;
                AssociatedObject.LostFocus += AssociatedObject_LostFocus;
                AssociatedObject.PointerPressed += AssociatedObject_PointerPressed;

                if (!HasCustomMessage)
                {
                    MessageSubscriptionToken = Declarations.EventAggregator
                        .GetEvent<MessageEvent>()
                        .Subscribe(GetMessage);
                }
            }
        }

        public static readonly StyledProperty<bool> HasCustomMessageProperty =
           AvaloniaProperty.Register<TextBlockMessageBehavior, bool>(nameof(HasCustomMessage), false);

        public bool HasCustomMessage
        {
            get { return GetValue(HasCustomMessageProperty); }
            set { SetValue(HasCustomMessageProperty, value); }
        }

        private void GetMessage(NotificationMessage obj)
        {
            try
            {
                Functions.RunOnMain(() =>
                {
                    if (AssociatedObject != null)
                    {
                        if (!obj.IsActive)
                        {
                            AssociatedObject.Text = null;
                        }
                        else
                        {
                            AssociatedObject.Opacity = 1;
                            AssociatedObject.IsVisible = true;
                            if (obj.MessageType == MessageTypes.Error || obj.MessageType == MessageTypes.ValidationError)
                                AssociatedObject.Foreground = Brushes.PaleVioletRed;
                            else if (obj.MessageType == MessageTypes.Warning)
                                AssociatedObject.Foreground = Brushes.OrangeRed;
                            else AssociatedObject.Foreground = Brushes.LightPink;
                            string[] data = { obj.Message, obj.SubMessage, obj.Narration };
                            AssociatedObject.Text = string.Join("\r\n", data.Distinct());
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Called when the behavior is being detached from its <see cref="Behavior.AssociatedObject"/>.
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();
            if (AssociatedObject != null)
            {
                AssociatedObject.Tapped -= AssociatedObject_Tapped;
                AssociatedObject.LostFocus -= AssociatedObject_LostFocus;
                AssociatedObject.PointerPressed -= AssociatedObject_PointerPressed;
            }
            MessageSubscriptionToken?.Dispose();
        }

        private void AssociatedObject_PointerPressed(object sender, PointerPressedEventArgs e)
        {
            try
            {
                if (AssociatedObject != null)
                {
                    AssociatedObject.Text = "";
                }
            }
            catch { }
        }

        private void AssociatedObject_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (AssociatedObject != null)
                {
                    AssociatedObject.Text = "";
                }
            }
            catch { }
        }

        private void AssociatedObject_Tapped(object sender, RoutedEventArgs e)
        {
            try
            {
                if (AssociatedObject != null)
                {
                    AssociatedObject.Text = "";
                }
            }
            catch { }
        }

        private void LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (AssociatedObject != null)
                {
                    AssociatedObject.Text = "";
                }
            }
            catch { }
        }
    }
}