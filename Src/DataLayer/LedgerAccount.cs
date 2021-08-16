using Kfa.SubSystems.Cheques.Contracts.Data;
using ReactiveUI.Fody.Helpers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kfa.SubSystems.Cheques.Datalayer
{
    [Table("tbl_ledger_accounts")]
    public class LedgerAccount : KfaData
    {
        [Reactive, Column("ledger_account_id")] public override long Id { get; set; }
        [Reactive, Column("description")] public string Description { get; set; }
        [Reactive, Column("group_name")] public string GroupName { get; set; }
        [Reactive, Column("cost_center_code")] public long CostCentreCode { get; set; }
        [Reactive, Column("increase_with_debit")] public bool IncreaseWithDebit { get; set; }
        [Reactive, Column("ledger_account_code")] public string LedgerAccountCode { get; set; }
        [Reactive, Column("main_group")] public string MainGroup { get; set; }
        [Reactive, Column("narration")] public string Narration { get; set; }
        [ForeignKey(nameof(CostCentreCode))] public virtual CostCentre CostCentre { get; set; }

        public ICollection<GeneralLedgersDetail> GeneralLedgerDetails { get; set; }
        public ICollection<LeasedPropertiesAccount> LeasedPropertyAccounts { get; set; }
        public ICollection<Supplier> Suppliers { get; set; }
    }
}