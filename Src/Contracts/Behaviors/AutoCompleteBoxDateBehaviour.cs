using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Kfa.SubSystems.Cheques.Contracts.Behaviors
{
    /// <summary>
    /// Toggles <see cref="TreeViewItem.IsExpanded"/> property of the associated <see cref="TreeViewItem"/> control on <see cref="InputElement.DoubleTapped"/> event.
    /// </summary>
    public sealed class AutoCompleteBoxDateBehaviour : Behavior<AutoCompleteBox>
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
                    var text = AssociatedObject.Text;
                    var numbers = Regex.Split(AssociatedObject.Text ?? "", @"\D+")
                        .Select(x => int.TryParse(x, out int num) ? num : 0)
                        .Where(x => x > 0)
                        .ToArray();

                    static string getYear(string ans)
                    {
                        if (short.TryParse(ans, out short oo))
                        {
                            if (100 > oo)
                            {
                                var mm = Convert.ToInt32((DateTime.Now.Year + 3).ToString().Substring(2));
                                return oo < mm ? "20" + oo : "19" + oo;
                            }
                            else
                            {
                                var mm = Convert.ToInt32($"{ans}0000".Substring(0, 4));
                                return mm.ToString("0000");
                            }
                        }
                        return ans;
                    }
                    if (numbers.Length == 3)
                    {
                        try
                        {
                            AssociatedObject.Text = new DateTime(Convert.ToInt32(getYear(numbers[2].ToString())), numbers[1], numbers[0]).ToString("dd-MM-yyyy");
                            return;
                        }
                        catch { }
                        try
                        {
                            AssociatedObject.Text = new DateTime(Convert.ToInt32(getYear(numbers[0].ToString())), numbers[1], numbers[2]).ToString("dd-MM-yyyy");
                            return;
                        }
                        catch { }
                    }
                    else if (numbers.Length == 1 && text?.Length > 1)
                    {
                        if (byte.TryParse(text.Substring(0, 2), out byte mn) && mn > 0 && mn < 32)
                        {
                            if (byte.TryParse(text.Substring(2, 2), out byte kn) && kn > 0 && kn < 13)
                                text = $"{text.Substring(0, 2)}-{text.Substring(2, 2)}-{getYear(text[4..])}";
                            else if (byte.TryParse(text.Substring(2, 1), out byte kn1) && kn1 > 0 && kn1 < 13)
                                text = $"{text.Substring(0, 2)}-{kn1:00}-{getYear(text[3..])}";
                        }
                        else if (byte.TryParse(text.Substring(0, 1), out byte mn1) && mn1 > 0 && mn1 < 32)
                        {
                            if (byte.TryParse(text.Substring(1, 2), out byte kn) && kn > 0 && kn < 13)
                                text = $"{mn1:00}-{text.Substring(1, 2)}-{getYear(text[3..])}";
                            else if (byte.TryParse(text.Substring(1, 1), out byte kn1) && kn1 > 0 && kn1 < 13)
                                text = $"{mn1:00}-{kn1:00}-{getYear(text[2..])}";
                        }
                        AssociatedObject.Text = text;
                    }
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