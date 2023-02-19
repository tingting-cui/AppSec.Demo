using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace MoveShareApplication.Models
{
    public class Item
    {
        //[Required] attribute is not needed for value types such as DateTime, int, double, and float

        [Key]
        public string Item_id { get; set; }

        [Required]
        //foreign key
        public string Owner_id { get; set; }
        [ForeignKey("Owner_id")]
        public IdentityUser? ApplicationUser { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Name shall be less than 50 characters")]
        public string Name { get; set; }


        [StringLength(250, ErrorMessage = "Description shall be less than 250 characters")]
        public string? Description { get; set; }

        public DateTime Created_at { get; set; }

        public DateTime LastUpdate_at { get; set; }

        [Required]
        public bool Available { get; set; }

        [Range(1, 20, ErrorMessage = "Amount range between 1 to 20")]
        public int Quantity { get; set; }

        [Required]
        [StringLength(250, ErrorMessage = "Location info shall be less than 250 characters")]
        public string Location { get; set; }


        //Itended for time and address. sensitive info. Will be shown after order. To add GDPR relevant protection 
        public string? PickUpNote { get; set; }


        //One-Many, collection navigation property
        public ICollection<Picture>? Picture { get; set; }
        public ICollection<Order>? Order { get; set; }

    }


    /*
    public class ItemViewModel
    {
        public string Item_id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Name shall be less than 50 characters")]
        public string Name { get; set; }


        [StringLength(250, ErrorMessage = "Description shall be less than 250 characters")]
        public string? Description { get; set; }


        [Range(1, 20, ErrorMessage = "Amount range between 1 to 20")]
        public int Quantity { get; set; }


        [Required]
        [StringLength(250, ErrorMessage = "Location info shall be less than 250 characters")]
        public string Location { get; set; }


        //Itended for time and address. sensitive info. Will be shown after order. To add GDPR relevant protection
        public string? PickUpNote { get; set; }

    }
    */

}