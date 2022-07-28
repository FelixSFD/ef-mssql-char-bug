using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ef_mssql_char_bug
{
  public class OrderModel
  {
    [Column(TypeName = "char(10)")]
    [MinLength(10)]
    [MaxLength(10)]
    [Key]
    public string OrderId { get; set; }


    public string Info { get; set; }


    public virtual ICollection<OrderItemModel> Items { get; set; }
  }
}
