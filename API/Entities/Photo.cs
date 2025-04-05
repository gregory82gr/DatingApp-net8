using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities{

    [Table("Photos")]
    
    public class Photo
    {
        public int Id { get; set; }
        public required string Url { get; set; } 
        public DateTime DateAdded { get; set; } = DateTime.Now;
        public bool IsMain { get; set; } 
        public string? PublicId { get; set; } 

        //Navigation properties
        public int AppUserId { get; set; }
        [ForeignKey(nameof(AppUserId))]
        public AppUser AppUser { get; set; } = null!;
    
    }
}