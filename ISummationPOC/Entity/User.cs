using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISummationPOC.Entity
{
    [Table("mst_user")]
    public class User
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("usertypeid")]
        public int UserTypeId { get; set; }       

        [Column("firstname")]

        public string FirstName { get; set; }
        [Column("lastname")]

        public string LastName { get; set; }
        [Column("email")]

        public string Email { get; set; }
        [Column("mobile")]

        public string Mobile { get; set; }

        [Column("profileimage")]
        public string ? ProfileImage { get; set; } 
 
    }
}
