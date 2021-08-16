using Kfa.SubSystems.Cheques.Contracts.Data;
using ReactiveUI.Fody.Helpers;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kfa.SubSystems.Cheques.Datalayer
{
    [Table("tbl_user_roles")]
    public class UserRole : KfaData
    {
        [Reactive, Column("role_id")] public override long Id { get; set; }
        [Reactive, Column("expirational_date")] public DateTime ExpirationDate { get; set; }
        [Reactive, Column("maturity_date")] public DateTime MaturityDate { get; set; }
        [Reactive, Column("narration")] public string Narration { get; set; }
        [Reactive, Column("role_name")] public string RoleName { get; set; }
    }
}