using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Kfa.SubSystems.Cheques.ViewModels;

namespace Kfa.SubSystems.Cheques.Views
{
    public class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
       // public int MyProperty { get; set; }
        public MainWindow()
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
