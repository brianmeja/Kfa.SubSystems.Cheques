using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;
using System.Linq;

namespace Kfa.SubSystems.Cheques.Contracts.Behaviors
{
    /// <summary>
    /// Toggles <see cref="TreeViewItem.IsExpanded"/> property of the associated <see cref="TreeViewItem"/> control on <see cref="InputElement.DoubleTapped"/> event.
    /// </summary>
    public sealed class AutoCompleteBoxMonthBehavior : Behavior<AutoCompleteBox>
    {
        /// <summary>
        /// Called after the behavior is attached to the <see cref="Behavior.AssociatedObject"/>.
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();
            if (AssociatedObject != null)
            {
                AssociatedObject.KeyUp += TextTyped;
                AssociatedObject.LostFocus += LostFocus;
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
                AssociatedObject.KeyUp -= TextTyped;
                AssociatedObject.LostFocus -= LostFocus;
            }
        }

        private void LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (AssociatedObject != null)
                {
                    if (decimal.TryParse(new string(AssociatedObject.Text?.Where(char.IsDigit).ToArray()), out decimal ans))
                        AssociatedObject.Text = ans.ToString("00");
                }
            }
            catch { }
        }

        private void TextTyped(object sender, KeyEventArgs e)
        {
            try
            {
                if (AssociatedObject != null)
                {
                    if (e.Key != Key.Delete || e.Key != Key.Back)
                    {
                    }
                }
            }
            catch { }
        }
    }
}