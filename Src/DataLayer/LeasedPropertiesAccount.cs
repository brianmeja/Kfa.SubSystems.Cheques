using Kfa.SubSystems.Cheques.Contracts.Data;
using ReactiveUI.Fody.Helpers;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kfa.SubSystems.Cheques.Datalayer
{
    [Table("tbl_leased_properties_accounts")]
    public class LeasedPropertiesAccount : KfaData
    {
        [Reactive, Column("leased_property_account_id")]
        public override long Id { get; set; }

        [Reactive, Column("Account_number")]
        public string AccountNumber { get; set; }

        [Reactive, Column("commencement_rent")]
        public decimal CommencementRent { get; set; }

        [Reactive, Column("cost_center_code")]
        public long CostCentreCode { get; set; }

        [Reactive, Column("current_rent")]
        public decimal CurrentRent { get; set; }

        [Reactive, Column("landlord_address")]
        public string LandlordAddress { get; set; }

        [Reactive, Column("last_review_date")]
        public DateTime LastReviewDate { get; set; }

        [Reactive, Column("leased_on")]
        public DateTime LeasedOn { get; set; }

        [Reactive, Column("ledger_account_id")]
        public long LedgerAccountId { get; set; }

        [Reactive, Column("narration")]
        public string Narration { get; set; }

        [ForeignKey(nameof(CostCentreCode))] public virtual CostCentre CostCentre { get; set; }
        [ForeignKey(nameof(LedgerAccountId))] public virtual LedgerAccount LedgerAccount { get; set; }
    }
}