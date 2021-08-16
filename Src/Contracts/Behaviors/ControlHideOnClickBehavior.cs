using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;
using Kfa.SubSystems.Cheques.Contracts.Messaging;
using Prism.Events;

namespace Kfa.SubSystems.Cheques.Contracts.Behaviors
{
    /// <summary>
    /// Toggles <see cref="TreeViewItem.IsExpanded"/> property of the associated <see cref="TreeViewItem"/> control on <see cref="InputElement.DoubleTapped"/> event.
    /// </summary>
    public sealed class ControlHideOnClickBehavior : Behavior<Control>
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
            }
        }

        public static readonly StyledProperty<bool> HasCustomMessageProperty =
           AvaloniaProperty.Register<ControlHideOnClickBehavior, bool>(nameof(HasCustomMessage), false);

        public bool HasCustomMessage
        {
            get { return GetValue(HasCustomMessageProperty); }
            set { SetValue(HasCustomMessageProperty, value); }
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
                    AssociatedObject.IsVisible = false;
                    Notifier.LongProcessDeNotify();
                    //AssociatedObject.Text = "";
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
                    // AssociatedObject.Text = "";
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
                    AssociatedObject.IsVisible = false;
                    Notifier.LongProcessDeNotify();
                    //AssociatedObject.Text = "";
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
                    //AssociatedObject.Text = "";
                }
            }
            catch { }
        }
    }
}