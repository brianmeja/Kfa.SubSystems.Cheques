using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Kfa.SubSystems.Cheques.Views
{
    public partial class Chequerequisitionbatches : Window
    {
        public Chequerequisitionbatches()
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
