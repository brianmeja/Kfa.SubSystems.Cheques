﻿using System.Diagnostics;
using Avalonia.Xaml.Interactivity;

namespace Avalonia.ExtendedToolkit.Helper
{
    
    #pragma warning disable CS1570 // XML-Comment has invalid chars
    ///<summary>
    /// add log functionality into a style
    /// (for testing only)
    ///
    /// in the schema put this lines:
    ///
    /// xmlns:helper="clr-namespace:Avalonia.ExtendedToolkit.Helper;assembly=Avalonia.ExtendedToolkit"
    /// xmlns:i="clr-namespace:Avalonia.Xaml.Interactivity;assembly=Avalonia.Xaml.Interactivity"
    ///
    /// in the resources of the style add theses lines:
    /// <helper:Behaviors  x:Key="debugTriggers">
    ///  <helper:DebugAction/>
    /// </helper:Behaviors>
    ///
    /// and add this line into the style:
    /// <Setter Property="helper:StyledInteraction.Behaviors" Value="{StaticResource debugTriggers}"/>
    /// </summary>
    public class DebugAction : Trigger<AvaloniaObject>
#pragma warning restore CS1570 // XML-Kommentar weist ein ungültiges Format auf
    {
        /// <summary>
        /// registered PropertyChanged on AssociatedObject 
        /// </summary>
        protected override void OnAttached()
        {
            AssociatedObject.PropertyChanged += AssociatedObject_PropertyChanged;
            base.OnAttached();
        }

        /// <summary>
        /// unregistered PropertyChanged on AssociatedObject 
        /// </summary>
        protected override void OnDetaching()
        {
            AssociatedObject.PropertyChanged -= AssociatedObject_PropertyChanged;
            base.OnDetaching();
        }

        private void AssociatedObject_PropertyChanged(object sender, AvaloniaPropertyChangedEventArgs e)
        {
            Debug.WriteLine($"AssociatedObject: {AssociatedObject} Property Name: {e.Property.Name}" +
                $" New Value: {e.NewValue} Old Value: {e.OldValue}");
        }
    }
}
