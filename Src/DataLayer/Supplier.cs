using ReactiveUI.Fody.Helpers;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kfa.SubSystems.Cheques.Datalayer
{
    [Table("tbl_suppliers")]public class Supplier:KfaData
    {
       [Reactive,Column("supplier_id")]public override long Id{get;set;}
        [Reactive, Column("address")]public string Address{get;set;}
      [Reactive, Column("contact_person")]public string ContactPerson{get;set;}
       [Reactive, Column("cost_centre_code")]public long CostCentreCode{get;set;}
        [Reactive, Column("description")]public string Description{get;set;}
       [Reactive, Column("email")]public string Email{get;set;}
        [Reactive, Column("narration")]public string Narration{get;set;}
       [Reactive, Column("postal_code")]public string PostalCode{get;set;}
        [Reactive, Column("supplier_code")]public string SupplierCode{get;set;}
       [Reactive,Column("supplier_ledger_account")] public long SupplierLedgerAccountId{get;set;}
       [Reactive, Column("telephone")] public string Telephone{get;set;}
        [Reactive, Column("town")]public string Town{get;set;}
        [ForeignKey(nameof(CostCentreCode))] public virtual CostCentre CostCentre { get; set; }
        [ForeignKey(nameof(SupplierLedgerAccountId))] public virtual LedgerAccount LedgerAccount { get; set; }
    }
}