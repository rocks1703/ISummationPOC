using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISummationPOC.Entity
{

    [Table("mst_usertypes")]
    public class UserTypes
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("usertype")]
        public string UserType { get; set; }
    }
}
