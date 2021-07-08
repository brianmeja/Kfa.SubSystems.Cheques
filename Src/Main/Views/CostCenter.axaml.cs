using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Kfa.SubSystems.Cheques.Views
{
    public partial class CostCenter : Window
    {
        public CostCenter()
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
