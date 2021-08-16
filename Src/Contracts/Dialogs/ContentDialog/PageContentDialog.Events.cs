using Avalonia.Interactivity;
using System;

namespace Aura.UI.Controls
{
    public partial class PageContentDialog
    {
        public event EventHandler<RoutedEventArgs> OkButtonClick
        {
            add => AddHandler(OkButtonClickEvent, value);
            remove => RemoveHandler(OkButtonClickEvent, value);
        }

        public static readonly RoutedEvent<RoutedEventArgs> OkButtonClickEvent =
            RoutedEvent.Register<PageContentDialog, RoutedEventArgs>(nameof(OkButtonClick), RoutingStrategies.Bubble);
    }
}