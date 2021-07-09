﻿using System;
using System.Globalization;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.ExtendedToolkit.Controls;

namespace Avalonia.ExtendedToolkit.Converters
{
    /// <summary>
    /// tries to convert the <see cref="UnderlinedType"/> to bool
    /// </summary>
    public class UnderlinedTypeToBoolConverter : IValueConverter
    {
        /// <summary>
        /// if value is <see cref="IControl"/> and parameter is <see cref="UnderlinedType"/>
        /// and the underlining type is equal return true
        /// else false
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IControl control = value as IControl;
            if (control != null && parameter is UnderlinedType)
            {
                UnderlinedType underlinedTypeParam = (UnderlinedType)parameter;

                UnderlinedType underlinedType = TabControlHelper.GetUnderlined(control);
                if (underlinedType == underlinedTypeParam)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// does nothing
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return AvaloniaProperty.UnsetValue;
        }
    }
}
