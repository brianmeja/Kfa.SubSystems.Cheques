using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;
using System.Linq;
using System.Text.RegularExpressions;

namespace Kfa.SubSystems.Cheques.Contracts.Behaviors
{
    /// <summary>
    /// Toggles <see cref="TreeViewItem.IsExpanded"/> property of the associated <see cref="TreeViewItem"/> control on <see cref="InputElement.DoubleTapped"/> event.
    /// </summary>
    public sealed class TextBoxDateBehaviour : Behavior<TextBox>
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
        }

        private void TextTyped(object sender, KeyEventArgs e)
        {
            if (AssociatedObject != null)
            {
                if (e.Key != Key.Delete || e.Key != Key.Back)
                {
                    var numbers = Regex.Split(AssociatedObject.Text ?? "", @"\D+")
                        .Select(x => int.TryParse(x, out int num) ? num : 0)
                        .Where(x => x > 0)
                        .ToArray();
                }
            }
        }
    }
}