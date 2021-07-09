using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Kfa.SubSystems.Cheques.Datalayer;
using Kfa.SubSystems.Cheques.Datalayer.Src.Models;
using Kfa.SubSystems.Cheques.DataLayer;

namespace Kfa.SubSystems.Cheques.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            using var db = new ChequesContext();
            db.Database.EnsureCreated();

            new Repository<CostCentre>().AddAsync(new Datalayer.CostCentre{ Description="WESTERN REGION", Narration ="PAID", Region ="E.AFRICA", SupplierCodePrefix="SSS9"  });
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            new LoginPage().Show();
            new ChequeBatchDetails().Show();

        }
    }
}
