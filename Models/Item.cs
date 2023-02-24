//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace auctionSystem.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Web;

    public partial class Item
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Item()
        {
            this.BidTables = new HashSet<BidTable>();
        }
    
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public Nullable<decimal> ItemPrice { get; set; }
        public Nullable<int> itemQuantity { get; set; }
        public string state { get; set; }
        public string picture { get; set; }
        public Nullable<System.DateTime> open_date_time { get; set; }
        public Nullable<decimal> increment { get; set; }
        public string lastbidder { get; set; }
        public Nullable<System.DateTime> close_date_time { get; set; }
        public string ItemDesc { get; set; }
        public Nullable<int> UserId { get; set; }
        public string UserName { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        [NotMapped]
        public HttpPostedFileBase PictureToUpload { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BidTable> BidTables { get; set; }
        public virtual User User { get; set; }
    }
}
