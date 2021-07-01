using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kfa.SubSystems.Cheques.Datalayer
{
    [Table("tbl_general_ledgers_details")]public class GeneralLedgersDetail:KfaData
    {
      [Reactive, Column("gl_id")]public override long Id {get; set;}
     [Reactive, Column("balance")]public decimal Balance {get; set;}
     [Reactive, Column("class_of_card")]public string ClassOfCard {get; set;}
    [Reactive, Column("cost_center_code")]public long CostCentreCode {get; set;}

     [Reactive,Column("credit_amount")]public decimal CreditAmount {get; set;}
     [Reactive, Column("date")]public DateTime Date {get; set;}
     [Reactive, Column("debit_amount")]public decimal DebitAmount { get; set;}
     [Reactive, Column("gl_type")]public string GlType {get; set;}
     [Reactive, Column("ledger_account_id")]public long LedgerAccountId{get;set;}
    [Reactive, Column("narration")] public string Narration{get;set;}
     [Reactive, Column("particulars")]public string Particulars{get;set;}
     [Reactive, Column("transaction_group")]public string TransactionGroup{get;set;}
        [ForeignKey(nameof(CostCentreCode))] public virtual CostCentre CostCentre { get; set; }
        [ForeignKey(nameof(LedgerAccountId))] public virtual LedgerAccount LedgerAccount { get; set; }
        public ICollection<PaidCheque> CreditPaidCheques { get; set; }
        public ICollection<PaidCheque> DebitPaidCheques { get; set; }

    }
        
}