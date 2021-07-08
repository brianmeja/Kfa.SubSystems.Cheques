using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Kfa.SubSystems.Cheques.DataLayer
{
    public partial class LedgerAccounts : Window
    {
        public LedgerAccounts()
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
