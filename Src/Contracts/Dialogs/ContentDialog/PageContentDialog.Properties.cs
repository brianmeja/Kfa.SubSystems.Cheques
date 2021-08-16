using Avalonia;

namespace Aura.UI.Controls
{
    public partial class PageContentDialog
    {
        public object OkButtonContent
        {
            get => GetValue(OkButtonContentProperty);
            set => SetValue(OkButtonContentProperty, value);
        }

        public readonly static StyledProperty<object> OkButtonContentProperty =
            AvaloniaProperty.Register<PageContentDialog, object>(nameof(OkButtonContent), "Ok");
    }
}