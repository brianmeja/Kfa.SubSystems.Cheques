using Aura.UI.Controls.Navigation;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Kfa.SubSystems.Cheques.Contracts;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive.Disposables;

namespace Kfa.SubSystems.Cheques.Views
{
    public partial class MainView : ReactiveUserControl<MainViewViewModel>
    {
        public NavigationView NavigationView { get; }

        public MainView()
        {
            InitializeComponent();
            DataContext = new MainViewViewModel();
            this.NavigationView = this.FindControl<NavigationView>("NavigationView");
            this.WhenActivated(this.PageActivated);
        }

        private void PageActivated(CompositeDisposable disposal)
        {
            NavigationView.Bind(NavigationView.IsOpenProperty, Declarations.NavigationViewIsOpen).DisposeWith(disposal);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

    }
}
