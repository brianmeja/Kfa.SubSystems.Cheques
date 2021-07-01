using ReactiveUI.Fody.Helpers;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kfa.SubSystems.Cheques.Datalayer
{
    [Table("tbl_paid_cheques")]public class PaidCheque:KfaData
    {
       [Reactive,Column("cheque_id")] public override long Id {get;set;}
      [Reactive , Column("amount")]public decimal Amount{get;set;}
      [Reactive,Column("batch_key")]public long BatchKey{get;set;}
      [Reactive, Column("cheque_number")]public string ChequeNumber{get;set;}
      [Reactive, Column("cost_center_code")]public long CostCentreCode{get;set;}
       [Reactive, Column("description")]public string Description{get;set;}
      [Reactive, Column("narration")]public string Narration{get;set;}
      [Reactive,Column("paid_cheque_credit_gl_id")]public long PaidChequeCreditGlId{get;set;}
      [Reactive,Column("paid_cheque_debit_gl_id")]public long PaidChequeDebitGlId{get;set;}
       [Reactive, Column("payee_details")]public string PayeeDetails{get;set;}
      [Reactive, Column("suppliers")]public long SupplierId{get;set;}
      [Reactive,Column("type")]public string Type{get;set;}
        [ForeignKey(nameof(CostCentreCode))] public virtual CostCentre CostCentre { get; set; }
        [ForeignKey(nameof(BatchKey))] public virtual ChequeRequisitionBatch GetChequeRequisitionBatch{ get; set; }
        [ForeignKey(nameof(PaidChequeCreditGlId))] public virtual GeneralLedgersDetail CreditGl { get; set; }
        [ForeignKey(nameof(PaidChequeDebitGlId))] public virtual GeneralLedgersDetail DebitGl { get; set; }
        [ForeignKey(nameof(SupplierId))] public virtual Supplier Supplier { get; set; }

    }
}