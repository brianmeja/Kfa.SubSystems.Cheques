using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Kfa.SubSystems.Cheques.Views
{
    public partial class ChangePassword : Window
    {
        public ChangePassword()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
