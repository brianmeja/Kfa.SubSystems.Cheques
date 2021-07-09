﻿using Avalonia.Controls;

namespace Avalonia.ExtendedToolkit
{
    /// <summary>
    /// should be remove if avalonia supports naming of ColumnDefinition
    /// </summary>
    public class ColumnDefinitionExt: ColumnDefinition
    {
        /// <summary>
        /// get / get Name
        /// </summary>
        public string Name
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        /// <summary>
        /// <see cref="Name"/>
        /// </summary>
        public static readonly StyledProperty<string> NameProperty =
            AvaloniaProperty.Register<ColumnDefinitionExt, string>(nameof(Name));
    }
}
