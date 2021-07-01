//import essential element to be used
using System;
using System.ComponentModel.DataAnnotations.Schema;
using ReactiveUI.Fody.Helpers;

namespace Kfa.SubSystems.Cheques.Datalayer



{//creating an abstract class with data items
    public abstract class KfaData
    {
       [Reactive, Column("date_added")] 
       public DateTime DateAdded { get; set; }

       [Reactive, Column("date_updated")] 
       public DateTime DateUpdated { get; set; }
       [Reactive, Column("originator_id")]
       public long OriginatorId { get; set; }
       [Reactive] public abstract long Id { get; set; }
    }  
}
