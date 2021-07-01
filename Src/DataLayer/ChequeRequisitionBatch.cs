using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kfa.SubSystems.Cheques.Datalayer
{
    [Table("tbl_cheque_requisition_batches")]
    public class ChequeRequisitionBatch:KfaData
    {
       [Reactive,Column("batch_key")]public override long Id{get;set;}
       [Reactive,Column ("batch_number")]public string BatchNumber{get;set;}
       [Reactive,Column("class_of_card")]public string ClassOfCard{get;set;}
       [Reactive,Column("date")]public DateTime Date{get;set;}
       [Reactive,Column("month")]public string Month {get;set;}
       [Reactive,Column("narration")]public string Narration{get;set;}
       [Reactive,Column("no_of_documents")]public bool NoOfDocuments{get;set;}
       [Reactive,Column("total_amount")]public decimal TotalAmount{get;set;}
       [Reactive,Column("cost_centre_code")]public long CostCentreCode { get; set; }
       [ForeignKey(nameof(CostCentreCode))]public virtual CostCentre CostCentre { get; set; }
        public ICollection<PaidCheque> PaidCheques { get; set; }
        public ICollection<Supplier> Suppliers { get; set; }
           
            

    }
}