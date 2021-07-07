﻿using Avalonia.Controls;
using Avalonia.Media;

namespace Avalonia.ExtendedToolkit.Controls
{
    /// <summary>
    /// GroupBox attached properties
    /// </summary>
    public static class GroupBoxHelper
    {
        /// <summary>
        /// HeaderForeground AttachedProperty
        /// </summary>
        public static readonly AttachedProperty<IBrush> HeaderForegroundProperty =
        AvaloniaProperty.RegisterAttached<IControl, IBrush>("HeaderForeground", typeof(GroupBoxHelper));

        /// <summary>
        /// get HeaderForeground
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static IBrush GetHeaderForeground(IControl element)
        {
            return element.GetValue(HeaderForegroundProperty);
        }

        /// <summary>
        /// set HeaderForeground
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetHeaderForeground(IControl element, IBrush value)
        {
            element.SetValue(HeaderForegroundProperty, value);
        }
    }
}
