using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MoveShareApplication.Models
{
    public class Picture
    {
        [Key]
        public string Picture_id { get; set; }

        [Required]
        public byte[] Pictures { get; set; }

        public DateTime Created_at { get; set; }

        //foreign key
        [Required]
        public string AssociatedItem_id { get; set; }
        [ForeignKey("AssociatedItem_id")]
        public Item? Item { get; set; }

    }
}
