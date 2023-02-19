using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MoveShareApplication.Models
{
    public class Order
    {
        [Key]
        public string Order_id { get; set; }


        //foreign key
        [Required]
        public string Item_id { get; set; }
        [ForeignKey("Item_id")]
        public Item? Item { get; set; }

        public int Order_quantity { get; set; }

        public DateTime Created_at { get; set; }

        public DateTime LastUpdate_at { get; set; }

        //Range Created, Cancelled, Pickedup
        [Required]
        public string Status { get; set; }
        //public OrderStatus Status { get; set; }


        //foreign key, 
        //remove cascade deletion from generated migration
        [Required]
        public string Customer_id { get; set; }
        [ForeignKey("Customer_id")]
        public IdentityUser? ApplicationUser { get; set; }

        /*
        public enum OrderStatus
        {
            Created,
            Cancelled,
            Rejected
        }
        */
    }
}
