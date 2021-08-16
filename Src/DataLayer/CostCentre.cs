using Kfa.SubSystems.Cheques.Contracts.Data;
using ReactiveUI.Fody.Helpers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kfa.SubSystems.Cheques.Datalayer
{
    [Table("tbl_cost_centres")]
    public class CostCentre : KfaData
    {
        [Reactive, Column("cost_centre_code")]
        public override long Id { get; set; }

        [Reactive, Column("description")]
        public string Description { get; set; }

        [Reactive, Column("narration")]
        public string Narration { get; set; }

        [Reactive, Column("region")]
        public string Region { get; set; }

        [Reactive, Column("supplier_code_prefix")]
        public string SupplierCodePrefix { get; set; }

        // [Reactive, Column("record_comment_id")]
        // public string RecordCommentId { get; set;}
        //[Reactive, Column("record_verification_id")]
        // public string RecordVerificationId { get; set;}

        public ICollection<ChequeRequisitionBatch> ChequeRequisitionBatches { get; set; }
        public ICollection<GeneralLedgersDetail> GeneralLedgersDetails { get; set; }
        public ICollection<LeasedPropertiesAccount> LeasedPropertiesAccounts { get; set; }
        public ICollection<PaidCheque> PaidCheques { get; set; }
        public ICollection<Supplier> Suppliers { get; set; }
        public ICollection<LedgerAccount> LedgerAccounts { get; set; }
    }
}